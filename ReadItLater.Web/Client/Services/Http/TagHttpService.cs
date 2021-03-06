using ReadItLater.Data;
using ReadItLater.Web.Client.Services.Auth;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services.Http
{
    public class TagHttpService
    {
        private readonly UserToken userToken;
        private readonly HttpClient client;

        public TagHttpService(UserToken userToken, HttpClient client)
        {
            this.userToken = userToken;
            this.client = client;
        }

        public async Task<TagProjection[]> AutofillAsync(string key)
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            return await client.GetFromResult<TagProjection[]>($"tags/autofill?key=" + key);
        }
    }
}
