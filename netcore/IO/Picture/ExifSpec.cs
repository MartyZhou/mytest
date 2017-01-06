using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Cluj.Exif;
using Cluj.Photo;

namespace IO.Picture
{
    public class ExifSpec
    {
        /*
1 = BYTE An 8-bit unsigned integer.
2 = ASCII An 8-bit byte containing one 7-bit ASCII code. The final byte is terminated with NULL.
3 = SHORT A 16-bit (2-byte) unsigned integer,
4 = LONG A 32-bit (4-byte) unsigned integer, 
5 = RATIONAL Two LONGs. The first LONG is the numerator and the second LONG expresses the denominator.
7 = UNDEFINED An 8-bit byte that may take any value depending on the field definition.
9 = SLONG A 32-bit (4-byte) signed integer (2's complement notation).
10 = SRATIONAL Two SLONGs. The first SLONG is the numerator and the second SLONG is the denominator.
        */

        [Fact]
        public void UseExifMetadataReader2()
        {
            using (FileStream stream = new FileStream("./IO/Picture/p2.jpg", FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var meta = reader.ParseMetadata();
                
                Assert.Equal<string>("Apple", meta.Make);
                Assert.Equal<string>("iPhone SE", meta.Model);
                Assert.Equal<char>('N', meta.GPS.LatRef);
                Assert.Equal<char>('E', meta.GPS.LonRef);
            }
        }

        [Fact]
        public void UseExifMetadataReader1()
        {
            using (FileStream stream = new FileStream("./IO/Picture/p1.jpg", FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var meta = reader.ParseMetadata();
                
                Assert.Equal<string>("NIKON CORPORATION", meta.Make);
                Assert.Equal<string>("NIKON D40", meta.Model);
                Assert.Equal<char>('N', meta.GPS.LatRef);
                Assert.Equal<char>('E', meta.GPS.LonRef);
            }
        }
    }
}