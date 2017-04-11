using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xunit;

namespace Cluj.PhotoLocation.Test
{
    public class CacheTest
    {
        [Fact]
        public void InitCacheSuccessfully()
        {
            Assert.NotNull(Cache.TestCityLevelCache());
            Assert.NotEmpty(Cache.TestCityLevelCache());
            Assert.Equal<int>(42, Cache.TestCityLevelCache().Count);

            Assert.NotNull(Cache.TestAreaLevel1Cache());
            Assert.NotEmpty(Cache.TestAreaLevel1Cache());
            Assert.Equal<int>(19, Cache.TestAreaLevel1Cache().Count);

            Assert.NotNull(Cache.TestAreaLevel2Cache());
            Assert.NotEmpty(Cache.TestAreaLevel2Cache());
            Assert.Equal<int>(16, Cache.TestAreaLevel2Cache().Count);

            Assert.NotNull(Cache.TestCountryLevelCache());
            Assert.NotEmpty(Cache.TestCountryLevelCache());
            Assert.Equal<int>(9, Cache.TestCountryLevelCache().Count);
        }


        [Fact]
        public async void ParseCityNameSuccessfully()
        {
            var beijing = await Cache.GetCityName('N', 40.0608159, 'E', 116.514625).ConfigureAwait(false);

            Assert.Equal<string>("Beijing", beijing);

            var boston = await Cache.GetCityName('N', 42.32788, 'W', 70.849455).ConfigureAwait(false);

            Assert.Equal<string>("Boston", boston);

            var capetown = await Cache.GetCityName('S', 30, 'E', 22).ConfigureAwait(false);

            Assert.Equal<string>("Benede Oranje", capetown);

            var buenos = await Cache.GetCityName('S', 34.6, 'W', 58.4).ConfigureAwait(false);

            Assert.Equal<string>("Buenos Aires", buenos);
        }

        /*[Fact]
        public void ParseRawAddress()
        {
            using (var stream = new FileStream("./test/data/metadata_google.json", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    var addressString = reader.ReadToEnd();
                    var addresses = JsonConvert.DeserializeObject<List<AddressResult>>(addressString);

                    foreach (var address in addresses)
                    {
                        var fileName = string.Format("./test/data/gps_{0}.json", address.Results[0].PlaceId);
                        File.WriteAllText(fileName, JsonConvert.SerializeObject(address));
                    }
                }
            }
        }*/
    }
}