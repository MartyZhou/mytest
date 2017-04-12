﻿using System;
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
        private static List<string> unprocessedPhotos = new List<string>();
        private static List<string> notCopiedPhotos = new List<string>();
        private static List<string> exceptionPhotos = new List<string>();
        private static List<PhotoInfo> noGPSPhotos = new List<PhotoInfo>();
        private static Dictionary<string, Tuple<DateTime, DateTime, string>> locationTimeSpan = new Dictionary<string, Tuple<DateTime, DateTime, string>>();

        static void Main(string[] args)
        {
            Console.WriteLine("*********** PhotoHelper Started ***********");

            Console.WriteLine(JsonConvert.SerializeObject(config));

            var photoPaths = Directory.EnumerateFiles(config.PhotoSourceFolder, "*.jpg", SearchOption.AllDirectories);

            Task.WaitAll(ProcessPhotos(photoPaths));

            ProcessNoGPSPhotos();

            LogUnprocessedPhotos();

            LogNotCopiedPhotos();

            LogExceptionPhotos();

            Console.WriteLine("*********** PhotoHelper Finished ***********");
        }

        private static async Task ProcessPhotos(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                try
                {
                    var photoInfo = await ReadMeta(path).ConfigureAwait(false);
                    if (photoInfo.PhotoMetadata != null && photoInfo.PhotoMetadata.HasLocation)
                    {
                        CreateNewPhotoPathWithLocation(photoInfo);
                        CopyPhoto(photoInfo);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("[PhotoHelper] Error proccessing photo: {0}", e.Message));
                    exceptionPhotos.Add(path);
                }
            }
        }

        private static void CopyPhoto(PhotoInfo info)
        {
            try
            {
                // Console.WriteLine(string.Format("************************** Trying to copy file: {0}", info.NewPath));
                var directory = Path.GetDirectoryName(info.NewPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(info.NewPath))
                {
                    Console.WriteLine(string.Format("[PhotoHelper] Copying file: {0}", info.NewPath));
                    File.Copy(info.PhotoMetadata.FilePath, info.NewPath);
                }
                else
                {
                    info.NewPath = string.Format("{0}_{1}.jpg", info.NewPath, Guid.NewGuid());
                    File.Copy(info.PhotoMetadata.FilePath, info.NewPath);
                    Console.WriteLine(string.Format("[PhotoHelper] Copying file with Guid path: {0}", info.NewPath));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("[PhotoHelper] Error copying photo: {0}", e.Message));
                notCopiedPhotos.Add(info.PhotoMetadata.FilePath);
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
                    meta.Model = meta.Model.Trim('\x00');

                    //Console.WriteLine(string.Format("[PhotoHelper]###################### ReadMeta for file {0}. meta: {1}", meta.FilePath, JsonConvert.SerializeObject(meta)));

                    result.PhotoMetadata = meta;

                    if (meta.HasLocation)
                    {
                        var cityLocation = await Cache.GetCityName(meta.GPS.LatRef, meta.GPS.Lat, meta.GPS.LonRef, meta.GPS.Lon).ConfigureAwait(false);
                        var cityAndCountryName = GetCityAndCountryName(cityLocation);
                        result.City = cityAndCountryName.Item1;
                        result.Country = cityAndCountryName.Item2;

                        if (string.IsNullOrWhiteSpace(result.City))
                        {
                            noGPSPhotos.Add(result);
                            meta.HasLocation = false;
                        }
                    }
                    else
                    {
                        noGPSPhotos.Add(result);
                    }
                }
                else
                {
                    unprocessedPhotos.Add(path);
                }
            }

            return result;
        }

        private static void CreateNewPhotoPathWithLocation(PhotoInfo info)
        {
            if (info.PhotoMetadata.HasLocation && !string.IsNullOrWhiteSpace(info.City))
            {
                Console.WriteLine(string.Format("[PhotoHelper]###################### CreateNewPhotoPathWithLocation for city {0}. Info: {1}", info.City, JsonConvert.SerializeObject(info)));
                info.NewPath = CreateGPSNewPath(info);

                if (!IsCityExcluded(info.City) && !IsLocationExcluded(info.PhotoMetadata.GPS))
                {
                    CacheLocationTimeSpan(info);
                }
            }
        }

        private static void CacheLocationTimeSpan(PhotoInfo info)
        {
            if (!locationTimeSpan.ContainsKey(info.City))
            {
                locationTimeSpan.Add(info.City, new Tuple<DateTime, DateTime, string>(info.PhotoMetadata.TakenDate, info.PhotoMetadata.TakenDate, info.Country));
            }
            else
            {
                var dateSpan = locationTimeSpan[info.City];
                //Console.WriteLine(string.Format("[PhotoHelper]###################### before city {0} adjusted time span {1}", info.City, JsonConvert.SerializeObject(dateSpan)));

                if (dateSpan.Item1 > info.PhotoMetadata.TakenDate && dateSpan.Item1.Subtract(info.PhotoMetadata.TakenDate).TotalDays < config.SpanLimitDays)
                {
                    locationTimeSpan[info.City] = new Tuple<DateTime, DateTime, string>(info.PhotoMetadata.TakenDate, dateSpan.Item2, info.Country);
                }

                if (dateSpan.Item2 < info.PhotoMetadata.TakenDate && info.PhotoMetadata.TakenDate.Subtract(dateSpan.Item2).TotalDays < config.SpanLimitDays)
                {
                    locationTimeSpan[info.City] = new Tuple<DateTime, DateTime, string>(dateSpan.Item1, info.PhotoMetadata.TakenDate, info.Country);
                }

                //Console.WriteLine(string.Format("[PhotoHelper]###################### after city {0} adjusted time span {1}", info.City, JsonConvert.SerializeObject(dateSpan)));
            }
        }

        private static string CreateNoGPSNewPath(PhotoInfo info)
        {
            return string.Format(@"{0}/!NoGPS/{1}/{2}/{2}_{3}.jpg", config.PhotoDestinationFolder, info.PhotoMetadata.TakenDate.ToString("yyyy-MM"), info.PhotoMetadata.Model, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"));
        }

        private static string CreateGPSNewPath(PhotoInfo info)
        {
            return string.Format(@"{0}/{1}/{2}/{3}/{1}_{2}_{4}.jpg", config.PhotoDestinationFolder, info.Country, info.City, info.PhotoMetadata.Model, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"));
        }

        private static void ProcessNoGPSPhotos()
        {
            var tolerance = new TimeSpan(0, config.TimeTolerance, 0);

            foreach (var photo in noGPSPhotos)
            {
                AppendLocationForNoneGPSPhotos(photo, tolerance);

                if (string.IsNullOrWhiteSpace(photo.City))
                {
                    photo.NewPath = CreateNoGPSNewPath(photo);
                }
                else
                {
                    photo.NewPath = CreateGPSNewPath(photo);
                }

                CopyPhoto(photo);
            }
        }

        private static void LogUnprocessedPhotos()
        {
            File.WriteAllText(string.Format(@"{0}/unprocessed_photos.txt", config.PhotoDestinationFolder), JsonConvert.SerializeObject(unprocessedPhotos));
        }

        private static void LogNotCopiedPhotos()
        {
            File.WriteAllText(string.Format(@"{0}/not_copyied_photos.txt", config.PhotoDestinationFolder), JsonConvert.SerializeObject(notCopiedPhotos));
        }

        private static void LogExceptionPhotos()
        {
            File.WriteAllText(string.Format(@"{0}/exception_photos.txt", config.PhotoDestinationFolder), JsonConvert.SerializeObject(exceptionPhotos));
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

        private static Tuple<string, string> GetCityAndCountryName(CityLocation cityLocation)
        {
            var city = string.Empty;
            var country = cityLocation.Country;

            if (!string.IsNullOrWhiteSpace(cityLocation.City))
            {
                city = cityLocation.City;
            }
            else if (!string.IsNullOrWhiteSpace(cityLocation.AreaLevel2))
            {
                city = cityLocation.AreaLevel2;
            }
            else if (!string.IsNullOrWhiteSpace(cityLocation.AreaLevel1))
            {
                city = cityLocation.AreaLevel1;
            }
            else
            {
                city = country;
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                country = city;
            }

            return new Tuple<string, string>(city, country);
        }

        private static bool IsCityExcluded(string cityName)
        {
            var result = config.ExcludedCities.Contains(cityName.ToLower());

            if (result)
            {
                Console.WriteLine(string.Format("[PhotoHelper]###################### city {0} is excluded for location appending", cityName));
            }

            return result;
        }

        private static bool IsLocationExcluded(GPSInfo gps)
        {
            var result = false;

            var lat = gps.LatRef == 'N' ? gps.Lat : -gps.Lat;
            var lon = gps.LonRef == 'E' ? gps.Lon : -gps.Lon;

            foreach (var area in config.ExcludedArea)
            {
                var latTest = lat <= area.Norteast.Lat && lat >= area.Southwest.Lat;
                var lonTest = lon <= area.Norteast.Lng && lon >= area.Southwest.Lng;

                if (latTest && lonTest)
                {
                    result = true;

                    Console.WriteLine(string.Format("[PhotoHelper]###################### exclude location: {0}", JsonConvert.SerializeObject(gps)));
                    break;
                }
            }

            return result;
        }

        private static void AppendLocationForNoneGPSPhotos(PhotoInfo info, TimeSpan tolerance)
        {
            foreach (var item in locationTimeSpan)
            {
                //var leftTime = item.Value.Item1.Subtract(tolerance);
                //var rightTime = item.Value.Item2.Add(tolerance);

                var leftTime = item.Value.Item1;
                var rightTime = item.Value.Item2;

                //Console.WriteLine(string.Format("[PhotoHelper]###################### try to append location {0} to {1}. Photo Date {2}, left time: {3}, right time: {4}", info.City, info.PhotoMetadata.FilePath, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"), leftTime.ToString("yyyy-MM-dd HH-mm-ss"), rightTime.ToString("yyyy-MM-dd HH-mm-ss")));

                if (info.PhotoMetadata.TakenDate > leftTime && info.PhotoMetadata.TakenDate < rightTime)
                {
                    info.City = item.Key;
                    info.Country = item.Value.Item3;

                    Console.WriteLine(string.Format("[PhotoHelper]###################### append location {0} to {1}. Photo Date {2}, left time: {3}, right time: {4}", info.City, info.PhotoMetadata.FilePath, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"), leftTime.ToString("yyyy-MM-dd HH-mm-ss"), rightTime.ToString("yyyy-MM-dd HH-mm-ss")));

                    break;
                }
            }
        }
    }
}