using System;
using System.IO;
using System.Xml;
using Xunit;

namespace netcore.Xml
{
    public class XmlSpec
    {
        /*[Fact]
        public void XmlParse()
        {
            XmlDocument xml = new XmlDocument();
            using (FileStream fileStream = new FileStream("./Xml/bbcseed.xml", FileMode.Open))
            {
                xml.Load(fileStream);
            }

            XmlNodeList feeds = xml["rss"]["channel"].ChildNodes;

            //foreach(XmlNode node in feeds)
            //{
            //    if(node.Name == "item")
            //    {
            //        Console.WriteLine(node["link"].InnerText);
            //    }
            //}

            // Assert.Equal("rss", xml.ChildNodes[2].Name);
        }*/

        [Fact]
        public void HtmlParse()
        {
            // For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.
            XmlDocument xml = new XmlDocument();
            //XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            //xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;
            //xmlReaderSettings.IgnoreComments = true;
            //xmlReaderSettings.IgnoreProcessingInstructions = false;
            //xmlReaderSettings.IgnoreWhitespace = true;
            Stream stream = new FileStream("./Http/bbcnews.html", FileMode.Open);
            //XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings);
            // XmlTextReader xmlTextReader = new XmlTextReader(new FileStream("./Http/bbcnews.html", FileMode.Open));
            // xml.Load(xmlReader);

            HtmlReader htmlReader = new HtmlReader(stream);
            xml.Load(htmlReader);

            //string html = File.ReadAllText("./Http/bbcnews.html").Replace("&", " and ").Replace("<!DOCTYPE html>", "");
            //xml.LoadXml(html);
            Console.WriteLine("HtmlParse " + xml["html"].GetAttribute("lang") + " " + xml["html"].GetAttribute("id"));
            Console.WriteLine("HtmlParse " + xml["html"]["head"].Attributes[0].Name);
            Console.WriteLine("HtmlParse " + xml["html"]["head"].GetAttribute(" prefix"));
            Console.WriteLine("HtmlParse " + xml.ChildNodes.Count);
            Console.WriteLine("HtmlParse " + xml.ChildNodes[0].Name);
            Console.WriteLine("HtmlParse " + xml.ChildNodes[0].ChildNodes.Count);
            Console.WriteLine("HtmlParse " + xml.ChildNodes[0].ChildNodes[0].Name);
            Console.WriteLine("HtmlParse " + xml.ChildNodes[0].ChildNodes[0].ChildNodes.Count);
            Console.WriteLine("HtmlParse " + xml.ChildNodes[0].ChildNodes[0].ChildNodes[0].Name);
            Console.WriteLine("HtmlParse " + xml.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes.Count);
        }
    }
}