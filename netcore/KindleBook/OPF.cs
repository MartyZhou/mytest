using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace KindleBook
{
    [XmlRoot("package")]
    public class OPF
    {
        public OPF()
        {
            Metadata = new OPFMetadata();
            Spine = new OPFSpine();
            Items = new List<OPFItem>();
            RefItems = new List<OPFReference>();
        }

        [XmlAttribute("unique-identifier")]
        public string ID { get; set; }

        [XmlElement("metadata")]
        public OPFMetadata Metadata { get; set; }

        [XmlElement("spine")]
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
        public OPFMetadata()
        {
            DC = new DCMetadata();
            X = new XMetadata();
        }

        [XmlElement("dc-metadata")]
        public DCMetadata DC { get; set; }

        [XmlElement("x-metadata")]
        public XMetadata X { get; set; }
    }

    public class DCMetadata
    {
        [XmlElement(ElementName = "language", Namespace = "http://dc.mock")]
        public string Language { get; set; }

        [XmlElement(ElementName = "title", Namespace = "http://dc.mock")]
        public string Title { get; set; }

        [XmlElement(ElementName = "creator", Namespace = "http://dc.mock")]
        public string Creator { get; set; }

        [XmlElement(ElementName = "publisher", Namespace = "http://dc.mock")]
        public string Publisher { get; set; }

        [XmlElement(ElementName = "subject", Namespace = "http://dc.mock")]
        public string Subject { get; set; }

        [XmlElement(ElementName = "date", Namespace = "http://dc.mock")]
        public DateTime DateTime { get; set; }

        [XmlElement(ElementName = "description", Namespace = "http://dc.mock")]
        public string Description { get; set; }
    }

    public class XMetadata
    {
        public XMetadata()
        {
            Output = new XOutput();
        }

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
        public OPFManifest()
        {
            Items = new List<OPFItem>();
        }

        [XmlArrayItem("item")]
        public List<OPFItem> Items { get; set; }
    }

    public class OPFSpine
    {
        public OPFSpine()
        {
            Items = new List<OPFItemRef>();
        }

        [XmlAttribute("toc")]
        public string Toc { get; set; }

        [XmlElement("itemref")]
        public List<OPFItemRef> Items { get; set; }
    }

    public class OPFItem
    {
        [XmlAttribute("href")]
        public string Href { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("media-type")]
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
        public OPFGuide()
        {
            Reference = new OPFReference();
        }
        
        [XmlElement("reference")]
        public OPFReference Reference { get; set; }
    }
}