using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace KindleBook
{
    public class Test
    {
        private const string NAV_CONTENTS = "nav-contents";
        private const string OPF_NAME = "kindle-book";
        private NCX ncx = new NCX();
        private OPF opf = new OPF();

        private Dictionary<string, NavPoint> navs = new Dictionary<string, NavPoint>();

        public Test()
        {
            ncx.Author = new NCXText("kindle");
            ncx.Title = new NCXText("Kindle Magazine");

            opf.Metadata.DC.Creator = "kindle";
            opf.Metadata.DC.Language = "en-GB";
            opf.Metadata.DC.DateTime = DateTime.Now;
            opf.Metadata.DC.Description = "kindle";
            opf.Metadata.DC.Publisher = "kindle";
            opf.Metadata.DC.Subject = "news";
            opf.Metadata.DC.Title = "kindle " + DateTime.Now.ToString();

            opf.Metadata.X.Output.ContentType = "application/x-mobipocket-subscription-magazine";
            opf.Metadata.X.Output.Encoding = "utf-8";

            opf.Spine.Toc = NAV_CONTENTS;
        }

        public async Task Run()
        {
            XmlDocument doc = ReadLocalSeed();
            await DownloadArticals(doc).ConfigureAwait(false);
            Write();
        }

        private XmlDocument ReadLocalSeed()
        {
            XmlDocument doc = new XmlDocument();
            using (FileStream fileStream = new FileStream("./KindleBook/bm.xml", FileMode.Open))
            {
                doc.Load(fileStream);
            }

            return doc;
        }

        private async Task DownloadArticals(XmlDocument doc)
        {
            int index = 0;
            using (HttpClient client = new HttpClient())
            {
                foreach (XmlNode node in doc["rss"]["channel"].ChildNodes)
                {
                    if (node.Name == "item" && index < 10)
                    {
                        index++;
                        string url = node["link"].InnerText;
                        string title = node["title"].InnerText;
                        string category = node["category"].InnerText;
                        string description = node["description"].InnerText;
                        string localHref = string.Format("BM/{0}.html", index);
                        string path = string.Format("./KindleBook/{0}", localHref);

                        HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                        using (FileStream fileStream = new FileStream(path, FileMode.Create))
                        {
                            await response.Content.CopyToAsync(fileStream).ConfigureAwait(false);
                            OPFItem item = new OPFItem();
                            item.Href = localHref;
                            item.Id = index.ToString();
                            item.MediaType = "application/xhtml+xml";
                            opf.Items.Add(item);

                            OPFItemRef itemRef = new OPFItemRef();
                            itemRef.IdRef = index.ToString();
                            opf.Spine.Items.Add(itemRef);

                            OPFReference reference = new OPFReference();
                            reference.Href = localHref;
                            reference.Title = title;
                            reference.RefType = "toc";
                            opf.RefItems.Add(reference);

                            if (!navs.ContainsKey(category))
                            {
                                NavPoint nav = new NavPoint();
                                nav.Label.Text = category;
                                nav.Id = category;
                                nav.NavClass = "section";
                                nav.Content.Src = string.Format("{0}-content.html", category);
                                nav.Items = new List<NavPoint>();

                                navs.Add(category, nav);
                            }

                            NavPoint articalNav = new NavPoint();
                            articalNav.Id = index.ToString();
                            articalNav.Label.Text = title;
                            articalNav.NavClass = "artical";
                            articalNav.Content.Src = localHref;
                            articalNav.MBP = new List<NCXMBP>();
                            articalNav.MBP.Add(new NCXMBP("name", title));
                            articalNav.MBP.Add(new NCXMBP("description", description));

                            navs[category].Items.Add(articalNav);

                            await Task.Delay(5000);
                        }
                    }
                }
            }
        }

        private void Write()
        {
            ncx.NavMap = navs.Values.ToList();
            using (FileStream fileStream = new FileStream(string.Format("./KindleBook/{0}.ncx", NAV_CONTENTS), FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NCX));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("mbp", "http://mbp.mock");
                serializer.Serialize(fileStream, ncx, ns);
            }

            using (FileStream fileStream = new FileStream(string.Format("./KindleBook/{0}.opf", OPF_NAME), FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OPF));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("dc", "http://dc.mock");
                serializer.Serialize(fileStream, opf, ns);
            }
        }

    }
}