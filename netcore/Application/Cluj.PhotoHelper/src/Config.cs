using System.Collections.Generic;
using Cluj.PhotoLocation;
using Newtonsoft.Json;

namespace Cluj.PhotoHelper
{
    internal class Config
    {
        [JsonProperty("photo_src_path")]
        public string PhotoSourceFolder { get; set; }

        [JsonProperty("photo_dest_path")]
        public string PhotoDestinationFolder { get; set; }
        [JsonProperty("new_path_format_gps")]
        public string NewPhotoPathFormatGPS { get; set; }
        [JsonProperty("new_path_format")]
        public string NewPhotoPathFormatNoneGPS { get; set; }

        [JsonProperty("excluded_cities")]
        public List<string> ExcludedCities { get; set; }

        [JsonProperty("excluded_area")]
        public List<Bounds> ExcludedArea { get; set; }

        [JsonProperty("time_tolerance")]
        public int TimeTolerance { get; set; }
        [JsonProperty("span_limit_days")]
        public int SpanLimitDays { get; set; }
    }
}