using Xunit;

namespace Cluj.PhotoLocation.Test
{
    public class CacheTest
    {

        [Fact]
        public async void ParseCityNameSuccessfully()
        {
            var result = await Cache.GetCityName('N', 0, 'E', 0).ConfigureAwait(false);
            
            Assert.Equal<string>("Hyderabad", result);
        }
    }
}