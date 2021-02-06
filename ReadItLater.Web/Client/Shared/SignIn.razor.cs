using Microsoft.AspNetCore.Components;
using ReadItLater.Data.Dtos;
using ReadItLater.Web.Client.Services.Auth;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class SignIn : IDisposable
    {
        [Inject]
        private CustomAuthStateProvider CustomStateProvider { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private LoginRequest loginRequest;

        protected override void OnInitialized()
        {
            loginRequest = new LoginRequest { UserName = "iam", RememberMe = true };

            CustomStateProvider.AuthenticationStateChanged += _ => AuthenticationStateChangedHandler();
        }

        private void AuthenticationStateChangedHandler()
        {
            StateHasChanged();
            Navigation.NavigateTo("/");
        }

        private async Task LoginAction()
        {
            await CustomStateProvider.Login(loginRequest);
        }

        private void RedirectToRegistration()
        {
            Navigation.NavigateTo($"authentication/registration");
        }

        public void Dispose()
        {
            CustomStateProvider.AuthenticationStateChanged -= _ => AuthenticationStateChangedHandler();
        }
    }
}
