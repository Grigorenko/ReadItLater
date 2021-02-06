using Microsoft.AspNetCore.Components;
using ReadItLater.Data.Dtos;
using ReadItLater.Web.Client.Services.Auth;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class SignOut : IDisposable
    {
        [Inject]
        private CustomAuthStateProvider CustomStateProvider { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private RegisterRequest registerRequest;

        protected override void OnInitialized()
        {
            registerRequest = new RegisterRequest { };

            CustomStateProvider.AuthenticationStateChanged += _ => AuthenticationStateChangedHandler();
        }

        private void AuthenticationStateChangedHandler()
        {
            StateHasChanged();
            Navigation.NavigateTo("/");
        }

        private async Task RegistrationAction()
        {
            await CustomStateProvider.Register(registerRequest);
        }

        private void RedirectToLogin()
        {
            Navigation.NavigateTo($"authentication/login");
        }

        public void Dispose()
        {
            CustomStateProvider.AuthenticationStateChanged -= _ => AuthenticationStateChangedHandler();
        }
    }
}
