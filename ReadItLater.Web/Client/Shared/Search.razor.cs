using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class Search
    {
        [Parameter]
        public EventCallback<string> SearchCallback { get; set; }

        private string searchTerm;

        private async Task Searching()
        {
            if (string.IsNullOrEmpty(searchTerm) || searchTerm.Length < 3)
                return;

            await SearchCallback.InvokeAsync(searchTerm);
        }
    }
}
