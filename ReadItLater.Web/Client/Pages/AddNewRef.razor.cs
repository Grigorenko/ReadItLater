﻿using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ReadItLater.Web.Client.Pages
{
    public partial class AddNewRef : IDisposable
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public AppState AppState { get; set; }

        [Parameter]
        public FolderListItemProjection[] folders { get; set; }

        private string url;
        private RefProjection model;
        private string tag;
        private bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            var logMsg = $"{nameof(AddNewRef)}.{nameof(OnInitializedAsync)}";
            Console.WriteLine(logMsg);

            isLoading = false;
            model = new RefProjection();
            //AppState.StartRefEdited += async id => await EditMode(id);

            //kostil
            //if (AppState.IsRefEditing)
            //    await EditMode(AppState.EditingRefId);

            AppState.WriteStatusLog(logMsg);
        }

        //private async Task EditMode(Guid id)
        //{
        //    Console.WriteLine($"{nameof(AddNewRef)}.{nameof(EditMode)}(id:{id})");

        //    if (AppState.IsRefEditing && AppState.EditingRefId != default)
        //    {
        //        model = await Http.GetFromJsonAsync<RefProjection>($"refs/{id}");
        //        model.IsDefault = false;

        //        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model));

        //        //StateHasChanged();
        //    }

        //    AppState.WriteStatusLog();
        //}

        private async Task Add()//mode
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

            AppState.WriteStatusLog(logMsg);
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
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            using (var content = new StringContent(model.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await Http.PostAsync("refs", content);
            }

            isLoading = false;

            CloseForm(contentChanged: true);

            AppState.WriteStatusLog(logMsg);
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
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            Console.WriteLine(json);

            using (var content = new StringContent(model.ToString(), Encoding.UTF8, MediaTypeNames.Application.Json))
            {
                await Http.PutAsync("refs", content);
            }

            isLoading = false;

            CloseForm(contentChanged: true);

            AppState.WriteStatusLog(logMsg);
        }

        private async Task Delete()
        {
            if (isLoading)
                return;

            var logMsg = $"{nameof(AddNewRef)}.{nameof(Delete)}";
            Console.WriteLine(logMsg);

            isLoading = true;

            await Http.DeleteAsync($"refs?id={model.Id}");

            isLoading = false;

            CloseForm(contentChanged: true);

            AppState.WriteStatusLog(logMsg);
        }

        private void CloseForm(bool contentChanged = false)
        {
            var logMsg = $"{nameof(AddNewRef)}.{nameof(CloseForm)}";
            Console.WriteLine(logMsg);

            //    if (AppState.IsRefAdding)
            //    AppState.EndRefAdding();

            //else if (AppState.IsRefEditing)
            //    AppState.EndRefEditing(model.Id);

            AppState.Close();

            model = new RefProjection();

            if (contentChanged)
                AppState.ContentChanged();

            AppState.WriteStatusLog(logMsg);
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

            AppState.WriteStatusLog(logMsg);
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

            AppState.WriteStatusLog(logMsg);
        }

        private RenderFragment Loading() => b =>
        {
            b.OpenElement(1, "i");
            b.AddAttribute(1, "class", "fa fa-spin fa-spinner");
            b.CloseElement();
        };

        public void Dispose()
        {
            //AppState.StartRefEdited -= async id => await EditMode(id);
        }
    }
}