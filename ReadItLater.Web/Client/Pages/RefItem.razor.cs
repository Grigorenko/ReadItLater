using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using System;

namespace ReadItLater.Web.Client.Pages
{
    public partial class RefItem
    {
        [Parameter]
        public Ref Item { get; set; }

        [Inject]
        public AppState AppState { get; set; }

        //private bool isLoading;

        //protected override void OnInitialized()
        //{
        //    Console.WriteLine($"{nameof(RefItem)}.{nameof(OnInitialized)}");

        //    isLoading = false;

        //    AppState.WriteStatusLog();
        //}

        private void ChooseTag(Guid tagId)
        {
            var logMsg = $"{nameof(RefItem)}.{nameof(ChooseTag)}(tagId:{tagId})";
            Console.WriteLine(logMsg);

            AppState.TagChosen(tagId);

            AppState.WriteStatusLog(logMsg);
        }

        private void EditRef()
        {
            //if (isLoading)
            //    return;

            var logMsg = $"{nameof(RefItem)}.{nameof(EditRef)}";
            Console.WriteLine(logMsg);

            //isLoading = true;
            AppState.RefEditing(Item.Id);
            //isLoading = false;

            AppState.WriteStatusLog(logMsg);
        }
    }
}
