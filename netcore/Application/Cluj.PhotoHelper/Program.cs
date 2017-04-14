using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cluj.Exif;
using Cluj.PhotoLocation;
using Newtonsoft.Json;

namespace Cluj.PhotoHelper
{
    class Program
    {
        private const string PATH_COUNTRY = "country";
        private const string PATH_CITY = "city";
        private const string PATH_DEVICE = "device";
        private const string PATH_DATE = "date";
        private static readonly Config config = GetConfig();
        private static List<string> unprocessedPhotos = new List<string>();
        private static List<string> notCopiedPhotos = new List<string>();
        private static List<string> exceptionPhotos = new List<string>();
        private static List<PhotoInfo> noGPSPhotos = new List<PhotoInfo>();
        private static Dictionary<string, Tuple<DateTime, DateTime, string>> locationTimeSpan = new Dictionary<string, Tuple<DateTime, DateTime, string>>();
        private static Dictionary<string, NodeTimeSpan> nodeTimeSpanCache = new Dictionary<string, NodeTimeSpan>();
        private static Dictionary<string, NodeTimeSpan> level2TimeSpanCache = new Dictionary<string, NodeTimeSpan>();
        private static Dictionary<string, NodeTimeSpan> level1TimeSpanCache = new Dictionary<string, NodeTimeSpan>();
        private static Dictionary<string, NodeTimeSpan> countryTimeSpanCache = new Dictionary<string, NodeTimeSpan>();
        private static Dictionary<int, string> gpsFilePathParts;
        private static Dictionary<int, string> noneGPSFilePathParts;


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
                    //Console.WriteLine(string.Format("[PhotoHelper] Copying file: {0}", info.NewPath));
                    File.Copy(info.PhotoMetadata.FilePath, info.NewPath);
                }
                else
                {
                    info.NewPath = string.Format("{0}_{1}.jpg", info.NewPath, Guid.NewGuid());
                    File.Copy(info.PhotoMetadata.FilePath, info.NewPath);
                    //Console.WriteLine(string.Format("[PhotoHelper] Copying file with Guid path: {0}", info.NewPath));
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
                        var leafNode = await Cache.GetLeafNode(meta.GPS.LatRef, meta.GPS.Lat, meta.GPS.LonRef, meta.GPS.Lon).ConfigureAwait(false);
                        if (leafNode != null)
                        {
                            var cityAndCountryName = GetCityAndCountryName(leafNode);
                            result.City = cityAndCountryName.Item1;
                            result.Country = cityAndCountryName.Item2;
                            result.Node = leafNode;
                        }

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
                // Console.WriteLine(string.Format("[PhotoHelper]###################### CreateNewPhotoPathWithLocation for city {0}. Info: {1}", info.City, JsonConvert.SerializeObject(info)));
                info.NewPath = CreateGPSNewPath(info);

                if (!IsCityExcluded(info.City) && !IsLocationExcluded(info.PhotoMetadata.GPS))
                {
                    CacheLocationTimeSpan(info);
                }
            }
        }

        private static void CacheLocationTimeSpan(PhotoInfo info)
        {
            /*if (!locationTimeSpan.ContainsKey(info.City))
            {
                locationTimeSpan.Add(info.City, new Tuple<DateTime, DateTime, string>(info.PhotoMetadata.TakenDate, info.PhotoMetadata.TakenDate, info.Country));
            }

            var dateSpan = locationTimeSpan[info.City];
            //Console.WriteLine(string.Format("[PhotoHelper]###################### before city {0} adjusted time span {1}", info.City, JsonConvert.SerializeObject(dateSpan)));

            if (dateSpan.Item1 > info.PhotoMetadata.TakenDate && dateSpan.Item1.Subtract(info.PhotoMetadata.TakenDate).TotalDays < config.SpanLimitDays)
            {
                locationTimeSpan[info.City] = new Tuple<DateTime, DateTime, string>(info.PhotoMetadata.TakenDate, dateSpan.Item2, info.Country);
            }

            if (dateSpan.Item2 < info.PhotoMetadata.TakenDate && info.PhotoMetadata.TakenDate.Subtract(dateSpan.Item2).TotalDays < config.SpanLimitDays)
            {
                locationTimeSpan[info.City] = new Tuple<DateTime, DateTime, string>(dateSpan.Item1, info.PhotoMetadata.TakenDate, info.Country);
            }*/

            //Console.WriteLine(string.Format("[PhotoHelper]###################### after city {0} adjusted time span {1}", info.City, JsonConvert.SerializeObject(dateSpan)));

            if (info.Node != null)
            {
                CacheNodeTimeSpan(info.Node, info);
            }
        }

        private static NodeTimeSpan CacheNodeTimeSpan(Node node, PhotoInfo info)
        {

            if (node.Parent == null)
            {
                var topNode = node;
                //Console.WriteLine(string.Format("########## parent is null ############### try to cache time span {0} for {1}", node.FormattedAddress, info.PhotoMetadata.FilePath));
                if (!nodeTimeSpanCache.ContainsKey(topNode.PlaceId))
                {
                    NodeTimeSpan span = new NodeTimeSpan();
                    span.Node = topNode;
                    span.LeftDate = info.PhotoMetadata.TakenDate;
                    span.RightDate = info.PhotoMetadata.TakenDate;

                    nodeTimeSpanCache.Add(topNode.PlaceId, span);
                }

                var topNodeSpan = nodeTimeSpanCache[topNode.PlaceId];
                UpdateSpanTime(topNodeSpan, info);

                return topNodeSpan;
            }
            else
            {
                var parentNode = node.Parent;
                //Console.WriteLine(string.Format("########## parent is not null ############### try to cache time span {0} for {1}", node.FormattedAddress, info.PhotoMetadata.FilePath));
                var parentNodeSpan = CacheNodeTimeSpan(parentNode, info);

                if (!parentNodeSpan.Children.ContainsKey(node.PlaceId))
                {
                    NodeTimeSpan span = new NodeTimeSpan();
                    span.Node = node;
                    span.LeftDate = info.PhotoMetadata.TakenDate;
                    span.RightDate = info.PhotoMetadata.TakenDate;

                    parentNodeSpan.Children.Add(node.PlaceId, span);
                }

                var childSpan = parentNodeSpan.Children[node.PlaceId];
                UpdateSpanTime(childSpan, info);

                return parentNodeSpan;
            }
        }

        private static void UpdateSpanTime(NodeTimeSpan span, PhotoInfo info)
        {
            //Console.WriteLine(string.Format("try to update span for {0}. left: {1}, right: {2}. PhotoTakenDate: {3}", span.Node.FormattedAddress, span.LeftDate.ToString("yyyy-MM-dd HH-mm-ss"), span.RightDate.ToString("yyyy-MM-dd HH-mm-ss"), info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss")));
            if (span.LeftDate > info.PhotoMetadata.TakenDate && span.LeftDate.Subtract(info.PhotoMetadata.TakenDate).TotalDays < config.SpanLimitDays)
            {
                span.LeftDate = info.PhotoMetadata.TakenDate;
                //Console.WriteLine(string.Format("after update left span for {0}. left: {1}, right: {2}", span.Node.FormattedAddress, span.LeftDate.ToString("yyyy-MM-dd HH-mm-ss"), span.RightDate.ToString("yyyy-MM-dd HH-mm-ss")));
            }

            if (span.RightDate < info.PhotoMetadata.TakenDate && info.PhotoMetadata.TakenDate.Subtract(span.RightDate).TotalDays < config.SpanLimitDays)
            {
                span.RightDate = info.PhotoMetadata.TakenDate;
                //Console.WriteLine(string.Format("after update right span for {0}. left: {1}, right: {2}", span.Node.FormattedAddress, span.LeftDate.ToString("yyyy-MM-dd HH-mm-ss"), span.RightDate.ToString("yyyy-MM-dd HH-mm-ss")));
            }
        }

        private static string CreateNoGPSNewPath(PhotoInfo info)
        {
            var format = string.Format("{0}/{1}.jpg", config.PhotoDestinationFolder, config.NewPhotoPathFormatNoneGPS);
            var pathParams = GeneratePathParams(noneGPSFilePathParts, info);

            var result = string.Format(format, pathParams);
            //Console.WriteLine(result + "  ********** " + pathParams.Length + " noneGPSFilePathParts count" + gpsFilePathParts.Count);

            return result;
            //return string.Format(@"{0}/!NoGPS/{1}/{2}/{2}_{3}.jpg", config.PhotoDestinationFolder, info.PhotoMetadata.TakenDate.ToString("yyyy-MM"), info.PhotoMetadata.Model, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"));
        }

        private static string CreateGPSNewPath(PhotoInfo info)
        {
            var format = string.Format("{0}/{1}.jpg", config.PhotoDestinationFolder, config.NewPhotoPathFormatGPS);
            var pathParams = GeneratePathParams(gpsFilePathParts, info);

            var result = string.Format(format, pathParams);
            //Console.WriteLine(result + "  ********** " + pathParams.Length + " gpsFilePathParts count" + gpsFilePathParts.Count);
            return result;
            //return string.Format(@"{0}/{1} {4:yyyy.MM}/{2}/{3}/{1}_{2}_{4:yyyy-MM-dd HH-mm-ss}.jpg", config.PhotoDestinationFolder, info.Country, info.City, info.PhotoMetadata.Model, info.PhotoMetadata.TakenDate);
        }

        private static object[] GeneratePathParams(Dictionary<int, string> pathParts, PhotoInfo info)
        {
            object[] results = new object[pathParts.Count];

            for (var i = 0; i < pathParts.Count; i++)
            {
                results.SetValue(SelectPhotoInfoProperty(pathParts[i], info), i);
            }

            return results;
        }

        private static object SelectPhotoInfoProperty(string pathPart, PhotoInfo info)
        {
            object result = string.Empty;

            switch (pathPart)
            {
                case PATH_COUNTRY:
                    result = info.Country;
                    break;
                case PATH_CITY:
                    result = info.City;
                    break;
                case PATH_DEVICE:
                    result = info.PhotoMetadata.Model;
                    break;
                case PATH_DATE:
                    result = info.PhotoMetadata.TakenDate;
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        private static void ProcessNoGPSPhotos()
        {
            foreach (var photo in noGPSPhotos)
            {
                AppendLocationForNoneGPSPhotos(photo);

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
                    var c = JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
                    ParseNewFilePath(c);
                    return c;
                }
            }
        }

        private static string ParseFilePath(string pathConfig, out Dictionary<int, string> pathParts)
        {
            pathParts = new Dictionary<int, string>();

            var pattern = @"\{(.*?)\}";
            var replacement = @"*";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            var result = rgx.Replace(pathConfig, replacement);
            Console.WriteLine(result);
            var pathSymbols = result.Split('*');
            MatchCollection matches = rgx.Matches(pathConfig);

            StringBuilder sb = new StringBuilder();

            if (pathSymbols.Length == matches.Count + 1)
            {
                for (var i = 1; i < pathSymbols.Length; i++)
                {
                    var match = matches[i - 1];
                    var matchValue = match.Value;
                    var format = string.Empty;

                    //Console.WriteLine("############## matchValue" + matchValue);

                    if (matchValue.Contains(":"))
                    {
                        var matchValueParts = matchValue.Split(':');
                        matchValue = matchValueParts[0];
                        format = matchValueParts[1].TrimEnd('}');
                        //Console.WriteLine("############## format" + format);
                    }

                    sb.Append("{");
                    sb.Append(string.Format("{0}", i - 1));
                    if (!string.IsNullOrWhiteSpace(format))
                    {
                        sb.Append(string.Format(":{0}", format));
                    }
                    sb.Append("}");
                    sb.Append(pathSymbols[i]);

                    pathParts.Add(i - 1, matchValue.Trim('{', '}'));
                }
            }

            //Console.WriteLine("############## " + sb.ToString());
            return sb.ToString();
        }

        private static void ParseNewFilePath(Config c)
        {
            c.NewPhotoPathFormatGPS = ParseFilePath(c.NewPhotoPathFormatGPS, out gpsFilePathParts);
            c.NewPhotoPathFormatNoneGPS = ParseFilePath(c.NewPhotoPathFormatNoneGPS, out noneGPSFilePathParts);
        }

        private static Tuple<string, string> GetCityAndCountryName(Node leaf)
        {
            var city = string.Empty;
            var country = string.Empty;

            foreach (var component in leaf.Components)
            {
                foreach (var componentType in component.Types)
                {
                    if (componentType == AddressComponentType.country)
                    {
                        country = component.LongName;
                        break;
                    }

                    if (componentType == AddressComponentType.locality)
                    {
                        city = component.LongName;
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(city) && (componentType == AddressComponentType.administrative_area_level_2 || componentType == AddressComponentType.administrative_area_level_2))
                    {
                        city = component.LongName;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                city = country;
            }

            return new Tuple<string, string>(city, country);
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

                    //Console.WriteLine(string.Format("[PhotoHelper]###################### exclude location: {0}", JsonConvert.SerializeObject(gps)));
                    break;
                }
            }

            return result;
        }

        private static void AppendLocationForNoneGPSPhotos(PhotoInfo info)
        {
            /*foreach (var item in locationTimeSpan)
            {
                var leftTime = item.Value.Item1.Subtract(tolerance);
                var rightTime = item.Value.Item2.Add(tolerance);

                //var leftTime = item.Value.Item1;
                //var rightTime = item.Value.Item2;

                Console.WriteLine(string.Format("[PhotoHelper]###################### try to append location {0} to {1}. Photo Date {2}, left time: {3}, right time: {4}", info.City, info.PhotoMetadata.FilePath, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"), leftTime.ToString("yyyy-MM-dd HH-mm-ss"), rightTime.ToString("yyyy-MM-dd HH-mm-ss")));

                if (info.PhotoMetadata.TakenDate > leftTime && info.PhotoMetadata.TakenDate < rightTime)
                {
                    info.City = item.Key;
                    info.Country = item.Value.Item3;

                    Console.WriteLine(string.Format("[PhotoHelper]###################### append location {0} to {1}. Photo Date {2}, left time: {3}, right time: {4}", info.City, info.PhotoMetadata.FilePath, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"), leftTime.ToString("yyyy-MM-dd HH-mm-ss"), rightTime.ToString("yyyy-MM-dd HH-mm-ss")));

                    break;
                }
            }*/
            // Console.WriteLine(string.Format("try to append location for {0}", info.PhotoMetadata.FilePath));
            foreach (var item in nodeTimeSpanCache)
            {
                var node = MatchNodeByTimeSpan(info, item.Value);
                if (node != null)
                {
                    //Console.WriteLine(string.Format("matched top level {0} for {1}", node.FormattedAddress, info.PhotoMetadata.FilePath));

                    var cityAndCountryName = GetCityAndCountryName(node);
                    info.City = cityAndCountryName.Item1;
                    info.Country = cityAndCountryName.Item2;
                    info.Node = node;
                    break;
                }
            }
        }

        public static Node MatchNodeByTimeSpan(PhotoInfo info, NodeTimeSpan span)
        {
            //Console.WriteLine(string.Format("try to match {0} for {1}", span.Node.FormattedAddress, info.PhotoMetadata.FilePath));
            Node result = null;

            var tolerance = new TimeSpan(0, config.TimeTolerance, 0);
            var leftTime = span.LeftDate.Subtract(tolerance);
            var rightTime = span.RightDate.Add(tolerance);

            //var leftTime = span.LeftDate;
            //var rightTime = span.RightDate;

            //Console.WriteLine(string.Format("[PhotoHelper]###################### node span details: {0}", JsonConvert.SerializeObject(span)));

            //Console.WriteLine(string.Format("[PhotoHelper]###################### try to append location {0} to {1}. Photo Date {2}, left time: {3}, right time: {4}", span.Node.FormattedAddress, info.PhotoMetadata.FilePath, info.PhotoMetadata.TakenDate.ToString("yyyy-MM-dd HH-mm-ss"), leftTime.ToString("yyyy-MM-dd HH-mm-ss"), rightTime.ToString("yyyy-MM-dd HH-mm-ss")));
            if (info.PhotoMetadata.TakenDate > leftTime && info.PhotoMetadata.TakenDate < rightTime)
            {
                result = span.Node;
                //Console.WriteLine(string.Format("matched {0} for {1}", span.Node.FormattedAddress, info.PhotoMetadata.FilePath));
                if (span.Children.Count > 0)
                {
                    foreach (var childSpan in span.Children)
                    {
                        var childResult = MatchNodeByTimeSpan(info, childSpan.Value);
                        if (childResult != null)
                        {
                            //Console.WriteLine(string.Format("matched child level {0} for {1}", span.Node.FormattedAddress, info.PhotoMetadata.FilePath));
                            result = childResult;
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}