using Xunit;
using System.IO;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html;
using AngleSharp.Dom.Html;

namespace AngleSharpSpec
{
    public class ASSSpec
    {
        [Fact]
        public async Task TestAngleSharp()
        {
            var context = BrowsingContext.New();
            var source = @"<!DOCTYPE html><html><head></head><body><span>test</span></body></html>";
            var document = await context.OpenAsync(res => res.Content(source));

            Assert.NotNull(document);
        }

        [Fact]
        public async Task TestAngleSharpStreamBBC()
        {
            var context = BrowsingContext.New();
            Stream stream = new FileStream("./Http/bbcnews.html", FileMode.Open);
            var document = await context.OpenAsync(res => res.Content(stream, true));

            var titleElement = document.QuerySelector("title");
            var storyElement = document.QuerySelector(".story-body__inner");
            var body = document.CreateElement("body");
            body.AppendChild(storyElement);

            while (document.Head.Children.Length > 0)
            {
                document.Head.RemoveChild(document.Head.FirstChild);
            }

            document.Head.AppendChild(titleElement);
            document.Body.Remove();
            document.Body = (IHtmlElement)body;

            FileStream fileStream = new FileStream("./Http/bbcnews2.html", FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);
            document.ToHtml(writer, HtmlMarkupFormatter.Instance);
            writer.Flush();

            Assert.NotNull(document);
        }

        [Fact]
        public async Task TestAngleSharpStreamBM()
        {
            var context = BrowsingContext.New();
            Stream stream = new FileStream("./KindleBook/BM/2.html", FileMode.Open);
            var document = await context.OpenAsync(res => res.Content(stream, true));

            var contentTypeElement = document.QuerySelector("meta");
            var titleElement = document.QuerySelector("title");
            var storyElement = document.QuerySelector(".a-entry");
            var body = document.CreateElement("body");
            body.AppendChild(storyElement);

            while (document.Head.Children.Length > 0)
            {
                document.Head.RemoveChild(document.Head.FirstChild);
            }

            document.Head.AppendChild(contentTypeElement);
            document.Head.AppendChild(titleElement);
            document.Body.Remove();
            document.Body = (IHtmlElement)body;

            FileStream fileStream = new FileStream("./KindleBook/BM/2_2.html", FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);
            document.ToHtml(writer, HtmlMarkupFormatter.Instance);
            writer.Flush();

            Assert.NotNull(document);
        }
    }
}