using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class TagsSelector : IDisposable
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public Context Context { get; set; }

        [Parameter]
        public int refsAllCount { get; set; }
        //ToDo: think about getting this value from query for all tags by folder

        private Shared.Breadcrumbs breadcrumbsComponent;
        //private AddNewRef addNewRefComponent;
        private TagListItemProjection[] tags;
        private Guid? folderId;
        private Guid? tagId;

        protected override void OnInitialized()
        {
            var logMsg = $"{nameof(TagsSelector)}.{nameof(OnInitialized)}";
            Console.WriteLine(logMsg);

            Context.FolderChanged += async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            Context.StateChanged += () => StateHasChanged();
            //Context.DataChanged += async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            //Context.TagChanged += (_, _) => StateHasChanged();
            //Context.RefAdded += () => StateHasChanged();
            //Context.Closed += () => StateHasChanged();
            //Context.RefEdited += async id => await RefEditedEventHandler(id);// { await addNewRefComponent.RefEditedEventHandler(id); StateHasChanged(); };
            folderId = null;
            tagId = null;

            Context.WriteStatusLog(logMsg);
        }

        private void TagChosen(Guid? id)
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(TagChosen)}(id:{id})";
            Console.WriteLine(logMsg);

            Context.TagChosen(id);
            this.tagId = id;

            Context.WriteStatusLog(logMsg);
        }

        private async Task FolderChangedEventHandler(Guid folderId, Guid? tagId)
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(FolderChangedEventHandler)}(folderId:{folderId}, tagId:{tagId})";
            Console.WriteLine(logMsg);

            tags = await Http.GetFromJsonAsync<TagListItemProjection[]>($"Folders/{folderId}/tags");
            this.folderId = folderId;
            this.tagId = tagId;

            StateHasChanged();

            if (breadcrumbsComponent is null)
                throw new NullReferenceException($"{nameof(breadcrumbsComponent)} is null.");

            await breadcrumbsComponent?.FolderChangedEventHandler(folderId, tagId);

            Context.WriteStatusLog(logMsg);
        }

        private string GetSelectedTagClassName(Guid? tagId = null) =>
            !this.tagId.HasValue && tagId is null
                ? "selected"
                : this.tagId.HasValue && tagId == this.tagId.Value
                    ? "selected"
                    : string.Empty;
        // @(folderId.HasValue && !tagId.HasValue ? "selected" : "")
        //@(tagId.HasValue && item.Id == tagId.Value ? "selected" : "")

        public void Dispose()
        {
            Context.FolderChanged -= async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            Context.StateChanged -= () => StateHasChanged();
            //Context.DataChanged -= async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            //Context.TagChanged -= (_, _) => StateHasChanged();
            //Context.RefAdded -= () => StateHasChanged();
            //Context.Closed -= () => StateHasChanged();
            //Context.RefEdited -= async id => await RefEditedEventHandler(id);
        }
    }
}
