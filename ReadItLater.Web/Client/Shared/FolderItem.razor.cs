using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class FolderItem
    {
        [Inject]
        public FolderHttpService httpService { get; set; }

        [Parameter]
        public FolderListItemProjection Folder { get; set; }

        [Parameter]
        public bool IsFolderEditing { get; set; }

        [Parameter]
        public bool IsNested { get; set; }

        [Parameter]
        public EventCallback<Guid> FolderChosenCallback { get; set; }

        [Parameter]
        public EventCallback DataChangedCallback { get; set; }

        private string count => Folder.RefsCount > 0 ? Folder.RefsCount.ToString() : string.Empty;
        private bool editMode;
        public string value;

        protected override void OnInitialized()
        {
            editMode = false;
        }

        private async Task FolderChosen()
        {
            if (!FolderChosenCallback.HasDelegate)
                return;

            await FolderChosenCallback.InvokeAsync(Folder.Id);
        }

        private void Edit()
        {
            value = Folder.Name;
            editMode = true;
        }

        private async Task FolderEdited()
        {
            if (string.IsNullOrEmpty(value))
                return;

            await httpService.UpdateName(Folder.Id, value);

            editMode = false;

            await DataChangedCallbackInvokeAsync();
        }

        private async Task FolderMoveUp()
        {
            await httpService.MoveUp(Folder.Id);

            await DataChangedCallbackInvokeAsync();
        }

        private async Task FolderMoveDown()
        {
            await httpService.MoveDown(Folder.Id);

            await DataChangedCallbackInvokeAsync();
        }

        private async Task FolderDelete()
        {
            await httpService.DeleteAsync(Folder.Id);

            await DataChangedCallbackInvokeAsync();
        }

        private async Task DataChangedCallbackInvokeAsync()
        {
            if (!DataChangedCallback.HasDelegate)
                return;

            await DataChangedCallback.InvokeAsync(Folder.Id);
        }
    }
}
