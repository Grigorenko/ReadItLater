using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class EmptyFolderItem
    {
        [Inject]
        public FolderHttpService httpService { get; set; }

        [Parameter]
        public bool IsNested { get; set; }

        [Parameter]
        public Guid? ParentId { get; set; }

        [Parameter]
        public EventCallback DataChangedCallback { get; set; }

        private string value;

        private async Task AddFolder()
        {
            if (string.IsNullOrEmpty(value))
                return;

            var folder = new FolderProjection { ParentId = ParentId, Name = value };

            await httpService.CreateAsync(folder);

            value = null;
            
            if (!DataChangedCallback.HasDelegate)
                return;

            await DataChangedCallback.InvokeAsync(folder);
        }
    }
}
