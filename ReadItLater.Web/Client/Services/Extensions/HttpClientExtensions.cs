using ReadItLater.Web.Client.Services.Http;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public static class HttpClientExtensions
    {
        public static async Task<T?> GetFromResult<T>(this HttpClient client, string url)
            where T : class
        {
            var result = await client.GetFromJsonAsync<Result<T>>(url);

            if (result?.IsSuccess ?? false)
                return result.Value;

            throw new Exception();
        }

        public static async Task PostFromResult(this HttpClient client, string url, HttpContent content)
        {
            var response = await client.PostAsync(url, content);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorResult>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!result?.IsSuccess ?? true)
                throw new Exception();
        }

        public static async Task PutFromResult(this HttpClient client, string url, HttpContent content)
        {
            var response = await client.PutAsync(url, content);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorResult>(body);

            if (!result?.IsSuccess ?? true)
                throw new Exception();
        }

        public static async Task PatchFromResult(this HttpClient client, string url, HttpContent content)
        {
            var response = await client.PatchAsync(url, content);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorResult>(body);

            if (!result?.IsSuccess ?? true)
                throw new Exception();
        }
    }
}
