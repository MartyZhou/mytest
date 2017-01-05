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
        public void UseExifMetadataReader()
        {
            using (FileStream stream = new FileStream("./IO/Picture/p2.jpg", FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var meta = reader.ParseMetadata();
            }
        }

        [Fact]
        public void GetExifMetadata()
        {
            var gpsInfo = new GPSInfo();
            using (FileStream stream = new FileStream("./IO/Picture/p2.jpg", FileMode.Open))
            {
                Console.WriteLine(string.Format("{0:x}", stream.ReadByte())); // SOI FF D8
                Console.WriteLine(string.Format("{0:x}", stream.ReadByte()));
                Console.WriteLine(string.Format("{0:x}", stream.ReadByte())); // APP1 FF E1
                Console.WriteLine(string.Format("{0:x}", stream.ReadByte()));
                byte[] buffer = new byte[2]; // APP1 length
                stream.Read(buffer, 0, buffer.Length);

                byte[] exifName = new byte[4];
                stream.Read(exifName, 0, exifName.Length);
                Console.WriteLine(Encoding.UTF8.GetString(exifName));

                stream.ReadByte();
                stream.ReadByte(); // APP1 padding

                var app1start = stream.Position;
                byte[] app1Header = new byte[8];
                stream.Read(app1Header, 0, app1Header.Length);

                byte[] countBuffer = new byte[2]; // Number of Interoperability
                stream.Read(countBuffer, 0, countBuffer.Length);

                var count = Convert.ToInt16(countBuffer[0]) * 10 + Convert.ToInt16(countBuffer[1]);

                List<Tuple<long, int>> stringValues = new List<Tuple<long, int>>();

                long exifOffset;
                long gpsOffset = 0;

                for (int i = 0; i < count; i++)
                {
                    byte[] itemBuffer = new byte[12];
                    stream.Read(itemBuffer, 0, itemBuffer.Length);
                    Console.WriteLine(string.Join(" ", itemBuffer.Select(a => string.Format("{0:x}", a))));
                    var itemType = Convert.ToInt16(itemBuffer[3]);
                    if (itemType == 2 || itemType == 4)
                    {
                        var itemLength = Convert.ToInt32(itemBuffer[4]) * 1000 + Convert.ToInt32(itemBuffer[5]) * 100 + Convert.ToInt32(itemBuffer[6]) * 10 + Convert.ToInt32(itemBuffer[7]);
                        var offset = Convert.ToInt64(itemBuffer[8]) * 4096 + Convert.ToInt64(itemBuffer[9]) * 256 + Convert.ToInt64(itemBuffer[10] << 8) + Convert.ToInt64(itemBuffer[11]);
                        Console.WriteLine(string.Format("length: {0}, offset: {1}", itemLength, offset));

                        if (itemType == 2)
                        {
                            stringValues.Add(new Tuple<long, int>(offset, itemLength));
                        }
                        else if (itemType == 4)
                        {
                            var tagId = string.Format("{0:x}{1:x}", itemBuffer[0], itemBuffer[1]);
                            if (tagId == "8825") // GPS tag
                            {
                                gpsOffset = offset;
                            }
                            else if (tagId == "8769") // Exif tag
                            {
                                exifOffset = offset;
                            }
                        }

                    }
                }

                var nextBuffer = new byte[4];
                stream.Read(nextBuffer, 0, nextBuffer.Length);

                foreach (var item in stringValues)
                {
                    stream.Position = item.Item1 + app1start;
                    var stringBuffer = new byte[item.Item2];
                    stream.Read(stringBuffer, 0, stringBuffer.Length);
                    Console.WriteLine(Encoding.UTF8.GetString(stringBuffer));
                }

                List<Tuple<long, int>> gpsRefs = new List<Tuple<long, int>>();
                if (gpsOffset > 0)
                {
                    stream.Position = gpsOffset + app1start;

                    byte[] gpsCountBuffer = new byte[2]; // GPS IFD Number
                    stream.Read(gpsCountBuffer, 0, gpsCountBuffer.Length);
                    var gpsCount = Convert.ToInt16(gpsCountBuffer[0]) * 16 + Convert.ToInt16(gpsCountBuffer[1]);

                    for (int i = 0; i < gpsCount; i++)
                    {
                        byte[] itemBuffer = new byte[12];
                        stream.Read(itemBuffer, 0, itemBuffer.Length);
                        Console.WriteLine(string.Join(" ", itemBuffer.Select(a => string.Format("{0:x}", a))));
                        var tagId = ParseTagId(itemBuffer);
                        if (tagId == ExifTagId.GPSLatRef)
                        {
                            gpsInfo.LatRef = Convert.ToChar(itemBuffer[8]);
                        }
                        else if (tagId == ExifTagId.GPSLonRef)
                        {
                            gpsInfo.LonRef = Convert.ToChar(itemBuffer[8]);
                        }
                        else if (tagId == ExifTagId.GPSLat || tagId == ExifTagId.GPSLon)
                        {
                            var position = stream.Position;
                            byte[] offsetByte = { itemBuffer[8], itemBuffer[9], itemBuffer[10], itemBuffer[11] };
                            var latOffset = ParseOffset(offsetByte);
                            var latPosition = latOffset + app1start;
                            byte[] latBuffer = new byte[24];
                            stream.Position = latPosition;
                            stream.Read(latBuffer, 0, latBuffer.Length);
                            stream.Position = position;

                            if (tagId == ExifTagId.GPSLat)
                            {
                                gpsInfo.Lat = ParseGPSValue(latBuffer);
                            }
                            else if (tagId == ExifTagId.GPSLon)
                            {
                                gpsInfo.Lon = ParseGPSValue(latBuffer);
                            }
                            Console.WriteLine(string.Join(" ", latBuffer.Select(a => string.Format("{0:x}", a))));
                        }
                    }
                }

                byte[] testBuffer = new byte[1024];
                stream.Read(testBuffer, 0, testBuffer.Length);
            }
        }

        private ExifTagId ParseTagId(byte[] raw)
        {
            return (ExifTagId)(Convert.ToInt32(raw[0] << 8) + Convert.ToInt32(raw[1]));
        }

        private int ParseOffset(byte[] raw)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(raw);
            }
            return BitConverter.ToInt32(raw, 0);
        }

        private float ParseGPSValue(byte[] raw)
        {
            byte[] degreeNumeratorBuffer = { raw[0], raw[1], raw[2], raw[3] };
            var degreeNumerator = ParseOffset(degreeNumeratorBuffer);
            byte[] degreeDenumeratorBuffer = { raw[4], raw[5], raw[6], raw[7] };
            var degreeDenominator = ParseOffset(degreeDenumeratorBuffer);
            var degree = degreeNumerator / degreeDenominator;

            byte[] minuteNumeratorBuffer = { raw[8], raw[9], raw[10], raw[11] };
            var minuteNumerator = ParseOffset(minuteNumeratorBuffer);
            byte[] minuteDenumeratorBuffer = { raw[12], raw[13], raw[14], raw[15] };
            var minuteDenominator = ParseOffset(minuteDenumeratorBuffer);
            var minute = minuteNumerator / minuteDenominator;

            byte[] secondNumeratorBuffer = { raw[16], raw[17], raw[18], raw[19] };
            var secondNumerator = ParseOffset(secondNumeratorBuffer);
            byte[] secondDenumeratorBuffer = { raw[20], raw[21], raw[22], raw[23] };
            var secondDenominator = ParseOffset(secondDenumeratorBuffer);
            var second = secondNumerator / secondDenominator;

            return degree + (float)minute / 60 + (float)second / 3600;
        }

        [Fact]
        public void TestByte2Int()
        {
            int degree = 17;
            int minute = 23;
            int second = 22;

            float t = degree + (float)minute / 100 + (float)second / 10000;

            byte b1 = 0xff;
            byte b2 = 0xfe;
            byte[] bytes = { 0, 0, 0xff, 0xfe };
            var result = Convert.ToInt32(b1 << 8) + Convert.ToInt32(b2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            var result2 = BitConverter.ToInt32(bytes, 0);
        }
    }
}