using System;
using Cluj.Exif;

namespace Cluj.Photo
{
    public class PhotoMetadata
    {
        public Endian Endian { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime TakenDate { get; set; }
        public bool HasLocation { get; set; }
        public GPSInfo GPS { get; set; }
        public GoogleAddressInfo Address { get; set; }
        public string FilePath { get; set; }
        public string NewDirPath { get; set; }
        public string NewFilePath { get; set; }
    }

    public struct GoogleAddressType
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }

    public struct GoogleAddressInfo
    {
        public GoogleAddressType[] address_components { get; set; }
        public string formatted_address { get; set; }
        public string place_id { get; set; }
        public GoogleGeometry geometry { get; set; }
        public string[] types { get; set; }
    }

    public struct GoogleLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public struct GoogleBounds
    {
        public GoogleLocation northeast { get; set; }
        public GoogleLocation southwest { get; set; }
    }

    public struct GoogleGeometry
    {
        public GoogleBounds bounds { get; set; }
        public string[] types { get; set; }
    }

    public struct GoogleAddressResult
    {
        public GoogleAddressInfo[] results { get; set; }
    }

    public struct TripSpan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public struct TripDuration
    {
        public GoogleAddressInfo Location { get; set; }
        public TripSpan[] Spans { get; set; }
    }

    public struct Config
    {
        public string path { get; set; }
        public string new_path { get; set; }
    }
}