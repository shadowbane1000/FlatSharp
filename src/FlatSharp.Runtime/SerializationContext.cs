﻿/*
 * Copyright 2018 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// A context object for a FlatBuffer serialize operation. The context is responsible for allocating space in the buffer
    /// and managing the latest offset.
    /// </summary>
    public sealed class SerializationContext
    {
        private const int VTableBucketCount = 64;

        /// <summary>
        /// A delegate to invoke after the serialization process has completed. Used for sorting vectors.
        /// </summary>
        public delegate void PostSerializeAction(Span<byte> span, SerializationContext context);

        internal static readonly ThreadLocal<SerializationContext> ThreadLocalContext = new ThreadLocal<SerializationContext>(() => new SerializationContext());

        private int offset;
        private int capacity;
        private readonly List<PostSerializeAction> postSerializeActions;
        private readonly List<int>[] vtableOffsets;

        /// <summary>
        /// Initializes a new serialization context.
        /// </summary>
        public SerializationContext()
        {
            this.postSerializeActions = new List<PostSerializeAction>();
            this.vtableOffsets = new List<int>[VTableBucketCount];

            for (int i = 0; i < VTableBucketCount; ++i)
            {
                this.vtableOffsets[i] = new List<int>();
            }
        }

        /// <summary>
        /// The maximum offset within the buffer.
        /// </summary>
        public int Offset
        {
            get => this.offset;
            set => this.offset = value;
        }

        /// <summary>
        /// The shared string writer used for this serialization operation.
        /// </summary>
        public ISharedStringWriter? SharedStringWriter { get; set; }

        /// <summary>
        /// Resets the context.
        /// </summary>
        public void Reset(int capacity)
        {
            this.offset = 0;
            this.capacity = capacity;
            this.SharedStringWriter = null;
            this.postSerializeActions.Clear();

            var offsets = this.vtableOffsets;
            for (int i = 0; i < offsets.Length; ++i)
            {
                offsets[i].Clear();
            }
        }

        /// <summary>
        /// Invokes any post-serialize actions.
        /// </summary>
        public void InvokePostSerializeActions(Span<byte> span)
        {
            var actions = this.postSerializeActions;
            int count = actions.Count;

            for (int i = 0; i < count; ++i)
            {
                actions[i](span, this);
            }
        }

        public void AddPostSerializeAction(PostSerializeAction action)
        {
            this.postSerializeActions.Add(action);
        }

        /// <summary>
        /// Allocate a vector and return the index. Does not populate any details of the vector.
        /// </summary>
        public int AllocateVector(int itemAlignment, int numberOfItems, int sizePerItem)
        {
            checked
            {
                if (numberOfItems < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(numberOfItems));
                }

                int bytesNeeded = numberOfItems * sizePerItem + sizeof(uint);

                // Vectors have a size uoffset_t, followed by N items. The uoffset_t needs to be 4 byte aligned, while the items need to be N byte aligned.
                // So, if the items are double or long, the length field has 4 byte alignment, but the item field has 8 byte alignment.
                // This means that we need to choose an offset such that:
                // (lengthIndex) % 4 == 0
                // (lengthIndex + 4) % N == 0
                //
                // Obviously, if N <= 4 this is trivial. If N = 8, it gets a bit more interesting.
                // First, align the offset to 4.
                int offset = this.offset;
                offset += SerializationHelpers.GetAlignmentError(offset, sizeof(uint));

                // Now, align offset + 4 to item alignment.
                offset += SerializationHelpers.GetAlignmentError(offset + sizeof(uint), itemAlignment);
                this.offset = offset;

                offset = this.AllocateSpace(bytesNeeded, sizeof(uint));

                Debug.Assert(offset % 4 == 0);
                Debug.Assert((offset + 4) % itemAlignment == 0);

                return offset;
            }
        }

        /// <summary>
        /// Allocates a block of memory. Returns the offset.
        /// </summary>
        public int AllocateSpace(int bytesNeeded, int alignment)
        {
            checked
            {
                int offset = this.offset;
                Debug.Assert(alignment == 1 || alignment % 2 == 0);

                offset += SerializationHelpers.GetAlignmentError(offset, alignment);

                int finalOffset = offset + bytesNeeded;
                if (finalOffset >= this.capacity)
                {
                    throw new BufferTooSmallException();
                }

                this.offset = finalOffset;
                return offset;
            }
        }

        public int FinishVTable<TSpanWriter>(
            TSpanWriter writer,
            int tableLength,
            Span<byte> buffer,
            Span<byte> vtable) where TSpanWriter : ISpanWriter
        {
            checked
            {
                // write table length.
                writer.WriteUShort(vtable, (ushort)tableLength, sizeof(ushort), this);

                int index = FindLastNonZeroValueIndex(vtable.Slice(2 * sizeof(ushort)));
                vtable = vtable.Slice(0, 4 + index);

                writer.WriteUShort(vtable, (ushort)vtable.Length, 0, this);

                List<int> bucket = vtableOffsets[GetHash(vtable) % VTableBucketCount];
                int count = bucket.Count;

                for (int i = 0; i < count; ++i)
                {
                    int offset = bucket[i];

                    ReadOnlySpan<byte> existingVTable = buffer.Slice(offset);
                    existingVTable = existingVTable.Slice(0, ScalarSpanReader.ReadUShort(existingVTable));

                    if (CompareEquality(existingVTable, vtable))
                    {
                        // Slowly bubble used things towards the front of the list.
                        // This is not exact, but should keep frequently used
                        // items towards the front. We have 64 separate buckets, which
                        // means that items should experience low contention.
                        if (i != 0)
                        {
                            Promote(i, bucket);
                        }

                        return offset;
                    }
                }

                // Oh, well. Write the new table.
                int newVTableOffset = this.AllocateSpace(vtable.Length, sizeof(ushort));
                vtable.CopyTo(buffer.Slice(newVTableOffset));
                bucket.Add(newVTableOffset);

                // "Insert" this item in the middle of the list.
                int maxIndex = bucket.Count - 1;
                Promote(maxIndex, bucket);

                return newVTableOffset;
            }
        }

        /// <summary>
        /// Gets a hash code based on the first 64 bits of the vtable.
        /// If the vtable is not 64 bits long, the length of the vtable is used.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHash(ReadOnlySpan<byte> vtable)
        {
            int length = vtable.Length;
            if (length >= sizeof(ulong))
            {
                ulong value = ScalarSpanReader.ReadULong(vtable);
                ulong high = value >> 32;
                ulong low = value & uint.MaxValue;

                // force positive
                return (int)(high ^ low) & int.MaxValue;
            }

            return length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int FindLastNonZeroValueIndex(ReadOnlySpan<byte> values)
        {
            Debug.Assert(values.Length % 2 == 0);

            int length = values.Length;
            int tmp;

            while (length >= sizeof(ulong) &&
                   ScalarSpanReader.ReadULong(values.Slice(tmp = length - sizeof(ulong))) == 0)
            {
                length = tmp;
            }

            if (length >= sizeof(uint) &&
                ScalarSpanReader.ReadUInt(values.Slice(tmp = length - sizeof(uint))) == 0)
            {
                length = tmp;
            }

            if (length >= sizeof(ushort) &&
                ScalarSpanReader.ReadUShort(values.Slice(tmp = length - sizeof(ushort))) == 0)
            {
                length = tmp;
            }

            return length;
        }

        /// <summary>
        /// Promote frequently-used items to be closer to the front of the list.
        /// This is done with a swap to avoid shuffling the whole list by inserting
        /// at a given index. An alternative might be an unrolled linked list data structure.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Promote(int i, List<int> offsets)
        {
            int swapIndex = i / 2;

            int temp = offsets[i];
            offsets[i] = offsets[swapIndex];
            offsets[swapIndex] = temp;
        }

        /// <summary>
        /// Possible to use SIMD intrinsics here, but they often end up hurting performance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CompareEquality(ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            int length = left.Length;
            int offset = 0;

            if (length != right.Length)
            {
                return false;
            }

            while (length >= sizeof(ulong))
            {
                if (ScalarSpanReader.ReadULong(left.Slice(offset)) != ScalarSpanReader.ReadULong(right.Slice(offset)))
                {
                    return false;
                }

                offset += sizeof(ulong);
                length -= sizeof(ulong);
            }

            if (length >= sizeof(uint))
            {
                if (ScalarSpanReader.ReadUInt(left.Slice(offset)) != ScalarSpanReader.ReadUInt(right.Slice(offset)))
                {
                    return false;
                }

                offset += sizeof(uint);
                length -= sizeof(uint);
            }

            if (length >= sizeof(ushort))
            {
                if (ScalarSpanReader.ReadUShort(left.Slice(offset)) != ScalarSpanReader.ReadUShort(right.Slice(offset)))
                {
                    return left[offset] == right[offset];
                }

                offset += sizeof(ushort);
                length -= sizeof(ushort);
            }

            return length == 0 || left[offset] == right[offset];
        }
    }
}
