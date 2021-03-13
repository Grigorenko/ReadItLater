using ReadItLater.Data;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services.Http
{
    public class FolderHttpService
    {
        private readonly HttpClient client;

        public FolderHttpService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<FolderListItemProjection[]> GetListAsync()
        {
            return await client.GetFromResult<FolderListItemProjection[]>($"Folders/list");
        }

        public async Task<TagListItemProjection[]> GetListOfTagsAsync(Guid folderId)
        {
            return await client.GetFromResult<TagListItemProjection[]>($"Folders/{folderId}/tags");
        }

        public async Task<BreadcrumbProjection[]> GetBreadcrumbsAsync(Guid folderId)
        {
            return await client.GetFromResult<BreadcrumbProjection[]>($"Folders/{folderId}/breadcrumbs");
        }

        public async Task CreateAsync(FolderProjection projection)
        {
            using (var content = new StringContent(projection.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                //await client.PostAsync("Folders", content);
                await client.PostFromResult("Folders", content);
            }
        }

        public async Task UpdateName(Guid folderId, string name)
        {
            await client.PatchAsync($"Folders/{folderId}/name/{name}", null);
        }

        public async Task MoveUp(Guid folderId)
        {
            await client.PatchAsync($"Folders/{folderId}/moveup", null);
        }

        public async Task MoveDown(Guid folderId)
        {
            await client.PatchAsync($"Folders/{folderId}/movedown", null);
        }

        public async Task DeleteAsync(Guid id)
        {
            await client.DeleteAsync($"Folders/{id}");
        }
    }
}
