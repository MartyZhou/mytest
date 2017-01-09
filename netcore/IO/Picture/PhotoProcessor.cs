using System;
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
        private const string FolderPath = @"D:\photo\Photos\test";
        private const string NewFolderPath = @"D:\photo\Photos\test2";
        private static List<GoogleAddressInfo> addressCache = new List<GoogleAddressInfo>();
        private static List<PhotoMetadata> photoMetaCache = new List<PhotoMetadata>();
        private static int apiCount = 0;

        public static async Task Process()
        {
            try
            {
                var filePaths = Directory.EnumerateFiles(FolderPath, "*.jpg", SearchOption.AllDirectories);

                foreach (var filePath in filePaths)
                {
                    var meta = ReadMeta(filePath);
                    if (meta.HasLocation)
                    {
                        GoogleAddressInfo addressInfo;
                        if (TryGetLocation(meta.GPS, out addressInfo))
                        {
                            meta.Address = addressInfo;
                        }
                        else
                        {
                            var foundAddress = false;
                            var address = await GetAddress(meta.GPS).ConfigureAwait(false);
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

                    photoMetaCache.Add(meta);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void CopyPhotos()
        {
            foreach (var meta in photoMetaCache)
            {
                var country = string.Empty;
                var level1 = string.Empty;
                var city = string.Empty;

                try
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
                            meta.NewDirPath = string.Format(@"{0}\{1}", NewFolderPath, country);
                            meta.NewFilePath = string.Format(@"{0}\{1}-{2}.jpg", meta.NewDirPath, meta.TakenDate.ToString("yyyy-MM-dd hh-mm-ss"), country);
                        }
                        else
                        {
                            meta.NewDirPath = string.Format(@"{0}\{1}\{2}", NewFolderPath, country, city);
                            meta.NewFilePath = string.Format(@"{0}\{1}-{2}-{3}.jpg", meta.NewDirPath, meta.TakenDate.ToString("yyyy-MM-dd hh-mm-ss"), country, city);
                        }

                        if (!Directory.Exists(meta.NewDirPath))
                        {
                            Directory.CreateDirectory(meta.NewDirPath);
                            File.Copy(meta.FilePath, meta.NewFilePath);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static PhotoMetadata ReadMeta(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                var reader = new PhotoMetadataReader(stream);
                var result = reader.ParseMetadata();
                result.FilePath = stream.Name;
                return result;
            }
        }

        private static async Task<GoogleAddressResult> GetAddress(GPSInfo gps)
        {
            using (var httpClient = new HttpClient())
            {
                var lat = gps.LatRef == 'N' ? gps.Lat : -gps.Lat;
                var lng = gps.LonRef == 'E' ? gps.Lon : -gps.Lon;
                var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}", lat, lng, API_KEY);
                apiCount++;
                var response = await httpClient.GetStreamAsync(url).ConfigureAwait(false);
                using (var reader = new StreamReader(response))
                {
                    var addressString = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<GoogleAddressResult>(addressString);
                }
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
    }
}