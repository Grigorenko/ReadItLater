using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Shared
{
    public partial class Breadcrumbs
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public Context Context { get; set; }

        [Parameter]
        public Guid? FolderId { get; set; }

        private BreadcrumbProjection[] breadcrumbs;

        public async Task FolderChangedEventHandler(Guid folderId, Guid? tagId)
        {
            breadcrumbs = await Http.GetFromJsonAsync<BreadcrumbProjection[]>($"Folders/{folderId}/breadcrumbs");

            StateHasChanged();
        }

        private void FolderChosen(Guid id)
        {
            Context.FolderChosen(id);
        }
    }
}
