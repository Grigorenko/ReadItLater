using ReadItLater.Data.Dtos;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services.Auth
{
    public interface IAuthenticationService
    {
        Task<CurrentUser?> LoginAsync(LoginRequest loginRequest);
        Task LogoutAsync();
        Task RegisterAsync(RegisterRequest registerRequest);
    }
}