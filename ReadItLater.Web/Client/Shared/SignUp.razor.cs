using Microsoft.AspNetCore.Components;
using ReadItLater.Data.Dtos;
using ReadItLater.Web.Client.Services.Auth;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class SignUp : IDisposable
    {
        [Inject]
        private CustomAuthStateProvider StateProvider { get; set; }

        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private RegisterRequest registerRequest;

        protected override void OnInitialized()
        {
            registerRequest = new RegisterRequest { };

            StateProvider.AuthenticationStateChanged += _ => AuthenticationStateChangedHandler();
        }

        private void AuthenticationStateChangedHandler()
        {
            StateHasChanged();
            Navigation.NavigateTo("/");
        }

        private async Task HandleValidSubmit()
        {
            await AuthenticationService.RegisterAsync(registerRequest);
        }

        private void RedirectToLogin()
        {
            Navigation.NavigateTo($"authentication/login");
        }

        public void Dispose()
        {
            StateProvider.AuthenticationStateChanged -= _ => AuthenticationStateChangedHandler();
        }
    }
}
