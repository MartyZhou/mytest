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
            Assert.NotNull(beijing);
            Assert.Equal<string>("155 Shun Huang Lu, Chaoyang Qu, Beijing Shi, China", beijing.FormattedAddress);

            var boston = await Cache.GetLeafNode('N', 42.32788, 'W', 70.849455).ConfigureAwait(false);
            Assert.NotNull(boston);
            Assert.Equal<string>("Plymouth County, MA, USA", boston.FormattedAddress);

            var capetown = await Cache.GetLeafNode('S', 30, 'E', 22).ConfigureAwait(false);
            Assert.NotNull(capetown);
            Assert.Equal<string>("South Africa", capetown.FormattedAddress);

            var buenos = await Cache.GetLeafNode('S', 34.6, 'W', 58.4).ConfigureAwait(false);
            Assert.NotNull(buenos);
            Assert.Equal<string>("Balvanera, Autonomous City of Buenos Aires, Argentina", buenos.FormattedAddress);
        }

        [Fact]
        public async void NodesAreLinked()
        {
            var leafCache = Cache.TestLeafAddressCache();
            var totalCache = Cache.TestTotalAddressCache();

            Assert.NotEmpty(leafCache);
            Assert.NotEmpty(totalCache);

            var leaf = leafCache["ChIJWUa248bgtjER3HD5mAfvesg"];
            Assert.Equal<string>("Unnamed Road, Pulau Perhentian Kecil, Kuala Besut, Terengganu, Malaysia", leaf.FormattedAddress);
            // Console.WriteLine(JsonConvert.SerializeObject(leaf));
            Assert.Equal<LocationType>(LocationType.GEOMETRIC_CENTER, leaf.LocationType);
            Assert.NotNull(leaf.Parent);
            Assert.Equal<string>("ChIJV9P7V3XdtjER_qPy7a_lj2o", leaf.Parent.PlaceId);

            Console.WriteLine(string.Format("Total address cache count: {0}", totalCache.Count));
            Console.WriteLine(string.Format("Leaf address cache count: {0}", leafCache.Count));

            var puertoPrincesa = await Cache.GetLeafNode('N', 5.8997220993042, 'E', 102.715270996094).ConfigureAwait(false);
            Assert.NotNull(puertoPrincesa);
            Assert.Equal<string>("Unnamed Road, Pulau Perhentian Kecil, Kuala Besut, Terengganu, Malaysia", puertoPrincesa.FormattedAddress);

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