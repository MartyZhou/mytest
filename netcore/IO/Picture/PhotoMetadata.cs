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
}