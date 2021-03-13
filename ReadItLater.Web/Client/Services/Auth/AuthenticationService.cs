using ReadItLater.Data.Dtos;
using ReadItLater.Web.Client.Services.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace ReadItLater.Web.Client.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService localStorage;
        private readonly CustomAuthStateProvider authStateProvider;
        private readonly AuthHttpService api;

        public AuthenticationService(
            ILocalStorageService localStorage,
            CustomAuthStateProvider authStateProvider,
            AuthHttpService api)
        {
            this.localStorage = localStorage;
            this.authStateProvider = authStateProvider;
            this.api = api;
        }

        public async Task<CurrentUser?> LoginAsync(LoginRequest loginRequest)
        {
            var user = await api.LoginAsync(loginRequest);

            if (user != null && !string.IsNullOrEmpty(user.Token))
            {
                await localStorage.SetItemAsync("token", user.Token);

                authStateProvider.NotifyUserLogin(user.Token);
            }

            return user;
        }

        public async Task LogoutAsync()
        {
            await api.LogoutAsync();
            await localStorage.RemoveItemAsync("token");

            authStateProvider.NotifyUserLogout();
        }

        public async Task RegisterAsync(RegisterRequest registerRequest)
        {
            await api.RegisterAsync(registerRequest);
        }
    }
}
