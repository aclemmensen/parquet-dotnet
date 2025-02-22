﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;

namespace Parquet.Data
{
   /// <summary>
   /// Handler for built-in data types in .NET
   /// </summary>
   abstract class BasicPrimitiveDataTypeHandler<TSystemType> : BasicDataTypeHandler<TSystemType>
      where TSystemType : struct
   {
      private static readonly ArrayPool<int> IntPool = ArrayPool<int>.Shared;
      private static readonly IComparer<TSystemType> Comparer = Comparer<TSystemType>.Default;
      private static readonly IEqualityComparer<TSystemType> EqualityComparer = EqualityComparer<TSystemType>.Default;

      public BasicPrimitiveDataTypeHandler(DataType dataType, Thrift.Type thriftType, Thrift.ConvertedType? convertedType = null)
         : base(dataType, thriftType, convertedType)
      {
      }

      public override Array GetArray(int minCount, bool rent, bool isNullable)
      {
         if(rent)
         {
            return isNullable
               ? ArrayPool<TSystemType?>.Shared.Rent(minCount)
               : ArrayPool<TSystemType?>.Shared.Rent(minCount);
         }

         if (isNullable)
         {
            return new TSystemType?[minCount];
         }
         return new TSystemType[minCount];
      }

      public override ArrayView PackDefinitions(Array data, int offset, int count, int maxDefinitionLevel, out int[] definitions, out int definitionsLength, out int nullCount)
      {
         return PackDefinitions((TSystemType?[])data, offset, count, maxDefinitionLevel, out definitions, out definitionsLength, out nullCount);
      }

      public override Array UnpackDefinitions(Array untypedSource, int[] definitionLevels, int maxDefinitionLevel, out bool[] hasValueFlags)
      {
         return UnpackDefinitions((TSystemType[])untypedSource, definitionLevels, maxDefinitionLevel, out hasValueFlags);
      }

      private TSystemType?[] UnpackDefinitions(TSystemType[] src, int[] definitionLevels, int maxDefinitionLevel, out bool[] hasValueFlags)
      {
         TSystemType?[] result = (TSystemType?[])GetArray(definitionLevels.Length, false, true);
         hasValueFlags = new bool[definitionLevels.Length];

         int isrc = 0;
         for (int i = 0; i < definitionLevels.Length; i++)
         {
            int level = definitionLevels[i];

            if (level == maxDefinitionLevel)
            {
               result[i] = src[isrc++];
               hasValueFlags[i] = true;
            }
            else if(level == 0)
            {
               hasValueFlags[i] = true;
            }

         }

         return result;

      }

      private ArrayView PackDefinitions(TSystemType?[] data, int offset, int count, int maxDefinitionLevel, out int[] definitionLevels, out int definitionsLength, out int nullCount)
      {
         definitionLevels = IntPool.Rent(count);
         definitionsLength = count;

         WritableArrayView<TSystemType> result = ArrayView.CreateWritable<TSystemType>(count);
         int ir = 0;
         nullCount = 0;

         int y = 0;
         for (int i = offset; i < (offset + count); i++, y++)
         {
            TSystemType? value = data[i];

            if(value == null)
            {
               definitionLevels[y] = 0;
               nullCount++;
            }
            else
            {
               definitionLevels[y] = maxDefinitionLevel;
               result[ir++] = value.Value;
            }
         }

         return result;
      }

      public override int Compare(TSystemType x, TSystemType y)
      {
         return Comparer.Compare(x, y);
      }

      public override bool Equals(TSystemType x, TSystemType y)
      {
         return EqualityComparer.Equals(x, y);
      }

      public override byte[] PlainEncode(Thrift.SchemaElement tse, TSystemType x)
      {
         using(var ms = new MemoryStream())
         {
            using(var bs = new BinaryWriter(ms))
            {
               WriteOne(bs, x);
            }

            return ms.ToArray();
         }
      }

      public override object PlainDecode(Thrift.SchemaElement tse, byte[] encoded)
      {
         if (encoded == null) return null;

         using(var ms = new MemoryStream(encoded))
         {
            using(var br = new BinaryReader(ms))
            {
               TSystemType element = ReadSingle(br, tse, -1);
               return element;
            }
         }

      }
   }
}
