using ReadItLater.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services.Http
{
    public class TagHttpService
    {
        private readonly HttpClient client;

        public TagHttpService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<TagProjection[]> AutofillAsync(string key)
        {
            return await client.GetFromResult<TagProjection[]>($"tags/autofill?key=" + key);
        }
    }
}
