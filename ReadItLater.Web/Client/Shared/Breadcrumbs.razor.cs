using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class Breadcrumbs
    {
        [Inject]
        public FolderHttpService httpService { get; set; }

        [Inject]
        public Context Context { get; set; }

        [Parameter]
        public Guid? FolderId { get; set; }

        private BreadcrumbProjection[] breadcrumbs;

        public async Task FolderChangedEventHandler(Guid folderId, Guid? tagId)
        {
            breadcrumbs = await httpService.GetBreadcrumbsAsync(folderId);

            StateHasChanged();
        }

        private void FolderChosen(Guid id)
        {
            Context.FolderChosen(id);
        }
    }
}
