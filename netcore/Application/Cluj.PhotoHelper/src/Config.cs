using Newtonsoft.Json;

namespace Cluj.PhotoHelper
{
    internal class Config
    {
        [JsonProperty("photo_src_path")]
        public string PhotoSourceFolder { get; set; }

        [JsonProperty("photo_dest_path")]
        public string PhotoDestinationFolder { get; set; }

        [JsonProperty("new_path_format")]
        public string NewPhotoPathFormat { get; set; }
    }
}