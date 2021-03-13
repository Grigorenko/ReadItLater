using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Blazored.LocalStorage;

namespace ReadItLater.Web.Client.Services.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationState anonymous;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsStringAsync("token");

            if (string.IsNullOrWhiteSpace(token))
                return anonymous;

            return CreateAuthenticationState(token);
        }

        public void NotifyUserLogin(string token)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(CreateAuthenticationState(token)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
        }

        private AuthenticationState CreateAuthenticationState(string token) =>
            new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(GetClaimsFromToken(token), "Bearer")));

        private IEnumerable<Claim> GetClaimsFromToken(string token) => 
            new JwtSecurityTokenHandler()
                .ReadJwtToken(token)
                .Claims;
    }
}
