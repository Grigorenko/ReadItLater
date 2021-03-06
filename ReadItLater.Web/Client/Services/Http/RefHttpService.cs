using ReadItLater.Data;
using ReadItLater.Web.Client.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ReadItLater.Web.Client.Services.Http
{
    public class RefHttpService
    {
        private readonly UserToken userToken;
        private readonly HttpClient client;

        public RefHttpService(UserToken userToken, HttpClient client)
        {
            this.userToken = userToken;
            this.client = client;
        }

        public async Task<RefProjection> GetByIdAsync(Guid id)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            return await client.GetFromResult<RefProjection>($"refs/{id}");
        }

        public async Task<RefProjection> GetByUrlAsync(string url)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            return await client.GetFromResult<RefProjection>("get-ref?url=" + HttpUtility.UrlEncode(url));
        }

        public async Task<Ref[]> SearchAsync(Guid? folderId, Guid? tagId, string searchTerm, int offset = 0, int limit = 50, string sort = "")
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            var url = BuildUrl(folderId, tagId, offset, limit, sort, searchTerm, "search");
            return await client.GetFromResult<Ref[]>(url);
        }

        public async Task<Ref[]> GetAsync(Guid? folderId, Guid? tagId, int offset = 0, int limit = 50, string sort = "")
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            var url = BuildUrl(folderId, tagId, offset, limit, sort, null);
            return await client.GetFromResult<Ref[]>(url);
        }

        public async Task CreateAsync(RefProjection projection)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            using (var content = new StringContent(projection.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await client.PostAsync("refs", content);
            }
        }

        public async Task UpdateAsync(RefProjection projection)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            using (var content = new StringContent(projection.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await client.PutAsync("refs", content);
            }
        }

        public async Task UpdateCountOfView(Guid refId)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            await client.PatchAsync("refs/" + refId, null);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            await client.DeleteAsync($"refs/{id}");
        }

        private string BuildUrl(Guid? folderId, Guid? tagId, int offset, int limit, string sort, string searchTerm, string urlPath = null)
        {
            var parameters = GetParameters(folderId, tagId, offset, limit, sort, searchTerm);
            var builder = new UriBuilder("", "")
            {
                Path = "refs" + (string.IsNullOrEmpty(urlPath) ? string.Empty : $"/{urlPath}"),
                Query = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))
            };

            return builder.ToString().TrimStart('/');
        }

        private IDictionary<string, string> GetParameters(Guid? folderId, Guid? tagId, int offset, int limit, string sort, string searchTerm)
        {
            var parameters = new Dictionary<string, string>
            {
                ["folderId"] = folderId?.ToString(),
                ["tagId"] = tagId?.ToString(),
                [nameof(offset)] = offset.ToString(),
                [nameof(limit)] = limit.ToString(),
                [nameof(sort)] = sort,
                [nameof(searchTerm)] = searchTerm
            };

            return parameters
                .Where(p => !string.IsNullOrEmpty(p.Value))
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
