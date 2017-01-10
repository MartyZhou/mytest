using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Cluj.Exif;
using Newtonsoft.Json;

namespace Cluj.Photo
{
    public static class PhotoProcessor
    {
        private const string API_KEY = "AIzaSyDQw1khA5tgbDFLv4pGRd1_yOp747LnXdE";
        private static List<GoogleAddressResult> googleCache = new List<GoogleAddressResult>();
        private static List<GoogleAddressInfo> addressCache = new List<GoogleAddressInfo>();
        private static ConcurrentQueue<PhotoMetadata> photoQueue = new ConcurrentQueue<PhotoMetadata>();
        private static Config config;
        private static bool inProgress = true;

        public static async Task Process()
        {
            try
            {
                config = GetConfig();
                var filePaths = Directory.EnumerateFiles(config.path, "*.jpg", SearchOption.AllDirectories);
                var copyPhotosTask = Task.Run(() => CopyPhotosTask());

                foreach (var filePath in filePaths)
                {
                    var meta = ReadMeta(filePath);
                    if (meta != null)
                    {
                        await GenerateAddress(meta).ConfigureAwait(false);
                        GenerateNewFilePath(meta);
                        photoQueue.Enqueue(meta);
                    }
                }

                inProgress = false;
                Task.WaitAll(copyPhotosTask);

                // Save metadata as log
                SaveMetadata();
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error: {0}", e.Message));
            }
        }

        private static void CopyPhotosTask()
        {
            while (inProgress)
            {
                PhotoMetadata meta;
                if (photoQueue.TryDequeue(out meta))
                {
                    CopyPhoto(meta);
                }

                Task.Delay(500);
            }
        }

        private static void CopyPhoto(PhotoMetadata meta)
        {
            try
            {
                if (!Directory.Exists(meta.NewDirPath))
                {
                    Directory.CreateDirectory(meta.NewDirPath);
                }

                if (!File.Exists(meta.NewFilePath))
                {
                    Console.WriteLine(string.Format("Copying file: {0}", meta.NewFilePath));
                    File.Copy(meta.FilePath, meta.NewFilePath);
                }
                else
                {
                    Console.WriteLine(string.Format("File exists: {0}", meta.NewFilePath));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error copying photo: {0}", e.Message));
            }
        }

        private static void SaveMetadata()
        {
            File.WriteAllText("./IO/Picture/metadata_google.json", JsonConvert.SerializeObject(googleCache));
        }

        private static PhotoMetadata ReadMeta(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var result = reader.ParseMetadata();
                if (result != null)
                {
                    result.FilePath = stream.Name;
                }

                return result;
            }
        }

        private static async Task GenerateAddress(PhotoMetadata meta)
        {
            if (meta.HasLocation)
            {
                GoogleAddressInfo addressInfo;
                if (TryGetLocation(meta.GPS, out addressInfo))
                {
                    meta.Address = addressInfo;
                }
                else
                {
                    Console.WriteLine(string.Format("Loading address for {0}", meta.FilePath));
                    var foundAddress = false;
                    var address = await GetAddress(meta.GPS).ConfigureAwait(false);
                    googleCache.Add(address);
                    foreach (var info in address.results)
                    {
                        if (info.types.Length == 2 && info.types[0] == "locality" && info.types[1] == "political")
                        {
                            addressCache.Add(info);
                            meta.Address = info;
                            foundAddress = true;
                            break;
                        }
                    }

                    if (!foundAddress)
                    {
                        foreach (var info in address.results)
                        {
                            if (info.types.Length == 2 && info.types[0] == "country" && info.types[1] == "political")
                            {
                                addressCache.Add(info);
                                meta.Address = info;
                                foundAddress = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static void GenerateNewFilePath(PhotoMetadata meta)
        {
            var country = string.Empty;
            var level1 = string.Empty;
            var city = string.Empty;

            if (meta.Address.address_components != null)
            {
                foreach (var addr in meta.Address.address_components)
                {
                    if (addr.types[0] == "country" && addr.types[1] == "political")
                    {
                        country = addr.long_name;
                    }
                    else if (addr.types[0] == "administrative_area_level_1" && addr.types[1] == "political")
                    {
                        level1 = addr.long_name;
                    }
                    else if (addr.types[0] == "locality" && addr.types[1] == "political")
                    {
                        city = addr.long_name;
                    }
                }

                if (!string.IsNullOrWhiteSpace(country))
                {
                    if (string.IsNullOrWhiteSpace(city))
                    {
                        meta.NewDirPath = string.Format(@"{0}/{1}", config.new_path, country);
                        meta.NewFilePath = string.Format(@"{0}/{1}-{2}.jpg", meta.NewDirPath, meta.TakenDate.ToString("yyyy-MM-dd hh-mm-ss"), country);
                    }
                    else
                    {
                        meta.NewDirPath = string.Format(@"{0}/{1}/{2}", config.new_path, country, city);
                        meta.NewFilePath = string.Format(@"{0}/{1}-{2}-{3}.jpg", meta.NewDirPath, meta.TakenDate.ToString("yyyy-MM-dd hh-mm-ss"), country, city);
                    }
                }
            }
            else
            {
                meta.NewDirPath = string.Format(@"{0}/{1}", config.new_path, meta.TakenDate.ToString("yyyy-MM-dd"));
                meta.NewFilePath = string.Format(@"{0}/{1}.jpg", meta.NewDirPath, meta.TakenDate.ToString("yyyy-MM-dd hh-mm-ss"));
            }
        }

        private static async Task<GoogleAddressResult> GetAddress(GPSInfo gps)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var lat = gps.LatRef == 'N' ? gps.Lat : -gps.Lat;
                    var lng = gps.LonRef == 'E' ? gps.Lon : -gps.Lon;
                    var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}", lat, lng, API_KEY);

                    var response = await httpClient.GetStreamAsync(url).ConfigureAwait(false);
                    using (var reader = new StreamReader(response))
                    {
                        var addressString = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<GoogleAddressResult>(addressString);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Failed to call google map API: {0}", e.Message));
                return new GoogleAddressResult();
            }
        }

        private static bool TryGetLocation(GPSInfo gps, out GoogleAddressInfo address)
        {
            var result = false;
            address = new GoogleAddressInfo();

            foreach (var info in addressCache)
            {
                result = gps.Lat <= info.geometry.bounds.northeast.lat
                && gps.Lat >= info.geometry.bounds.southwest.lat
                && gps.Lon <= info.geometry.bounds.northeast.lng
                && gps.Lon >= info.geometry.bounds.southwest.lng;

                if (result)
                {
                    address = info;
                }
            }

            return result;
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