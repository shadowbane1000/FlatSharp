// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatSharpTests.Oracle
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct VectorOfUnionTable : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static VectorOfUnionTable GetRootAsVectorOfUnionTable(ByteBuffer _bb) { return GetRootAsVectorOfUnionTable(_bb, new VectorOfUnionTable()); }
  public static VectorOfUnionTable GetRootAsVectorOfUnionTable(ByteBuffer _bb, VectorOfUnionTable obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public VectorOfUnionTable __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FlatSharpTests.Oracle.Union ValueType(int j) { int o = __p.__offset(4); return o != 0 ? (FlatSharpTests.Oracle.Union)__p.bb.Get(__p.__vector(o) + j * 1) : (FlatSharpTests.Oracle.Union)0; }
  public int ValueTypeLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<FlatSharpTests.Oracle.Union> GetValueTypeBytes() { return __p.__vector_as_span<FlatSharpTests.Oracle.Union>(4, 1); }
#else
  public ArraySegment<byte>? GetValueTypeBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public FlatSharpTests.Oracle.Union[] GetValueTypeArray() { int o = __p.__offset(4); if (o == 0) return null; int p = __p.__vector(o); int l = __p.__vector_len(o); FlatSharpTests.Oracle.Union[] a = new FlatSharpTests.Oracle.Union[l]; for (int i = 0; i < l; i++) { a[i] = (FlatSharpTests.Oracle.Union)__p.bb.Get(p + i * 1); } return a; }
  public TTable? Value<TTable>(int j) where TTable : struct, IFlatbufferObject { int o = __p.__offset(6); return o != 0 ? (TTable?)__p.__union<TTable>(__p.__vector(o) + j * 4) : null; }
  public string ValueAsString(int j) { int o = __p.__offset(6); return o != 0 ? __p.__string(__p.__vector(o) + j * 4) : null; }
  public int ValueLength { get { int o = __p.__offset(6); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<FlatSharpTests.Oracle.VectorOfUnionTable> CreateVectorOfUnionTable(FlatBufferBuilder builder,
      VectorOffset Value_typeOffset = default(VectorOffset),
      VectorOffset ValueOffset = default(VectorOffset)) {
    builder.StartTable(2);
    VectorOfUnionTable.AddValue(builder, ValueOffset);
    VectorOfUnionTable.AddValueType(builder, Value_typeOffset);
    return VectorOfUnionTable.EndVectorOfUnionTable(builder);
  }

  public static void StartVectorOfUnionTable(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddValueType(FlatBufferBuilder builder, VectorOffset ValueTypeOffset) { builder.AddOffset(0, ValueTypeOffset.Value, 0); }
  public static VectorOffset CreateValueTypeVector(FlatBufferBuilder builder, FlatSharpTests.Oracle.Union[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte((byte)data[i]); return builder.EndVector(); }
  public static VectorOffset CreateValueTypeVectorBlock(FlatBufferBuilder builder, FlatSharpTests.Oracle.Union[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static void StartValueTypeVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddValue(FlatBufferBuilder builder, VectorOffset ValueOffset) { builder.AddOffset(1, ValueOffset.Value, 0); }
  public static VectorOffset CreateValueVector(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateValueVectorBlock(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartValueVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<FlatSharpTests.Oracle.VectorOfUnionTable> EndVectorOfUnionTable(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatSharpTests.Oracle.VectorOfUnionTable>(o);
  }
};


}