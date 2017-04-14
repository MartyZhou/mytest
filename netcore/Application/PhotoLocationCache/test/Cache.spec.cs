/*using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xunit;

namespace Cluj.PhotoLocation.Test
{
    public class CacheTest
    {
        //[Fact]
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
            Assert.Equal<int>(17, Cache.TestAreaLevel2Cache().Count);

            Assert.NotNull(Cache.TestCountryLevelCache());
            Assert.NotEmpty(Cache.TestCountryLevelCache());
            Assert.Equal<int>(9, Cache.TestCountryLevelCache().Count);
        }


        [Fact]
        public async void ParseCityNameSuccessfully()
        {
            var beijing = await Cache.GetLeafNode('N', 40.0608159, 'E', 116.514625).ConfigureAwait(false);

            Assert.Equal<string>("Chaoyang, Beijing, China", beijing.FormattedAddress);

            var boston = await Cache.GetLeafNode('N', 42.32788, 'W', 70.849455).ConfigureAwait(false);

            Assert.Equal<string>("Boston, MA, USA", boston.FormattedAddress);

            var capetown = await Cache.GetLeafNode('S', 30, 'E', 22).ConfigureAwait(false);

            Assert.Equal<string>("South Africa", capetown.FormattedAddress);

            var buenos = await Cache.GetLeafNode('S', 34.6, 'W', 58.4).ConfigureAwait(false);

            Assert.Equal<string>("Balvanera, Autonomous City of Buenos Aires, Argentina", buenos.FormattedAddress);

            var puertoPrincesa = await Cache.GetLeafNode('N', 11.2597217559814, 'E', 119.570831298828).ConfigureAwait(false);

            Assert.Equal<string>("Unnamed Road, El Nido, Philippines", puertoPrincesa.FormattedAddress);
        }

        [Fact]
        public async void NodesAreLinked()
        {
            var leafCache = Cache.TestLeafAddressCache();
            var totalCache = Cache.TestTotalAddressCache();

            Assert.NotEmpty(leafCache);
            Assert.NotEmpty(totalCache);

            var leaf = leafCache["ChIJB4CXTdVhzB0RI8X6FPKfvXM"];
            Assert.Equal<string>("83 S Perimeter Rd, Robben Island, 7400, South Africa", leaf.FormattedAddress);
            Assert.Equal<LocationType>(LocationType.ROOFTOP, leaf.LocationType);
            Assert.NotNull(leaf.Parent);
            Assert.Equal<string>("ChIJURLu2YmmNBwRoOikHwxjXeg", leaf.Parent.PlaceId);

            Console.WriteLine(string.Format("Total address cache count: {0}", totalCache.Count));
            Console.WriteLine(string.Format("Leaf address cache count: {0}", leafCache.Count));

            var buenosNode = await Cache.GetLeafNode('S', 34.6, 'W', 58.4).ConfigureAwait(false);
            Assert.NotNull(buenosNode);
            Assert.Equal<string>("Balvanera, Autonomous City of Buenos Aires, Argentina", buenosNode.FormattedAddress);
            Assert.Equal<LocationType>(LocationType.APPROXIMATE, buenosNode.LocationType);
            Assert.Equal<string>("ChIJUaOV2u_KvJURIbHgKZTw24g", buenosNode.PlaceId);
        }

        //[Fact]
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
        }
    }
}*/