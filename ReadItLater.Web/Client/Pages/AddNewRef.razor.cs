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
        public Context Context { get; set; }

        private string url;
        private RefProjection model;
        private string tag;
        private bool isLoading;
        private FolderListItemProjection[] folders;

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

        private void AddTag()
        {
            if (string.IsNullOrEmpty(tag))
            {
                Console.WriteLine("Tag is empty.");
                return;
            }

            if (model.Tags is null)
                model.Tags = new List<Tag>();

            if (!model.Tags.Any(x => x.Name.Equals(tag)))
                model.Tags.Add(new Tag { Name = tag });

            else
                Console.WriteLine("Tag with the same name already added.");

            tag = string.Empty;
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
