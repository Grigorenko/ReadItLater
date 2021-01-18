using ReadItLater.Data;
using System;
using System.Net.Http;
using System.Net.Http.Json;
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
            return await client.GetFromJsonAsync<FolderListItemProjection[]>($"Folders/list");
        }

        public async Task<TagListItemProjection[]> GetListOfTagsAsync(Guid folderId)
        {
            return await client.GetFromJsonAsync<TagListItemProjection[]>($"Folders/{folderId}/tags");
        }
    }
}
