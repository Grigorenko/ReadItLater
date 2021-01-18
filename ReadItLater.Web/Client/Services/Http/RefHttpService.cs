using ReadItLater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ReadItLater.Web.Client.Services.Http
{
    public class RefHttpService
    {
        private readonly HttpClient client;

        public RefHttpService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<RefProjection> GetByIdAsync(Guid id)
        {
            return await client.GetFromJsonAsync<RefProjection>($"refs/{id}");
        }

        public async Task<RefProjection> GetByUrlAsync(string url)
        {
            return await client.GetFromJsonAsync<RefProjection>("get-ref?url=" + HttpUtility.UrlEncode(url));
        }

        public async Task<Ref[]> GetAsync(Guid? folderId, Guid? tagId)
        {
            var url = BuildUrl(folderId, tagId);
            return await client.GetFromJsonAsync<Ref[]>(url);
        }

        public async Task CreateAsync(RefProjection projection)
        {
            using (var content = new StringContent(projection.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await client.PostAsync("refs", content);
            }
        }

        public async Task UpdateAsync(RefProjection projection)
        {
            using (var content = new StringContent(projection.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await client.PutAsync("refs", content);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await client.DeleteAsync($"refs/{id}");
        }

        private string BuildUrl(Guid? folderId, Guid? tagId, int offset = 0, int limit = 25, string orderBy = "", string direction = "")
        {
            var parameters = new Dictionary<string, string>
            {
                ["folderId"] = folderId?.ToString(),
                ["tagId"] = tagId?.ToString(),
                [nameof(offset)] = offset.ToString(),
                [nameof(limit)] = limit.ToString(),
                [nameof(orderBy)] = orderBy,
                [nameof(direction)] = direction
            };

            var builder = new UriBuilder("", "")
            {
                Path = "refs",
                Query = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))
            };

            return builder.ToString().TrimStart('/');
        }
    }
}
