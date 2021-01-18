using ReadItLater.Web.Client.Services.Models;
using System;

namespace ReadItLater.Web.Client.Services
{
    public class SortingState : State
    {
        public SortingState()
        {
            Type = StateType.Sorting;
        }

        public override void AddNew()
        {
            Console.WriteLine($"{nameof(SortingState)}.{nameof(AddNew)} => {nameof(AddNewState)}");
            context.ChangeState(new AddNewState());
        }

        public override void Close()
        {
            Console.WriteLine($"{nameof(SortingState)}.{nameof(Close)} => {nameof(CloseState)}");
            context.ChangeState(new CloseState());
        }

        public override void Edit(Guid refId)
        {
            Console.WriteLine($"{nameof(SortingState)}.{nameof(Edit)} => {nameof(EditState)}");
            context.ChangeState(new EditState());
        }

        public override void Show(Guid? folderId = null, Guid? tagId = null)
        {
            Console.WriteLine($"{nameof(SortingState)}.{nameof(Show)} => {nameof(ShowState)}");
            context.ChangeState(new ShowState());
        }

        public override void Sorting(Badge[] badges)
        {
            Console.WriteLine($"{nameof(SortingState)}.{nameof(Sorting)} => {nameof(CloseState)}");
            context.ChangeState(new CloseState());
        }
    }
}
