using Microsoft.AspNetCore.Mvc;
using ReadItLater.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadItLater.Data;

namespace ReadItLater.Web.Server.Controllers
{
    public class HtmlController : ControllerBase
    {
        private readonly IHtmlParser htmlParser;

        public HtmlController(IHtmlParser htmlParser)
        {
            this.htmlParser = htmlParser;
        }

        [HttpGet("get-ref")]
        public async Task<Ref> GetMeta([FromQuery] string url)
        {
            var meta = await htmlParser.GetMetaAsync(url);
            //var tags = TryGetTags(url, meta.title);

            return new Ref
            {
                Id = Guid.NewGuid(),
                //FolderId = Storage.DefaultFolder.Id,
                Title = meta.title,
                Url = url,
                Image = meta.image,
                Priority = Priority.Low,
                //Tags = tags,
                Date = DateTime.Now
            };
        }

        private Tag[] TryGetTags(string url, string title)
        {
            IDictionary<string, Tag> KnownTags = new Dictionary<string, Tag>
            {
                //["youtu"] = Storage.Tags.Single(x => x.Name.Equals("youtube", StringComparison.OrdinalIgnoreCase)),
                //["proglib"] = Storage.Tags.Single(x => x.Name.Equals("proglib", StringComparison.OrdinalIgnoreCase)),
                //["tproger"] = Storage.Tags.Single(x => x.Name.Equals("tproger", StringComparison.OrdinalIgnoreCase)),
                //["delegat"] = Storage.Tags.Single(x => x.Name.Equals("delegate", StringComparison.OrdinalIgnoreCase)),
                //["c# 8"] = Storage.Tags.Single(x => x.Name.Equals("c# 8", StringComparison.OrdinalIgnoreCase)),
                //["c#"] = Storage.Tags.Single(x => x.Name.Equals("c#", StringComparison.OrdinalIgnoreCase)),
            };
            var tags = new List<Tag>();

            KnownTags.Keys
                .Where(t => url.Contains(t))
                .ToList()
                .ForEach(x => tags.Add(KnownTags[x]));
            KnownTags.Keys
                .Where(t => title.Contains(t))
                .ToList()
                .ForEach(x => tags.Add(KnownTags[x]));

            return tags.ToArray();

        }
    }
}
