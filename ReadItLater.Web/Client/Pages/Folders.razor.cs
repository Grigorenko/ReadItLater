using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System.Linq;
using ReadItLater.Web.Client.Services.Http;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Folders : IDisposable,
        IContext,
        IDataChanged
    {
        [Inject]
        public FolderHttpService httpService { get; set; }

        [Inject]
        public Context Context { get; set; }

        [Parameter]
        public bool IsFolderEditing { get; set; }

        private FolderListItemProjection[] folders;

        protected override async Task OnInitializedAsync()
        {
            folders = await GetFoldersAsync();
            Context.Subscribe(this);
        }

        async Task IDataChanged.Handle(Guid? folderId, Guid? tagId)
        {
            await DataChanged();
        }

        private void FolderChosenCallbackHandler(Guid folderId)
        {
            if (IsFolderEditing)
                return;

            var refsAllCount = folders.Concat(folders.SelectMany(f => f.Folders)).Single(f => f.Id == folderId).RefsCount;

            if (refsAllCount == 0)
                return;

            Context.FolderChosen(folderId);
        }

        private async Task DataChanged()
        {
            folders = await GetFoldersAsync();

            StateHasChanged();
        }

        private async Task<FolderListItemProjection[]> GetFoldersAsync()
        {
            var folders = await httpService.GetListAsync();

            if (!folders?.Any() ?? true)
                return folders;

            foreach (var item in folders)
                if (item.Folders?.Any() ?? false)
                {
                    item.Folders.First().IsArrowUpActive = false;
                    item.Folders.Last().IsArrowDownActive = false;
                }

            folders.First().IsArrowUpActive = false;
            folders.Last().IsArrowDownActive = false;

            return folders;
        }

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }
}
