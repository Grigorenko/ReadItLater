using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;

namespace ReadItLater.HtmlParser
{
    public class HtmlParser : IHtmlParser
    {
        public async Task<(string? title, string? image)> GetMetaAsync(string url)
        {
            if (url.Contains("youtu"))
            {
                var code = url.Substring(url.LastIndexOf("/") + 1);
                var uri = new UriBuilder("http", "youtube.com") { Path = "get_video_info", Query = $"video_id={code}" }.ToString();
                var body = await GetBodyAsync(uri);
                var queryParams = HttpUtility.ParseQueryString(body);
                var json = queryParams.Get("player_response");
                var obj = JsonSerializer.Deserialize<YouTube>(json);
                
                return (obj?.VideoDetails?.Title, obj?.VideoDetails?.Thumbnail?.Thumbnails?.OrderByDescending(t => t.Width)?.First()?.Url);
            }

            var web = new HtmlWeb();
            HtmlDocument document = web.Load(url);

            var node = document.DocumentNode;
            var meta = node.SelectNodes("/html/head/meta");
            var title = meta
                .FirstOrDefault(m => m.Attributes.Any(a => a.Name == "property" && a.Value == "og:title"))
                ?.Attributes
                ?.SingleOrDefault(x => x.Name == "content")
                ?.Value;
            var img = meta
                .FirstOrDefault(m => m.Attributes.Any(a => a.Name == "property" && a.Value == "og:image"))
                ?.Attributes
                ?.SingleOrDefault(x => x.Name == "content")
                ?.Value;

            return (title, img);
        }

        private async Task<string> GetBodyAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(url);
                var body = await resp.Content.ReadAsStringAsync();

                return body;
            }
        }
    }
}
