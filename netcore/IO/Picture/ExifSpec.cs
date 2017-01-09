using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Cluj.Exif;
using Cluj.Photo;
using Newtonsoft.Json;
using System.Net.Http;

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
        public void UseExifMetadataReader3()
        {
            using (FileStream stream = new FileStream("./IO/Picture/p3.jpg", FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var meta = reader.ParseMetadata();

                Assert.Equal<string>("GoPro", meta.Make);
                Assert.Equal<string>("HERO5 Black", meta.Model);
                Assert.Equal<char>('N', meta.GPS.LatRef);
                Assert.Equal<char>('E', meta.GPS.LonRef);
            }
        }

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

        [Fact]
        public async void ReadAddressFromGoogleApi()
        {
            var lat = 39.950000;
            var lng = 116.350000;
            using (var httpClient = new HttpClient())
            {
                var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key=AIzaSyDQw1khA5tgbDFLv4pGRd1_yOp747LnXdE", lat, lng);
                var response = await httpClient.GetStreamAsync(url).ConfigureAwait(false);
                using (var reader = new StreamReader(response))
                {
                    var addressString = reader.ReadToEnd();
                    var address = JsonConvert.DeserializeObject<GoogleAddressResult>(addressString);
                }
            }
        }

        [Fact]
        public void ReadPhotosFromDirectory()
        {
            // Use the following api to get address namespace
            // https://maps.googleapis.com/maps/api/geocode/json?latlng=31.3105545,120.602219&key=AIzaSyDQw1khA5tgbDFLv4pGRd1_yOp747LnXdE
            // Consider the following in one area so that I don't call googleapis too often.
            // Northwest 39.950000, 116.350000, Northeast 39.950000, 116.44000
            // Southwest 39.900000, 116.350000, Southeast 39.900000, 116.44000
            // |Latitude| = 0.05, |Longtitude| = 0.08
            try
            {
                // In Windows, "*.jpg" returns files end with both .jpg and .JPG, but in Mac, it only returns .jpg.
                var jpgFiles = Directory.EnumerateFiles("./", "*.jpg", SearchOption.AllDirectories);

                foreach (var filePath in jpgFiles)
                {
                    var meta = ReadMeta(filePath);
                    Console.WriteLine(meta.Make);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Fact]
        public void ExtractAddress()
        {
            using (var stream = new FileStream("./IO/Picture/SampleAddress.json", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    var addressString = reader.ReadToEnd();
                    var address = JsonConvert.DeserializeObject<GoogleAddressResult>(addressString);
                }
            }
        }

        [Fact]
        public async void ProcessPhotoFiles()
        {
            await PhotoProcessor.Process().ConfigureAwait(false);
            PhotoProcessor.CopyPhotos();
        }

        [Fact]
        public void CheckDirectory()
        {
            var path = @"D:\photo\Photos\test3\sub1";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private PhotoMetadata ReadMeta(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                return reader.ParseMetadata();
            }
        }
    }
}