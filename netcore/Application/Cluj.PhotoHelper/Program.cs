using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cluj.Exif;
using Cluj.PhotoLocation;
using Newtonsoft.Json;

namespace Cluj.PhotoHelper
{
    class Program
    {
        private static readonly Config config = GetConfig();

        static void Main(string[] args)
        {
            Console.WriteLine("*********** PhotoHelper Started ***********");

            Console.WriteLine(JsonConvert.SerializeObject(config));

            var photoPaths = Directory.EnumerateFiles(config.PhotoSourceFolder, "*.jpg", SearchOption.AllDirectories);

            Task.WaitAll(ProcessPhotos(photoPaths));

            Console.WriteLine("*********** PhotoHelper Finished ***********");
        }

        private static async Task ProcessPhotos(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                var photoInfo = await ReadMeta(path).ConfigureAwait(false);
                CreateNewPhotoPath(photoInfo);
                CopyPhoto(photoInfo);
            }
        }

        private static void CopyPhoto(PhotoInfo info)
        {
            try
            {
                var directory = Path.GetDirectoryName(info.NewPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(info.NewPath))
                {
                    Console.WriteLine(string.Format("Copying file: {0}", info.NewPath));
                    File.Copy(info.PhotoMetadata.FilePath, info.NewPath);
                }
                else
                {
                    info.NewPath = string.Format("{0}_{1}.jpg", info.NewPath, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffffff"));
                    File.Copy(info.PhotoMetadata.FilePath, info.NewPath);
                    Console.WriteLine(string.Format("File exists: {0}", info.NewPath));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error copying photo: {0}", e.Message));
            }
        }

        private static async Task<PhotoInfo> ReadMeta(string path)
        {
            var result = new PhotoInfo();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var meta = reader.ParseMetadata();
                if (meta != null)
                {
                    meta.FilePath = stream.Name;
                }

                result.PhotoMetadata = meta;

                if (meta.HasLocation)
                {
                    result.City = await Cache.GetCityName(meta.GPS.LatRef, meta.GPS.Lat, meta.GPS.LonRef, meta.GPS.Lon).ConfigureAwait(false);
                }
            }

            return result;
        }

        private static void CreateNewPhotoPath(PhotoInfo info)
        {
            if (info.PhotoMetadata.HasLocation)
            {
                info.NewPath = string.Format(@"{0}/{1}/{2}/{1}_{2}_{3}.jpg", config.PhotoDestinationFolder, info.City, info.PhotoMetadata.Model, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"));
            }
            else
            {
                info.NewPath = string.Format(@"{0}/!NoGPS/{1}/{2}/{2}_{3}.jpg", config.PhotoDestinationFolder, info.PhotoMetadata.TakenDate.ToString("yyyy-MM"), info.PhotoMetadata.Model, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"));
            }
        }

        private static Config GetConfig()
        {
            using (var stream = new FileStream("./config.json", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    return JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
                }
            }
        }
    }
}
