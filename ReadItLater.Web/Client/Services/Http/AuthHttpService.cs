using ReadItLater.Data.Dtos;
using ReadItLater.Web.Client.Services.Auth;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services.Http
{
    public class AuthHttpService
    {
        private readonly UserToken userToken;
        private readonly HttpClient client;

        public AuthHttpService(UserToken userToken, HttpClient client)
        {
            this.userToken = userToken;
            this.client = client;
        }

        public async Task<CurrentUser> CurrentUserInfo()
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            return await client.GetFromJsonAsync<CurrentUser>("auth/currentuserinfo");
        }

        public async Task Login(LoginRequest loginRequest)
        {
            var result = await client.PostAsJsonAsync("auth/login", loginRequest);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(await result.Content.ReadAsStringAsync());

            result.EnsureSuccessStatusCode();

            var body = await result.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<Result<CurrentUser>>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            if (user.IsSuccess)
                userToken.Token = user.Value.Token;

            else
                throw new Exception("Login failed!");
        }

        public async Task Logout()
        {
            if (!string.IsNullOrEmpty(userToken.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken.Token);

            var result = await client.PostAsync("auth/logout", null);

            result.EnsureSuccessStatusCode();
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            var result = await client.PostAsJsonAsync("auth/register", registerRequest);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception(await result.Content.ReadAsStringAsync());

            result.EnsureSuccessStatusCode();
        }
    }
}
