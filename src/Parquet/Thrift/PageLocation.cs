#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
/*
 * Autogenerated by Thrift Compiler (0.10.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;

using Thrift.Protocol;
using Thrift.Transport;

namespace Parquet.Thrift
{


  public partial class PageLocation : TBase
  {

    /// <summary>
    /// Offset of the page in the file *
    /// </summary>
    public long Offset { get; set; }

    /// <summary>
    /// Size of the page, including header. Sum of compressed_page_size and header
    /// length
    /// </summary>
    public int Compressed_page_size { get; set; }

    /// <summary>
    /// Index within the RowGroup of the first row of the page; this means pages
    /// change on record boundaries (r = 0).
    /// </summary>
    public long First_row_index { get; set; }

    public PageLocation() {
    }

    public PageLocation(long offset, int compressed_page_size, long first_row_index) : this() {
      this.Offset = offset;
      this.Compressed_page_size = compressed_page_size;
      this.First_row_index = first_row_index;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_offset = false;
        bool isset_compressed_page_size = false;
        bool isset_first_row_index = false;
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.I64) {
                Offset = iprot.ReadI64();
                isset_offset = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I32) {
                Compressed_page_size = iprot.ReadI32();
                isset_compressed_page_size = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I64) {
                First_row_index = iprot.ReadI64();
                isset_first_row_index = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
        if (!isset_offset)
          throw new TProtocolException(TProtocolException.INVALID_DATA);
        if (!isset_compressed_page_size)
          throw new TProtocolException(TProtocolException.INVALID_DATA);
        if (!isset_first_row_index)
          throw new TProtocolException(TProtocolException.INVALID_DATA);
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("PageLocation");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        field.Name = "offset";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Offset);
        oprot.WriteFieldEnd();
        field.Name = "compressed_page_size";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Compressed_page_size);
        oprot.WriteFieldEnd();
        field.Name = "first_row_index";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(First_row_index);
        oprot.WriteFieldEnd();
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("PageLocation(");
      __sb.Append(", Offset: ");
      __sb.Append(Offset);
      __sb.Append(", Compressed_page_size: ");
      __sb.Append(Compressed_page_size);
      __sb.Append(", First_row_index: ");
      __sb.Append(First_row_index);
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member