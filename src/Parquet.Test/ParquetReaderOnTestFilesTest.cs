using Parquet.Data;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace Parquet.Test
{
   using F = System.IO.File;

   /// <summary>
   /// Tests a set of predefined test files that they read back correct
   /// </summary>
   public class ParquetReaderOnTestFilesTest
   {
      private byte[] vals = new byte[18]
      {
         0x00,
         0x00,
         0x27,
         0x79,
         0x7f,
         0x26,
         0xd6,
         0x71,
         0xc8,
         0x00,
         0x00,
         0x4e,
         0xf2,
         0xfe,
         0x4d,
         0xac,
         0xe3,
         0x8f
      };

      [Fact]
      public void FixedLenByteArray_dictionary()
      {
         using (Stream s = F.OpenRead(GetDataFilePath("fixedlenbytearray.parquet")))
         {
            using (var r = new ParquetReader(s))
            {
               DataSet ds = r.Read();
            }
         }
      }

      [Fact]
      public void Datetypes_all()
      {
         DateTimeOffset offset, offset2;
         using (Stream s = F.OpenRead(GetDataFilePath("dates.parquet")))
         {
            using (var r = new ParquetReader(s))
            {
               DataSet ds = r.Read();
               offset = (DateTimeOffset)ds[0][1];
               offset2 = (DateTimeOffset)ds[1][1];
            }
         }
         Assert.Equal(new DateTime(2017, 1, 1), offset.Date);
         Assert.Equal(new DateTime(2017, 2, 1), offset2.Date);
      }

      [Fact]
      public void NestedColumns_Exception()
      {
         using (Stream s = F.OpenRead(GetDataFilePath("nested.parquet")))
         {
            using (var r = new ParquetReader(s))
            {
               Assert.Throws<NotImplementedException>(() => r.Read());
            }
         }
      }

      //[Fact]
      public void Delete_me_manual_test()
      {
         var ds = ParquetReader.ReadFile("C:\\tmp\\postcodes.plain.parquet");
      }

      private string GetDataFilePath(string name)
      {
         string thisPath = Assembly.Load(new AssemblyName("Parquet.Test")).Location;
         return Path.Combine(Path.GetDirectoryName(thisPath), "data", name);
      }
   }
}