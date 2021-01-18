using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class TagsSelector : IDisposable,
        IContext,
        IFolderChanged,
        IStateChanged
    {
        [Inject]
        public FolderHttpService httpService { get; set; }

        [Inject]
        public Context Context { get; set; }

        [Parameter]
        public int refsAllCount { get; set; }
        //ToDo: think about getting this value from query for all tags by folder

        private Shared.Breadcrumbs breadcrumbsComponent;
        private TagListItemProjection[] tags;
        private Guid? folderId;
        private Guid? tagId;

        protected override void OnInitialized()
        {
            Context.Subscribe(this);
            folderId = null;
            tagId = null;
        }

        private void TagChosen(Guid? id)
        {
            Context.TagChosen(id);
            this.tagId = id;
        }

        async Task IFolderChanged.Handle(Guid folderId, Guid? tagId)
        {
            tags = await httpService.GetListOfTagsAsync(folderId);
            this.folderId = folderId;
            this.tagId = tagId;

            StateHasChanged();

            if (breadcrumbsComponent is null)
                throw new NullReferenceException($"{nameof(breadcrumbsComponent)} is null.");

            await breadcrumbsComponent?.FolderChangedEventHandler(folderId, tagId);
        }

        async Task IStateChanged.Handle()
        {
            StateHasChanged();

            await Task.CompletedTask;
        }

        private string GetSelectedTagClassName(Guid? tagId = null) =>
            !this.tagId.HasValue && tagId is null
                ? "selected"
                : this.tagId.HasValue && tagId == this.tagId.Value
                    ? "selected"
                    : string.Empty;

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }
}
