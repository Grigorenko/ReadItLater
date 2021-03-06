using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class AddNewRef : IDisposable,
        IContext,
        IRefEdited,
        IStateChanged
    {
        [Inject]
        public RefHttpService refHttpService { get; set; }

        [Inject]
        public FolderHttpService folderHttpService { get; set; }

        [Inject]
        public TagHttpService tagHttpService { get; set; }

        [Inject]
        public Context Context { get; set; }

        private string url;
        private RefProjection model;
        private string tag;
        private bool isLoading;
        private FolderListItemProjection[] folders;
        private TagProjection[] autofilledTags;

        protected override async Task OnInitializedAsync()
        {
            isLoading = false;
            model = new RefProjection();
            Context.Subscribe(this);
            folders = await folderHttpService.GetListAsync();
        }

        async Task IRefEdited.Handle(Guid id)
        {
            if (Context.Type != StateType.Edit)
                throw new Exception();

            model = await refHttpService.GetByIdAsync(id);
            model.IsDefault = false;

            StateHasChanged();
        }

        async Task IStateChanged.Handle()
        {
            model = new RefProjection();
            StateHasChanged();

            await Task.CompletedTask;
        }

        private async Task Add()
        {
            if (isLoading)
                return;

            isLoading = true;
            model = await refHttpService.GetByUrlAsync(url);
            model.IsDefault = false;

            if (model.Tags != null)
                tag = string.Join(", ", model.Tags.Select(t => t.Name));

            isLoading = false;
        }

        private async Task Create()
        {
            if (isLoading)
                return;

            //ToDo: add validation

            isLoading = true;
            AddTag();
            await refHttpService.CreateAsync(model);

            isLoading = false;

            CloseForm(contentChanged: true);
        }

        private async Task Update()
        {
            if (isLoading)
                return;

            //ToDo: add validation

            isLoading = true;
            AddTag();
            await refHttpService.UpdateAsync(model);

            isLoading = false;

            CloseForm(contentChanged: true);
        }

        private async Task Delete()
        {
            if (isLoading)
                return;

            isLoading = true;

            await refHttpService.DeleteAsync(model.Id);

            isLoading = false;

            CloseForm(contentChanged: true);
        }

        private void CloseForm(bool contentChanged = false)
        {
            Context.Close();

            model = new RefProjection();

            if (contentChanged)
                Context.ContentChanged();
        }

        private void AddTagByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Tag is empty.");
                return;
            }

            if (model.Tags is null)
                model.Tags = new List<TagProjection>();

            if (!model.Tags.Any(x => x.Name.Equals(name)))
                model.Tags.Add(new TagProjection { Name = name });

            else
                Console.WriteLine("Tag with the same name already added.");

            UpdateAutoFilledTags(autofilledTags);

            tag = string.Empty;
        }

        private void AddTag()
        {
            AddTagByName(tag);
        }

        private void DeleteTag(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (model.Tags is null)
                return;

            if (model.Tags.Any(x => x.Name.Equals(name)))
                model.Tags = model.Tags.Where(t => !t.Name.Equals(name)).ToList();
        }

        private async Task AutofillTags(ChangeEventArgs e)
        {
            this.tag = e.Value.ToString();

            if (string.IsNullOrEmpty(tag) || tag.Length < 3)
            {
                autofilledTags = null;
                return;
            }

            var tags = await tagHttpService.AutofillAsync(tag);

            UpdateAutoFilledTags(tags);
        }

        private void UpdateAutoFilledTags(TagProjection[] tags)
        {
            if (model.Tags is null || !model.Tags.Any())
                autofilledTags = tags;

            else
                autofilledTags = tags
                    ?.Where(t => !model.Tags.Any(e => string.Equals(e.Name, t.Name, StringComparison.OrdinalIgnoreCase)))
                    ?.ToArray();
        }

        private void UploadImage()
        {
            StateHasChanged();
        }

        private RenderFragment Loading() => b =>
        {
            b.OpenElement(1, "i");
            b.AddAttribute(1, "class", "fa fa-spin fa-spinner");
            b.CloseElement();
        };

        private Priority[] PriorityOptions => (Priority[])Enum.GetValues(typeof(Priority));

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }
}
