using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace KindleBook
{
    public class KindleBookSpec
    {
        [Fact]
        public async Task LoadSeed()
        {
            string url = "http://www.businessmagazin.ro/rss-feed.xml";
            using (HttpClient client = new HttpClient())
            {
                // Stream stream = await client.GetStreamAsync(url).ConfigureAwait(false);
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                FileStream fileStream = new FileStream("./KindleBook/bm.xml", FileMode.Create);
                await response.Content.CopyToAsync(fileStream).ConfigureAwait(false);
            }
        }

        [Fact]
        public void ReadLocalSeed()
        {
            XmlDocument doc = new XmlDocument();
            using (FileStream fileStream = new FileStream("./KindleBook/bm.xml", FileMode.Open))
            {
                doc.Load(fileStream);

                Assert.Equal<string>("BusinessMagazin", doc["rss"]["channel"]["title"].InnerText);
            }
        }

        [Fact]
        public void SerializerOPF()
        {
            OPF opf = new OPF();
            opf.ID = "tst";
            DCMetadata dc = new DCMetadata();
            dc.Title = "BM";
            dc.Creator = "Marty";
            dc.DateTime = DateTime.Now;
            dc.Description = "test";
            dc.Publisher = "marty";
            dc.Subject = "bm";

            XMetadata x = new XMetadata();
            XOutput xoutput = new XOutput();
            xoutput.ContentType = "application/x-mobipocket-subscription-magazine";
            xoutput.Encoding = "utf-8";
            x.Output = xoutput;

            OPFMetadata opfMetadata = new OPFMetadata();
            opfMetadata.DC = dc;
            opfMetadata.X = x;
            opf.Metadata = opfMetadata;

            OPFManifest manifest = new OPFManifest();
            manifest.Items = new List<OPFItem>();
            OPFItem item1 = new OPFItem();
            item1.Href = "test";
            item1.Id = "fsakl";
            item1.MediaType = "application/xhtml+xml";
            manifest.Items.Add(item1);
            manifest.Items.Add(item1);

            opf.Items = manifest.Items;

            OPFReference opfref = new OPFReference();
            opfref.Href = "test";
            opfref.RefType = "type";
            opfref.Title = "table of contents";

            List<OPFReference> guide = new List<OPFReference>();
            guide.Add(opfref);
            guide.Add(opfref);

            opf.RefItems = guide;

            OPFSpine spine = new OPFSpine();
            spine.Items = new List<OPFItemRef>();
            OPFItemRef itemRef = new OPFItemRef();
            itemRef.IdRef = "tstes";
            spine.Items.Add(itemRef);
            spine.Items.Add(itemRef);

            //spine.Add(itemRef);
            //spine.Add(itemRef);
            spine.Toc = "test";

            opf.Spine = spine;

            using (FileStream fileStream = new FileStream("./KindleBook/bm.opf", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OPF));
                serializer.Serialize(fileStream, opf);
            }
        }
    }
}