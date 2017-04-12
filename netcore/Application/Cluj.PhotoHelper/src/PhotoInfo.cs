using Cluj.Exif;

namespace Cluj.PhotoHelper
{
    internal class PhotoInfo
    {
        public PhotoMetadata PhotoMetadata { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string NewPath { get; set; }
    }
}