using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System.Linq;
using ReadItLater.Web.Client.Services.Http;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Menu : IDisposable,
        IContext,
        IDataChanged
    {
        [Inject]
        public FolderHttpService httpService { get; set; }

        [Inject]
        public Context Context { get; set; }

        private FolderListItemProjection[] folders;
        private int refsAllCount;

        protected override async Task OnInitializedAsync()
        {
            folders = await httpService.GetListAsync();
            Context.Subscribe(this);
        }

        async Task IDataChanged.Handle(Guid? folderId, Guid? tagId)
        {
            folders = await httpService.GetListAsync();
            StateHasChanged();
        }

        public void FolderChosen(Guid folderId)
        {
            refsAllCount = folders.Concat(folders.SelectMany(f => f.Folders)).Single(f => f.Id == folderId).RefsCount;

            if (refsAllCount == 0)
            {
                Console.WriteLine($"Current folder doesn't exist any refs.");
                return;
            }

            Context.FolderChosen(folderId);
        }

        private void AddNew()
        {
            Context.RefAdding();
        }

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }
}
