﻿using Microsoft.AspNetCore.Components;
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
        public Context AppState { get; set; }

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

        private RenderFragment DotsIcon(int countOfDots = 3) => b =>
        {
            if (countOfDots > 3)
                countOfDots = 3;
            else if (countOfDots < 1)
                countOfDots = 1;

            b.OpenElement(1, "svg");
            b.AddAttribute(1, "version", "1.1");
            b.AddAttribute(1, "viewBox", "0 0 45.583 45.583");
            b.AddAttribute(1, "xml:space", "preserve");
            b.AddAttribute(1, "xmlns:xlink", "http://www.w3.org/1999/xlink");
            b.AddAttribute(1, "xmlns", "http://www.w3.org/2000/svg");
            b.AddAttribute(1, "height", "1em");
            b.OpenElement(2, "g");
            b.OpenElement(3, "g");

            for (int i = 0; i < countOfDots; i++)
            {
                b.OpenElement(4 + i, "path");
                b.AddAttribute(4 + i, "xmlns", "http://www.w3.org/2000/svg");
                //b.AddAttribute(4 + i, "fill", "green");

                if (i == 2)
                    b.AddAttribute(4 + i, "d", "M22.793,12.196c-3.361,0-6.078-2.729-6.078-6.099C16.715,2.73,19.432,0,22.793,0c3.353,0,6.073,2.729,6.073,6.097    C28.866,9.466,26.145,12.196,22.793,12.196z");
                else if (i == 1)
                    b.AddAttribute(4 + i, "d", "M22.794,28.889c-3.361,0-6.079-2.729-6.079-6.099c0-3.366,2.717-6.099,6.078-6.099c3.353,0,6.073,2.732,6.075,6.099    C28.866,26.162,26.144,28.889,22.794,28.889z");
                else if (i == 0)
                    b.AddAttribute(4 + i, "d", "M22.794,45.583c-3.361,0-6.079-2.729-6.079-6.099s2.717-6.098,6.078-6.098c3.353-0.002,6.073,2.729,6.073,6.098    S26.144,45.583,22.794,45.583z");
                b.CloseElement();
            }

            b.CloseElement();
            b.CloseElement();
            b.CloseElement();
        };

    }
}