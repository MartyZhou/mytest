using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace netcore.Http
{
    public class HttpSpec
    {
        [Fact]
        public async Task HttpGet()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://www.bbc.co.uk/news/world-37630460").ConfigureAwait(false);
            FileStream fileStream = new FileStream("./Http/bbcnews.html", FileMode.Create);
            await response.Content.CopyToAsync(fileStream).ConfigureAwait(false);

            Console.WriteLine(response);
            // Assert.Equal("rss", xml.ChildNodes[2].Name);
        }
    }
}