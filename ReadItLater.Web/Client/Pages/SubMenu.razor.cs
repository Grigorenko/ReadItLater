﻿using Microsoft.AspNetCore.Components;
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
        public Context Context { get; set; }

        [Parameter]
        public FolderListItemProjection[] folders { get; set; }

        [Parameter]
        public int refsAllCount { get; set; }

        private Shared.Breadcrumbs breadcrumbsComponent;
        private AddNewRef addNewRefComponent;
        private TagListItemProjection[] tags;
        private Guid? folderId;
        private Guid? tagId;

        protected override void OnInitialized()
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(OnInitialized)}";
            Console.WriteLine(logMsg);

            //Context.FolderChanged += async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            //Context.DataChanged += async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            //Context.TagChanged += (_, _) => StateHasChanged();
            //Context.RefAdded += () => StateHasChanged();
            //Context.Closed += () => StateHasChanged();
            //Context.RefEdited += async id => await RefEditedEventHandler(id);// { await addNewRefComponent.RefEditedEventHandler(id); StateHasChanged(); };
            folderId = null;
            tagId = null;

            Context.WriteStatusLog(logMsg);
        }

        private async Task RefEditedEventHandler(Guid id)
        {
            var logMsg = $"{nameof(SubMenu)}.{nameof(RefEditedEventHandler)}(id:{id})";
            Console.WriteLine(logMsg);

            StateHasChanged();
            if (addNewRefComponent is null)
                throw new NullReferenceException($"{nameof(addNewRefComponent)} is null.");

                await addNewRefComponent.RefEditedEventHandler(id);
            //await InvokeAsync(async () =>
            //{
            //    await addNewRefComponent.RefEditedEventHandler(id);
            //    StateHasChanged();
            //});

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

        public void Dispose()
        {
            //Context.FolderChanged -= async (folderId, tagId) => await FolderChangedEventHandler(folderId, tagId);
            //Context.DataChanged -= async (folderId, tagId) => await DataChangedEventHandler(folderId, tagId);
            //Context.TagChanged -= (_, _) => StateHasChanged();
            //Context.RefAdded -= () => StateHasChanged();
            //Context.Closed -= () => StateHasChanged();
            //Context.RefEdited -= async id => await RefEditedEventHandler(id);
        }
    }
}
