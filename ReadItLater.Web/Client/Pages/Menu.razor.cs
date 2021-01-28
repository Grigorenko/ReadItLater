using Microsoft.AspNetCore.Components;
using ReadItLater.Web.Client.Services;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Menu
    {
        [Inject]
        public Context Context { get; set; }

        private int refsAllCount;
        private bool isFolderEditing;

        protected override void OnInitialized()
        {
            isFolderEditing = false;
        }

        private void AddNew()
        {
            if (isFolderEditing)
                return;

            Context.RefAdding();
        }

        private async Task FolderEditingCallbackHandler()
        {
            isFolderEditing = !isFolderEditing;

            await Task.CompletedTask;
        }
    }
}
