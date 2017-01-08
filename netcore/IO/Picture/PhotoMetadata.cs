using System;
using Cluj.Exif;

namespace Cluj.Photo
{
    public class PhotoMetadata
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime TakenDate { get; set; }
        public GPSInfo GPS { get; set; }
    }

    public class PhotoFileMetadata : PhotoMetadata
    {
        public string Path { get; set; }
        public string NewPath { get; set; }
    }

    public class GoogleAddressType
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }

    public class GoogleAddressInfo
    {
        public GoogleAddressType[] address_components { get; set; }
        public string formatted_address { get; set; }
        public string place_id { get; set; }
        public string[] types { get; set; }
    }

    public class GoogleAddressResult
    {
        public GoogleAddressInfo[] results { get; set; }
    }
}