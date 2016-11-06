using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace KindleBook
{
    [XmlRoot("package")]
    public class OPF
    {
        [XmlAttribute("unique-identifier")]
        public string ID { get; set; }

        [XmlElement("metadata")]
        public OPFMetadata Metadata { get; set; }

        [XmlElement("spine")]
        //[XmlArray("spine")]
        //[XmlArrayItem("itemref2")]
        public OPFSpine Spine { get; set; }

        [XmlArray("manifest")]
        [XmlArrayItem("item")]
        public List<OPFItem> Items { get; set; }

        [XmlArray("guide")]
        [XmlArrayItem("reference")]
        public List<OPFReference> RefItems { get; set; }
    }

    public class OPFMetadata
    {
        [XmlElement("dc-metadata")]
        public DCMetadata DC { get; set; }

        [XmlElement("x-metadata")]
        public XMetadata X { get; set; }
    }

    public class DCMetadata
    {
        [XmlElement("dc:title")]
        public string Title { get; set; }

        [XmlElement("dc:creator")]
        public string Creator { get; set; }

        [XmlElement("dc:publisher")]
        public string Publisher { get; set; }

        [XmlElement("dc:subject")]
        public string Subject { get; set; }

        [XmlElement("dc:date")]
        public DateTime DateTime { get; set; }

        [XmlElement("dc:description")]
        public string Description { get; set; }
    }

    public class XMetadata
    {
        [XmlElement("output")]
        public XOutput Output { get; set; }
    }

    public class XOutput
    {

        [XmlAttribute("content-type")]
        public string ContentType { get; set; }

        [XmlAttribute("encoding")]
        public string Encoding { get; set; }
    }

    public class OPFManifest
    {
        [XmlArrayItem("item")]
        public List<OPFItem> Items { get; set; }
    }

    public class OPFSpine
    {
        [XmlAttribute("toc")]
        public string Toc { get; set; }

        [XmlElement("itemref")]
        public List<OPFItemRef> Items { get; set; }
    }

    public class OPFItem
    {
        [XmlAttribute("href")]
        public string Href { get; set; }

        [XmlAttribute("media-type")]
        public string Id { get; set; }

        [XmlAttribute("id")]
        public string MediaType { get; set; }
    }

    public class OPFItemRef
    {
        [XmlAttribute("idref")]
        public string IdRef { get; set; }
    }

    public class OPFReference
    {
        [XmlAttribute("href")]
        public string Href { get; set; }

        [XmlAttribute("type")]
        public string RefType { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }
    }

    public class OPFGuide
    {
        [XmlElement("reference")]
        public OPFReference Reference { get; set; }
    }
}