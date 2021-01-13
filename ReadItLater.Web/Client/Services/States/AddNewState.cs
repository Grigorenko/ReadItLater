using System;

namespace ReadItLater.Web.Client.Services
{
    public class AddNewState : State
    {
        public AddNewState()
        {
            Type = StateType.Add;
        }

        public override void AddNew()
        {
            Console.WriteLine($"{nameof(AddNewState)}.{nameof(AddNew)} => {nameof(CloseState)}");
            context.ChangeState(new CloseState());
        }

        public override void Edit(Guid refId)
        {
            Console.WriteLine($"{nameof(AddNewState)}.{nameof(Edit)} => {nameof(EditState)}");
            context.ChangeState(new EditState());
        }

        public override void Close()
        {
            Console.WriteLine($"{nameof(AddNewState)}.{nameof(Close)} => {nameof(CloseState)}");
            context.ChangeState(new CloseState());
        }

        public override void Show(Guid? folderId = null, Guid? tagId = null)
        {
            Console.WriteLine($"{nameof(AddNewState)}.{nameof(Show)} => {nameof(ShowState)}");
            context.ChangeState(new ShowState());
        }
    }
}
