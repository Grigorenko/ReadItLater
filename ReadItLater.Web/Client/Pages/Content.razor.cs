using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Content : IDisposable
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public Context AppState { get; set; }

        private Ref[] refs;

        protected override async Task OnInitializedAsync()
        {
            var logMsg = $"{nameof(Content)}.{nameof(OnInitializedAsync)}";
            Console.WriteLine(logMsg);

            await UpdateRefs(null, null, false);

            AppState.DataChanged += async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            AppState.FolderChanged += async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            AppState.TagChanged += async (folderId, tagId) => await TagChangedEventHandler(folderId, tagId);

            AppState.WriteStatusLog(logMsg);
        }

        private async Task DataChangedEventHandler(Guid? folderId, Guid? tagId)
        {
            var logMsg = $"{nameof(Content)}.{nameof(DataChangedEventHandler)}(folderId:{folderId}, tagId:{tagId})";
            Console.WriteLine(logMsg);

            await UpdateRefs(folderId, tagId);

            AppState.WriteStatusLog(logMsg);
        }

        private async Task FolderChangedEventHandler(Guid folderId, Guid? tagId)
        {
            var logMsg = $"{nameof(Content)}.{nameof(FolderChangedEventHandler)}(folderId:{folderId}, tagId:{tagId})";
            Console.WriteLine(logMsg);

            await UpdateRefs(folderId, tagId);

            AppState.WriteStatusLog(logMsg);
        }

        private async Task TagChangedEventHandler(Guid folderId, Guid? tagId)
        {
            var logMsg = $"{nameof(Content)}.{nameof(TagChangedEventHandler)}(folderId:{folderId}, tagId:{tagId})";
            Console.WriteLine(logMsg);

            await UpdateRefs(folderId, tagId);

            AppState.WriteStatusLog(logMsg);
        }

        private async Task UpdateRefs(Guid? folderId, Guid? tagId, bool stateHasChanged = true)
        {
            //ToDo: virtualize?
        //https://docs.microsoft.com/ru-ru/aspnet/core/blazor/webassembly-performance-best-practices?view=aspnetcore-5.0#virtualization
            var url = BuildUrl(folderId, tagId);
            //Console.WriteLine(url);
            refs = await Http.GetFromJsonAsync<Ref[]>(url);

            if (stateHasChanged)
                StateHasChanged();
        }

        private string BuildUrl(Guid? folderId, Guid? tagId, int offset = 0, int limit = 25, string orderBy = "", string direction = "")
        {
            var parameters = new Dictionary<string, string>
            {
                ["folderId"] = folderId?.ToString(),
                ["tagId"] = tagId?.ToString(),
                [nameof(offset)] = offset.ToString(),
                [nameof(limit)] = limit.ToString(),
                [nameof(orderBy)] = orderBy,
                [nameof(direction)] = direction
            };

            var builder = new UriBuilder("","")
            {
                Path = "refs",
                Query = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))
            };

            return builder.ToString().TrimStart('/');
        }

        public void Dispose()
        {
            AppState.DataChanged -= async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            AppState.FolderChanged -= async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            AppState.TagChanged -= async (folderId, tagId) => await TagChangedEventHandler(folderId, tagId);
        }
    }
}
