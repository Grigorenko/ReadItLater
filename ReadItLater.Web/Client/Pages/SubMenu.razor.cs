using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class SubMenu : IDisposable
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public AppState Context { get; set; }

        [Parameter]
        public FolderListItemProjection[] folders { get; set; }

        [Parameter]
        public int refsAllCount { get; set; }

        //private BreadcrumbProjection[] breadcrumbs;
        private TagListItemProjection[] tags;
        private Guid? folderId;
        private Guid? tagId;

        protected override void OnInitialized()
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(OnInitialized)}";
            Console.WriteLine(logMsg);

            Context.FolderChanged += async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            Context.DataChanged += async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            Context.TagChanged += (_, _) => StateHasChanged();
            Context.RefAdded += () => StateHasChanged();
            Context.Closed += () => StateHasChanged();
            //AppState.EndRefAdded += () => CloseAddingForm();
            //AppState.StartRefEdited += _ => StateHasChanged(); //id => EditMode(id);
            folderId = null;
            tagId = null;

            Context.WriteStatusLog(logMsg);
        }

        //private void EditMode(Guid id)
        //{
        //    Console.WriteLine($"{nameof(SubMenu)}.{nameof(EditMode)}(id:{id})");

        //    StateHasChanged();

        //    AppState.WriteStatusLog();
        //}

        private async Task FolderChangedEventHandler(Guid folderId, Guid? tagId)
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(FolderChangedEventHandler)}(folderId:{folderId}, tagId:{tagId})";
            Console.WriteLine(logMsg);

            //breadcrumbs = await Http.GetFromJsonAsync<BreadcrumbProjection[]>($"Folders/{folderId}/breadcrumbs");
            tags = await Http.GetFromJsonAsync<TagListItemProjection[]>($"Folders/{folderId}/tags");
            this.folderId = folderId;
            this.tagId = tagId;

            StateHasChanged();

            Context.WriteStatusLog(logMsg);
        }

        private async Task DataChangedEventHandler(Guid? folderId, Guid? tagId)
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(DataChangedEventHandler)}(folderId:{folderId}, tagId:{tagId})";
            Console.WriteLine(logMsg);

            if (folderId.HasValue)
            {
                tags = await Http.GetFromJsonAsync<TagListItemProjection[]>($"Folders/{folderId}/tags");
                this.folderId = folderId;
                this.tagId = tagId;

                StateHasChanged();
            }

            Context.WriteStatusLog(logMsg);
        }

        private void TagChosen(Guid? id)
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(TagChosen)}(id:{id})";
            Console.WriteLine(logMsg);

            Context.TagChosen(id);

            Context.WriteStatusLog(logMsg);
        }

        //private void CloseAddingForm()
        //{
        //    var logMsg = $"{nameof(SubMenu)}.{nameof(CloseAddingForm)}";
        //    Console.WriteLine(logMsg);

        //    //AppState.EndRefAdding();
        //    StateHasChanged();

        //    Context.WriteStatusLog(logMsg);
        //}

        public void Dispose()
        {
            Context.FolderChanged -= async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            Context.DataChanged -= async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            Context.TagChanged -= (_, _) => StateHasChanged();
            Context.RefAdded -= () => StateHasChanged();
            Context.Closed -= () => StateHasChanged();
        }
    }
}
