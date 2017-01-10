namespace Cluj.Exif
{
    public enum ExifTagId : ushort
    {
        GPSLatRef = 0x0001,
        GPSLat = 0x0002,
        GPSLonRef = 0x0003,
        GPSLon = 0x0004,
        Make = 0x010F,
        Model = 0x0110,
        Date = 0x0132,
        GPSOffset = 0x8825
    }

    public enum ExifTagType : short
    {
        BYTE = 1,
        ASCII = 2,
        SHORT = 3,
        LONG = 4,
        RATIONAL = 5,
        UNDEFINED = 7,
        SLONG = 9,
        SRATIONAL = 10
    }

    public enum MetadataFormat
    {
        EXIF,
        JFIF
    }

    public enum Endian
    {
        Big,
        Little
    }
}