using ReadItLater.Data.Dtos;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services.Http
{
    public class AuthHttpService
    {
        private readonly HttpClient client;

        public AuthHttpService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<CurrentUser?> CurrentUserInfoAsync()
        {
            return await client.GetFromJsonAsync<CurrentUser?>("auth/currentuserinfo");
        }

        public async Task<CurrentUser?> LoginAsync(LoginRequest loginRequest)
        {
            var result = await client.PostAsJsonAsync("auth/login", loginRequest);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(await result.Content.ReadAsStringAsync());

            result.EnsureSuccessStatusCode();

            var body = await result.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<Result<CurrentUser>>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (!user?.IsSuccess ?? true)
                throw new Exception("Login failed!");

            return user!.Value;
        }

        public async Task LogoutAsync()
        {
            var result = await client.PostAsync("auth/logout", null);

            result.EnsureSuccessStatusCode();
        }

        public async Task RegisterAsync(RegisterRequest registerRequest)
        {
            var result = await client.PostAsJsonAsync("auth/register", registerRequest);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(await result.Content.ReadAsStringAsync());

            result.EnsureSuccessStatusCode();
        }
    }
}
