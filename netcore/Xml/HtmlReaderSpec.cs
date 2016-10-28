using System;
using System.IO;
using System.Xml;
using Xunit;

namespace netcore.Xml
{
    public class HtmlReaderSpec
    {
        [Fact]
        public void XmlParse()
        {
            HtmlReader reader = new HtmlReader(null);

            // Assert.Equal("rss", xml.ChildNodes[2].Name);
        }
    }
}