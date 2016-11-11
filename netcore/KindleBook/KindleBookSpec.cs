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
        public void SerializeOPF()
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

            //opf.RefItems = guide;

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
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("dc", "http://dc.mock");
                serializer.Serialize(fileStream, opf, ns);
            }
        }

        [Fact]
        public void SerializeNCX()
        {
            NCX ncx = new NCX();
            ncx.Version = "text version";

            List<NCXMeta> metas = new List<NCXMeta>();
            NCXMeta meta = new NCXMeta();
            meta.Content = "test content";
            meta.Name = "test name";
            metas.Add(meta);
            metas.Add(meta);

            ncx.Head = metas;

            NCXText title = new NCXText("test");
            title.Text = "test title";

            ncx.Title = title;

            List<NavPoint> navMap = new List<NavPoint>();
            NavPoint root = new NavPoint();
            root.Content = new NavContent();
            root.Content.Src = "test src";

            root.Id = "testId";
            root.Label = new NCXText("label");
            root.Label.Text = "test label";

            NavPoint p2 = new NavPoint();
            p2.Content = new NavContent();
            p2.Content.Src = "test src";

            p2.Id = "testId";
            p2.Label = new NCXText("llll");
            p2.Label.Text = "test label";

            p2.MBP = new List<NCXMBP>();
            NCXMBP mbpname = new NCXMBP();
            mbpname.Name = "author";
            mbpname.Text = "test author";
            NCXMBP mbpdesc = new NCXMBP();
            mbpdesc.Name = "description";
            mbpdesc.Text = "this is a test description";

            p2.MBP.Add(mbpname);
            p2.MBP.Add(mbpdesc);

            root.Items = new List<NavPoint>();
            root.Items.Add(p2);
            root.Items.Add(p2);

            navMap.Add(root);

            ncx.NavMap = navMap;

            using (FileStream fileStream = new FileStream("./KindleBook/bm.ncx", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NCX));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("mbp", "http://mbp.mock");
                serializer.Serialize(fileStream, ncx, ns);
            }
        }

        [Fact]
        public async Task TestTest()
        {
            Test test = new Test();
            await test.Run();
        }
    }
}