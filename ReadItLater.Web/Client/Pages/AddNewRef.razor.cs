using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace ReadItLater.Web.Client.Pages
{
    public partial class AddNewRef : IDisposable
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public Context Context { get; set; }

        [Parameter]
        public FolderListItemProjection[] folders { get; set; }

        private string url;
        private RefProjection model;
        private string tag;
        private bool isLoading;

        protected override void OnInitialized()
        {
            var logMsg = $"{nameof(AddNewRef)}.{nameof(OnInitialized)}";
            Console.WriteLine(logMsg);

            isLoading = false;
            model = new RefProjection();
            Context.RefEdited += async id => await RefEditedEventHandler(id);
            Context.StateChanged += () => StateChangedEventHandler();

            Context.WriteStatusLog(logMsg);
        }

        public async Task RefEditedEventHandler(Guid id)
        {
            var logMsg = $"{nameof(AddNewRef)}.{nameof(RefEditedEventHandler)}(id:{id})";
            Console.WriteLine(logMsg);

            if (Context.Type != StateType.Edit)
                throw new Exception(logMsg);

            var res = await Http.GetFromJsonAsync<RefProjection>($"refs/{id}");

            Console.WriteLine(JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true }));
            model = res;
            model.IsDefault = false;

            //Console.WriteLine(JsonSerializer.Serialize(model));

            StateHasChanged();


            Context.WriteStatusLog(logMsg);
        }

        private void StateChangedEventHandler()
        {
            model = new RefProjection();
            StateHasChanged();
        }

        private async Task Add()
        {
            if (isLoading)
                return;

            var logMsg = $"{nameof(AddNewRef)}.{nameof(Add)}";
            Console.WriteLine(logMsg);

            isLoading = true;
            model = await Http.GetFromJsonAsync<RefProjection>("get-ref?url=" + HttpUtility.UrlEncode(url));
            model.IsDefault = false;

            if (model.Tags != null)
                tag = string.Join(", ", model.Tags.Select(t => t.Name));

            isLoading = false;

            Context.WriteStatusLog(logMsg);
        }

        private async Task Create()
        {
            if (isLoading)
                return;

            //ToDo: add validation

            var logMsg = $"{nameof(AddNewRef)}.{nameof(Create)}";
            Console.WriteLine(logMsg);

            isLoading = true;
            AddTag();
            var json = JsonSerializer.Serialize(model);

            using (var content = new StringContent(model.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await Http.PostAsync("refs", content);
            }

            isLoading = false;

            CloseForm(contentChanged: true);

            Context.WriteStatusLog(logMsg);
        }

        private async Task Update()
        {
            if (isLoading)
                return;

            //ToDo: add validation

            var logMsg = $"{nameof(AddNewRef)}.{nameof(Update)}";
            Console.WriteLine(logMsg);

            isLoading = true;
            AddTag();
            var json = JsonSerializer.Serialize(model);
            Console.WriteLine(json);

            using (var content = new StringContent(model.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await Http.PutAsync("refs", content);
            }

            isLoading = false;

            CloseForm(contentChanged: true);

            Context.WriteStatusLog(logMsg);
        }

        private async Task Delete()
        {
            if (isLoading)
                return;

            var logMsg = $"{nameof(AddNewRef)}.{nameof(Delete)}";
            Console.WriteLine(logMsg);

            isLoading = true;

            await Http.DeleteAsync($"refs/{model.Id}");

            isLoading = false;

            CloseForm(contentChanged: true);

            Context.WriteStatusLog(logMsg);
        }

        private void CloseForm(bool contentChanged = false)
        {
            var logMsg = $"{nameof(AddNewRef)}.{nameof(CloseForm)}";
            Console.WriteLine(logMsg);

            Context.Close();

            model = new RefProjection();

            if (contentChanged)
                Context.ContentChanged();

            Context.WriteStatusLog(logMsg);
        }

        private void AddTag()
        {
            var logMsg = $"{nameof(AddNewRef)}.{nameof(AddTag)}";
            Console.WriteLine(logMsg);

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

            Context.WriteStatusLog(logMsg);
        }

        private void DeleteTag(string name)
        {
            var logMsg = $"{nameof(AddNewRef)}.{nameof(DeleteTag)}(name: {name})";
            Console.WriteLine(logMsg);

            if (string.IsNullOrEmpty(name))
                return;

            if (model.Tags is null)
                return;

            if (model.Tags.Any(x => x.Name.Equals(name)))
                model.Tags = model.Tags.Where(t => !t.Name.Equals(name)).ToList();

            Context.WriteStatusLog(logMsg);
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
            Context.RefEdited -= async id => await RefEditedEventHandler(id);
            Context.StateChanged -= () => StateChangedEventHandler();
        }
    }
}
