using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace BlazorRSSFeed.Controller;

public class RssFeedController : ControllerBase
{
    [ResponseCache(Duration = 1200)]
    [HttpGet]
    [Route("feed.rss")]
    public async Task<IActionResult> GetRssFeed()
    {
        // Get the current url
        var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        
        // This object reflects our RSS feed root item
        var feed = new SyndicationFeed(
            "Title",
            "This is a sample title",
            new Uri(url))
        {
            // You could create a list here of your blog posts for example
            Items = new[]
            {
                new SyndicationItem(
                    "A Blog Post",
                    "Somecontent",
                    new Uri(url + "/url-to-your-sub-item"))
            }
        };

        // Create the XML Writer with it's settings
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true, // Makes it easier to read for humans
            Async = true, // You can omit this if you don't use the async API
        };

        using var stream = new MemoryStream();
        await using var xmlWriter = XmlWriter.Create(stream, settings);
        // Create the RSS Feed
        var rssFormatter = new Rss20FeedFormatter(feed, false);
        rssFormatter.WriteTo(xmlWriter);
        await xmlWriter.FlushAsync();

        return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
    }
}