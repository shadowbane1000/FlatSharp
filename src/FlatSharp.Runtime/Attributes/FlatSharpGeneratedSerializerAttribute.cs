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

namespace FlatSharp.Attributes;

/// <summary>
/// Stores the set of flags used to generate a serializer.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class FlatSharpGeneratedSerializerAttribute : Attribute
{
    public FlatSharpGeneratedSerializerAttribute(FlatBufferDeserializationOption deserializeOption)
    {
        this.DeserializationOption = deserializeOption;
    }

    /// <summary>
    /// The flags specified when this serializer was generated.
    /// </summary>
    public FlatBufferDeserializationOption DeserializationOption { get; }
}
