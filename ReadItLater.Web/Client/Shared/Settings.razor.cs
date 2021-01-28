using Microsoft.AspNetCore.Components;

namespace ReadItLater.Web.Client.Shared
{
    public partial class Settings
    {
        [Parameter]
        public EventCallback FolderEventCallback { get; set; }
    }
}
