// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatSharpTests.Oracle
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct UnionVectorTable : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static UnionVectorTable GetRootAsUnionVectorTable(ByteBuffer _bb) { return GetRootAsUnionVectorTable(_bb, new UnionVectorTable()); }
  public static UnionVectorTable GetRootAsUnionVectorTable(ByteBuffer _bb, UnionVectorTable obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public UnionVectorTable __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

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

  public static Offset<FlatSharpTests.Oracle.UnionVectorTable> CreateUnionVectorTable(FlatBufferBuilder builder,
      VectorOffset Value_typeOffset = default(VectorOffset),
      VectorOffset ValueOffset = default(VectorOffset)) {
    builder.StartTable(2);
    UnionVectorTable.AddValue(builder, ValueOffset);
    UnionVectorTable.AddValueType(builder, Value_typeOffset);
    return UnionVectorTable.EndUnionVectorTable(builder);
  }

  public static void StartUnionVectorTable(FlatBufferBuilder builder) { builder.StartTable(2); }
  public static void AddValueType(FlatBufferBuilder builder, VectorOffset ValueTypeOffset) { builder.AddOffset(0, ValueTypeOffset.Value, 0); }
  public static VectorOffset CreateValueTypeVector(FlatBufferBuilder builder, FlatSharpTests.Oracle.Union[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte((byte)data[i]); return builder.EndVector(); }
  public static VectorOffset CreateValueTypeVectorBlock(FlatBufferBuilder builder, FlatSharpTests.Oracle.Union[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static void StartValueTypeVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddValue(FlatBufferBuilder builder, VectorOffset ValueOffset) { builder.AddOffset(1, ValueOffset.Value, 0); }
  public static VectorOffset CreateValueVector(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateValueVectorBlock(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartValueVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<FlatSharpTests.Oracle.UnionVectorTable> EndUnionVectorTable(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatSharpTests.Oracle.UnionVectorTable>(o);
  }
  public UnionVectorTableT UnPack() {
    var _o = new UnionVectorTableT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(UnionVectorTableT _o) {
    _o.Value = new List<FlatSharpTests.Oracle.UnionUnion>();
    for (var _j = 0; _j < this.ValueLength; ++_j) {
      var _o_Value = new FlatSharpTests.Oracle.UnionUnion();
      _o_Value.Type = this.ValueType(_j);
      switch (this.ValueType(_j)) {
        default: break;
        case FlatSharpTests.Oracle.Union.BasicTypes:
          _o_Value.Value = this.Value<FlatSharpTests.Oracle.BasicTypes>(_j).HasValue ? this.Value<FlatSharpTests.Oracle.BasicTypes>(_j).Value.UnPack() : null;
          break;
        case FlatSharpTests.Oracle.Union.Location:
          _o_Value.Value = this.Value<FlatSharpTests.Oracle.Location>(_j).HasValue ? this.Value<FlatSharpTests.Oracle.Location>(_j).Value.UnPack() : null;
          break;
        case FlatSharpTests.Oracle.Union.stringValue:
          _o_Value.Value = this.ValueAsString(_j);
          break;
      }
      _o.Value.Add(_o_Value);
    }
  }
  public static Offset<FlatSharpTests.Oracle.UnionVectorTable> Pack(FlatBufferBuilder builder, UnionVectorTableT _o) {
    if (_o == null) return default(Offset<FlatSharpTests.Oracle.UnionVectorTable>);
    var _Value_type = default(VectorOffset);
    if (_o.Value != null) {
      var __Value_type = new FlatSharpTests.Oracle.Union[_o.Value.Count];
      for (var _j = 0; _j < __Value_type.Length; ++_j) { __Value_type[_j] = _o.Value[_j].Type; }
      _Value_type = CreateValueTypeVector(builder, __Value_type);
    }
    var _Value = default(VectorOffset);
    if (_o.Value != null) {
      var __Value = new int[_o.Value.Count];
      for (var _j = 0; _j < __Value.Length; ++_j) { __Value[_j] = FlatSharpTests.Oracle.UnionUnion.Pack(builder,  _o.Value[_j]); }
      _Value = CreateValueVector(builder, __Value);
    }
    return CreateUnionVectorTable(
      builder,
      _Value_type,
      _Value);
  }
};

public class UnionVectorTableT
{
  public List<FlatSharpTests.Oracle.UnionUnion> Value { get; set; }

  public UnionVectorTableT() {
    this.Value = null;
  }
}


}
