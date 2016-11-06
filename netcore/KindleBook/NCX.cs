using System.Collections.Generic;
using System.Xml.Serialization;

namespace KindleBook
{
    [XmlRoot("ncx")]
    public class NCX
    {
        public NCX()
        {
            Head = new List<NCXMeta>();
            NavMap = new List<NavPoint>();
        }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlArray("head")]
        [XmlArrayItem("meta")]
        public List<NCXMeta> Head { get; set; }

        [XmlElement("docTitle")]
        public NCXText Title { get; set; }

        [XmlElement("docAuthor")]
        public NCXText Author { get; set; }

        [XmlArray("navMap")]
        [XmlArrayItem("navPoint")]
        public List<NavPoint> NavMap { get; set; }
    }

    public class NCXMeta
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("content")]
        public string Content { get; set; }
    }

    public class NCXText
    {
        [XmlElement("text")]
        public string Text { get; set; }

        public NCXText() { }

        public NCXText(string text)
        {
            Text = text;
        }
    }

    public class NCXMBP
    {
        public NCXMBP() { }

        public NCXMBP(string name, string text)
        {
            Name = name;
            Text = text;
        }
        
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    public class NavPoint
    {
        public NavPoint()
        {
            Label = new NCXText();
            Content = new NavContent();
        }

        [XmlAttribute("playOrder")]
        public string Order { get; set; }

        [XmlAttribute("class")]
        public string NavClass { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("navLabel")]
        public NCXText Label { get; set; }

        [XmlElement("content")]
        public NavContent Content { get; set; }

        [XmlElement(ElementName = "meta", Namespace = "http://mbp.mock")]
        public List<NCXMBP> MBP { get; set; }

        [XmlElement("navPoint")]
        public List<NavPoint> Items { get; set; }
    }

    public class NavContent
    {
        [XmlAttribute("src")]
        public string Src { get; set; }
    }
}