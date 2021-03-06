using ReadItLater.Data;
using ReadItLater.Web.Client.Services.Auth;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services.Http
{
    public class FolderHttpService
    {
        private readonly UserToken userToken;
        private readonly HttpClient client;

        public FolderHttpService(UserToken userToken, HttpClient client)
        {
            this.userToken = userToken;
            this.client = client;
        }

        public async Task<FolderListItemProjection[]> GetListAsync()
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            return await client.GetFromResult<FolderListItemProjection[]>($"Folders/list");
        }

        public async Task<TagListItemProjection[]> GetListOfTagsAsync(Guid folderId)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            return await client.GetFromResult<TagListItemProjection[]>($"Folders/{folderId}/tags");
        }

        public async Task<BreadcrumbProjection[]> GetBreadcrumbsAsync(Guid folderId)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            return await client.GetFromResult<BreadcrumbProjection[]>($"Folders/{folderId}/breadcrumbs");
        }

        public async Task CreateAsync(FolderProjection projection)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            using (var content = new StringContent(projection.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                //await client.PostAsync("Folders", content);
                await client.PostFromResult("Folders", content);
            }
        }

        public async Task UpdateName(Guid folderId, string name)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            await client.PatchAsync($"Folders/{folderId}/name/{name}", null);
        }

        public async Task MoveUp(Guid folderId)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            await client.PatchAsync($"Folders/{folderId}/moveup", null);
        }

        public async Task MoveDown(Guid folderId)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            await client.PatchAsync($"Folders/{folderId}/movedown", null);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            await client.DeleteAsync($"Folders/{id}");
        }
    }
}
