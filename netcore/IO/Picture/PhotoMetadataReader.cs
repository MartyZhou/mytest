using System;
using System.Globalization;
using System.IO;
using System.Text;
using Cluj.Exif;

namespace Cluj.Photo
{
    public class PhotoMetadataReader
    {
        private readonly Stream stream;
        private short APP1_START_POSITION = 12;
        private const short APP1_HEADER_LENGTH = 8;

        public PhotoMetadataReader(Stream stream)
        {
            this.stream = stream;
        }

        public PhotoMetadata ParseMetadata()
        {
            var meta = new PhotoMetadata();
            var format = ParseFormat();
            if (format == MetadataFormat.EXIF)
            {
                APP1_START_POSITION = 12;
            }
            else if (format == MetadataFormat.JFIF)
            {
                APP1_START_POSITION = 30;
            }

            return ParseExif();
        }

        private PhotoMetadata ParseExif()
        {
            try
            {
                var meta = new PhotoMetadata();

                stream.Position = APP1_START_POSITION + APP1_HEADER_LENGTH;

                var count = Convert2BytesToShort(ReadBytes(2));
                for (ushort i = 0; i < count; i++)
                {
                    var tagId = Convert2BytesToTag(ReadBytes(2));
                    var tagType = ParseTagType(ReadBytes(2));
                    var length = Convert4BytesToInt(ReadBytes(4));
                    var valueBuffer = ReadBytes(4);

                    if (tagId == ExifTagId.Make)
                    {
                        meta.Make = GetStringRefValue(valueBuffer, length);
                    }
                    else if (tagId == ExifTagId.Model)
                    {
                        meta.Model = GetStringRefValue(valueBuffer, length);
                    }
                    else if (tagId == ExifTagId.Date)
                    {
                        meta.TakenDate = GetDateRefValue(valueBuffer, length);
                    }
                    else if (tagId == ExifTagId.GPSOffset)
                    {
                        meta.HasLocation = true;
                        meta.GPS = ReadGPSInfo(Convert4BytesToInt(valueBuffer));
                    }
                }

                return meta;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error parsing EXIF: {0}", e.Message));
                return null;
            }
        }

        private MetadataFormat ParseFormat()
        {
            var format = MetadataFormat.EXIF;
            stream.Position = 2;
            var app1Maker = Convert2BytesToUShort(ReadBytes(2));
            if (app1Maker == 0xffe0)
            {
                format = MetadataFormat.JFIF;
            }

            return format;
        }

        private byte[] ReadBytes(int length)
        {
            var buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }

        private string ReadString(long offset, int length)
        {
            var position = stream.Position;
            stream.Position = offset;
            var buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Position = position;
            return ConvertToString(buffer);
        }

        private DateTime ReadDate(long offset, int length)
        {
            var position = stream.Position;
            stream.Position = offset;
            var buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Position = position;
            return ConvertToDate(buffer);
        }

        private string GetStringRefValue(byte[] raw, int length)
        {
            var offset = Convert4BytesToInt(raw) + APP1_START_POSITION;
            return ReadString(offset, length);
        }

        private DateTime GetDateRefValue(byte[] raw, int length)
        {
            var offset = Convert4BytesToInt(raw) + APP1_START_POSITION;
            return ReadDate(offset, length);
        }

        private int Convert4BytesToInt(byte[] raw)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(raw);
            }
            return BitConverter.ToInt32(raw, 0);
        }

        private short Convert2BytesToShort(byte[] raw)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(raw);
            }
            return BitConverter.ToInt16(raw, 0);
        }

        private ushort Convert2BytesToUShort(byte[] raw)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(raw);
            }
            return BitConverter.ToUInt16(raw, 0);
        }

        private ExifTagId Convert2BytesToTag(byte[] raw)
        {
            return (ExifTagId)Convert2BytesToUShort(raw);
        }

        private string ConvertToString(byte[] raw)
        {
            var copy = new byte[raw.Length - 1];
            Array.Copy(raw, copy, raw.Length - 1);
            return Encoding.UTF8.GetString(copy);
        }

        private DateTime ConvertToDate(byte[] raw)
        {
            var dateString = ConvertToString(raw);
            DateTime result;

            if (!DateTime.TryParseExact(dateString, "yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
            {
                result = DateTime.MinValue;
            }

            return result;
        }

        private ExifTagType ParseTagType(byte[] raw)
        {
            return (ExifTagType)Convert.ToInt16(raw[1]);
        }

        private GPSInfo ReadGPSInfo(long offset)
        {
            try
            {
                var gps = new GPSInfo();
                var position = stream.Position;
                stream.Position = offset + APP1_START_POSITION;
                var count = Convert2BytesToUShort(ReadBytes(2));

                for (ushort i = 0; i < count; i++)
                {
                    var tagId = Convert2BytesToTag(ReadBytes(2));
                    var tagType = ParseTagType(ReadBytes(2));
                    var length = Convert4BytesToInt(ReadBytes(4));
                    var valueBuffer = ReadBytes(4);

                    if (tagId == ExifTagId.GPSLatRef)
                    {
                        gps.LatRef = Convert.ToChar(valueBuffer[0]);
                    }
                    else if (tagId == ExifTagId.GPSLonRef)
                    {
                        gps.LonRef = Convert.ToChar(valueBuffer[0]);
                    }
                    else if (tagId == ExifTagId.GPSLat)
                    {
                        gps.Lat = ReadGPSValue(Convert4BytesToInt(valueBuffer));
                    }
                    else if (tagId == ExifTagId.GPSLon)
                    {
                        gps.Lon = ReadGPSValue(Convert4BytesToInt(valueBuffer));
                    }
                }

                stream.Position = position;
                return gps;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error reading GPS: {0}", e.Message));
                return new GPSInfo();
            }
        }

        private float ReadGPSValue(long offset)
        {
            var position = stream.Position;
            stream.Position = offset + APP1_START_POSITION;
            var buffer = ReadBytes(24);
            stream.Position = position;
            return ParseGPSValue(buffer);
        }

        private float ParseGPSValue(byte[] raw)
        {
            byte[] degreeNumeratorBuffer = { raw[0], raw[1], raw[2], raw[3] };
            var degreeNumerator = Convert4BytesToInt(degreeNumeratorBuffer);
            byte[] degreeDenumeratorBuffer = { raw[4], raw[5], raw[6], raw[7] };
            var degreeDenominator = Convert4BytesToInt(degreeDenumeratorBuffer);
            var degree = degreeNumerator / degreeDenominator;

            byte[] minuteNumeratorBuffer = { raw[8], raw[9], raw[10], raw[11] };
            var minuteNumerator = Convert4BytesToInt(minuteNumeratorBuffer);
            byte[] minuteDenumeratorBuffer = { raw[12], raw[13], raw[14], raw[15] };
            var minuteDenominator = Convert4BytesToInt(minuteDenumeratorBuffer);
            var minute = minuteNumerator / minuteDenominator;

            byte[] secondNumeratorBuffer = { raw[16], raw[17], raw[18], raw[19] };
            var secondNumerator = Convert4BytesToInt(secondNumeratorBuffer);
            byte[] secondDenumeratorBuffer = { raw[20], raw[21], raw[22], raw[23] };
            var secondDenominator = Convert4BytesToInt(secondDenumeratorBuffer);
            var second = secondNumerator / secondDenominator;

            return degree + (float)minute / 60 + (float)second / 3600;
        }
    }
}