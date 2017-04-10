using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cluj.PhotoLocation
{
    public static class Cache
    {
        private const string API_KEY = "AIzaSyDQw1khA5tgbDFLv4pGRd1_yOp747LnXdE";

        public static async Task<string> GetCityName(char latRef, double lat, char lonRef, double lon)
        {
            var city = string.Empty;

            var address = await LoadAddressAsync(latRef, lat, lonRef, lon).ConfigureAwait(false);

            foreach (var addr in address.Results)
            {
                foreach (var addressType in addr.Types)
                {
                    if (addressType == "locality")
                    {
                        foreach (var component in addr.AddressComponents)
                        {
                            if ((component.Types[0] == "country" && component.Types[1] == "political")
                            || (component.Types[0] == "administrative_area_level_1" && component.Types[1] == "political"))
                            {
                                city = component.LongName;
                            }
                            else if (component.Types[0] == "locality" && component.Types[1] == "political")
                            {
                                city = component.LongName;
                                break;
                            }
                        }
                    }
                }

            }

            return city;
        }

        private static async Task<AddressResult> LoadAddressAsync(char latRef, double lat, char lonRef, double lon)
        {
            return await LoadAddressFromCacheAsync(latRef, lat, lonRef, lon).ConfigureAwait(false);
        }

        private static async Task<AddressResult> LoadAddressFromServerAsync(char latRef, double lat, char lonRef, double lon)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}", latRef == 'N' ? lat : -lat, lonRef == 'E' ? lon : -lon, API_KEY);

                    var response = await httpClient.GetAsync(url).ConfigureAwait(false);

                    using (FileStream fileStream = new FileStream(GenerateFileName(latRef, lat, lonRef, lon), FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fileStream).ConfigureAwait(false);
                    }

                    using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync().ConfigureAwait(false)))
                    {
                        var addressString = reader.ReadToEnd();
                        return JsonConvert.DeserializeObject<AddressResult>(addressString);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Failed to call google map API: {0}", e.Message));
                return new AddressResult();
            }
        }

        private static string GenerateFileName(char latRef, double lat, char lonRef, double lon)
        {
            return string.Format("{0}_{1}_{2}_{3}.json", latRef, lat, lonRef, lon);
        }

        private static async Task<AddressResult> LoadAddressFromCacheAsync(char latRef, double lat, char lonRef, double lon)
        {
            using (var stream = new FileStream("./test/data/SampleAddressHyderabad.json", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    var addressString = reader.ReadToEnd();
                    var address = JsonConvert.DeserializeObject<AddressResult>(addressString);

                    return await Task.FromResult(address);
                }
            }
        }
    }
}