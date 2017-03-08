using System.IO;
using Xunit;

namespace Cluj.Photo2.UnitTest
{
    public class PhotoSpec
    {
        [Fact]
        public void ReadExif()
        {
            using (FileStream stream = new FileStream("/p1_exif_header.jpg", FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var meta = reader.ParseMetadata();

                Assert.Equal<string>("NIKON CORPORATION", meta.Make);
                Assert.Equal<string>("NIKON D3300", meta.Model);
            }
        }
    }
}