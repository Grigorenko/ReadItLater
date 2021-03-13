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
        private CustomAuthStateProvider StateProvider { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private LoginRequest loginRequest;

        protected override void OnInitialized()
        {
            loginRequest = new LoginRequest { UserName = "iam", RememberMe = true };

            StateProvider.AuthenticationStateChanged += _ => AuthenticationStateChangedHandler();
        }

        private void AuthenticationStateChangedHandler()
        {
            StateHasChanged();
            Navigation.NavigateTo("/");
        }

        private async Task HandleValidSubmit()
        {
            await AuthenticationService.LoginAsync(loginRequest);
        }

        private void RedirectToRegistration()
        {
            Navigation.NavigateTo($"authentication/registration");
        }

        public void Dispose()
        {
            StateProvider.AuthenticationStateChanged -= _ => AuthenticationStateChangedHandler();
        }
    }
}
