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
            var logMsg = $"{nameof(Breadcrumbs)}.{nameof(FolderChangedEventHandler)}(folderId:{folderId}, tagId:{tagId})";
            Console.WriteLine(logMsg);

            breadcrumbs = await Http.GetFromJsonAsync<BreadcrumbProjection[]>($"Folders/{folderId}/breadcrumbs");

            StateHasChanged();

            Context.WriteStatusLog(logMsg);
        }

        private void FolderChosen(Guid id)
        {
            var logMsg = $"{nameof(Breadcrumbs)}:{nameof(FolderChosen)}(id:{id})";
            Console.WriteLine(logMsg);

            Context.FolderChosen(id);

            Context.WriteStatusLog(logMsg);
        }
    }
}
