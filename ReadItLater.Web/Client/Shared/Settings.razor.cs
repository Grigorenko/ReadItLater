using Microsoft.AspNetCore.Components;
using ReadItLater.Web.Client.Services.Auth;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class Settings
    {
        [Inject]
        public CustomAuthStateProvider stateProvider { get; set; }

        [Parameter]
        public EventCallback FolderEventCallback { get; set; }

        public async Task Logout()
        {
            await stateProvider.Logout();
        }
    }
}
