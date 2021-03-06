using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net.Http;
using System.Linq;
using Microsoft.AspNetCore.Components.Authorization;
using ReadItLater.Data.Dtos;
using ReadItLater.Web.Client.Services.Http;

namespace ReadItLater.Web.Client.Services.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly UserToken userToken;
        private readonly AuthHttpService api;
        private CurrentUser currentUser;

        public CustomAuthStateProvider(AuthHttpService api, UserToken userToken)
        {
            this.userToken = userToken;
            this.api = api;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            try
            {
                var userInfo = await GetCurrentUser();

                if (userInfo.IsAuthenticated)
                {
                    var claims = currentUser.Claims.Select(c => new Claim(c.Key, c.Value)).ToList();
                    //var claims = new[] { new Claim(ClaimTypes.Name, currentUser.UserName) }
                    //    .Concat(currentUser.Claims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                    this.userToken.Token = userInfo.Token;
                }
            }

            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task Logout()
        {
            await api.Logout();
            userToken.Token = null;
            currentUser = null;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Login(LoginRequest loginParameters)
        {
            await api.Login(loginParameters);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterRequest registerParameters)
        {
            await api.Register(registerParameters);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<CurrentUser> GetCurrentUser()
        {
            if (currentUser != null && currentUser.IsAuthenticated)
                return currentUser;

            currentUser = await api.CurrentUserInfo();

            return currentUser;
        }
    }
}
