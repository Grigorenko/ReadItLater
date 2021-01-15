using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class SortingBadge
    {
        [Parameter]
        public string Key { get; set; }

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> DeleteBadgeCallback { get; set; }

        private async Task DeleteBadge()
        {
            await DeleteBadgeCallback.InvokeAsync(Key);
        }
    }
}
