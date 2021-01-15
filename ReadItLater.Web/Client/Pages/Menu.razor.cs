using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using ReadItLater.Data;
using System.Net.Http;
using System.Net.Http.Json;
using ReadItLater.Web.Client.Services;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Menu : IDisposable
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public Context AppState { get; set; }

        private FolderListItemProjection[] folders;
        private int refsAllCount;

        protected override async Task OnInitializedAsync()
        {
            var logMsg = $"{nameof(Menu)}.{nameof(OnInitializedAsync)}";
            Console.WriteLine(logMsg);

            folders = await Http.GetFromJsonAsync<FolderListItemProjection[]>("Folders/list");
            //AppState.DataChanged += async () => await UpdateFolders();

            AppState.WriteStatusLog(logMsg);
        }

        //private async Task UpdateFolders()
        //{
        //    Console.WriteLine($"{nameof(Menu)}.{nameof(UpdateFolders)}");

        //    folders = await Http.GetFromJsonAsync<FolderListItemProjection[]>("Folders/list");
        //    StateHasChanged();

        //    AppState.WriteStatusLog();
        //}

        public void FolderChosen(Guid folderId)
        {
            var logMsg = $"{nameof(Menu)}.{nameof(FolderChosen)}(folderId:{folderId})";
            Console.WriteLine(logMsg);

            refsAllCount = folders.Concat(folders.SelectMany(f => f.Folders)).Single(f => f.Id == folderId).RefsCount;

            if (refsAllCount == 0)
            {
                Console.WriteLine($"Current folder doesn't exist any refs.");
                return;
            }

            //AppState.EndRefAdding();
            AppState.FolderChosen(folderId);

            AppState.WriteStatusLog(logMsg);
        }

        private void AddNew()
        {
            var logMsg = $"{nameof(Menu)}.{nameof(AddNew)}";
            Console.WriteLine(logMsg);

            //AppState.StartRefAdding();
            AppState.RefAdding();

            AppState.WriteStatusLog(logMsg);
        }

        public void Dispose()
        {
            //AppState.DataChanged -= async () => await UpdateFolders();
        }
    }
}
