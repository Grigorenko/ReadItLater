﻿using ReadItLater.Web.Client.Services.Models;
using System;

namespace ReadItLater.Web.Client.Services
{
    public class EditState : State
    {
        public EditState()
        {
            Type = StateType.Edit;
        }

        public override void AddNew()
        {
            Console.WriteLine($"{nameof(EditState)}.{nameof(AddNew)} => {nameof(AddNewState)}");
            context.ChangeState(new AddNewState());
        }

        public override void Close()
        {
            Console.WriteLine($"{nameof(EditState)}.{nameof(Close)} => {nameof(CloseState)}");
            context.ChangeState(new CloseState());
        }

        public override void Edit(Guid refId)
        {
            Console.WriteLine($"{nameof(EditState)}.{nameof(Edit)} => {nameof(EditState)}");
            context.ChangeState(new EditState());
        }

        public override void Show(Guid? folderId = null, Guid? tagId = null)
        {
            Console.WriteLine($"{nameof(EditState)}.{nameof(Show)} => {nameof(ShowState)}");
            context.ChangeState(new ShowState());
        }

        public override void Sorting(Badge[] badges)
        {
            Console.WriteLine($"{nameof(EditState)}.{nameof(Sorting)} => {nameof(SortingState)}");
            context.ChangeState(new SortingState());
        }
    }
}
