/*using System.IO;
using Newtonsoft.Json;
using Xunit;

namespace Cluj.PhotoLocation.Test
{
    public class DeserializeRawAddress
    {

        [Fact]
        public void DeserializeJsonSuccessfully()
        {
            using (var stream = new FileStream("./test/data/SampleAddressSuzhou.json", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    var addressString = reader.ReadToEnd();
                    var address = JsonConvert.DeserializeObject<AddressResult>(addressString);

                    Assert.NotNull(address);
                    Assert.Equal<string>("street_address", address.Results[0].Types[0]);
                }
            }
        }
    }
}
*/