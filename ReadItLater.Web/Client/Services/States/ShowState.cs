using System;

namespace ReadItLater.Web.Client.Services
{
    public class ShowState : State
    {
        public ShowState()
        {
            Type = StateType.Show;
        }

        public override void AddNew()
        {
            Console.WriteLine($"{nameof(ShowState)}.{nameof(AddNew)} => {nameof(AddNewState)}");
            context.ChangeState(new AddNewState());
        }

        public override void Close()
        {
            Console.WriteLine($"{nameof(ShowState)}.{nameof(Close)} => {nameof(CloseState)}");
            context.ChangeState(new CloseState());
        }

        public override void Edit(Guid refId)
        {
            Console.WriteLine($"{nameof(ShowState)}.{nameof(Edit)} => {nameof(EditState)}");
            context.ChangeState(new EditState());
        }

        public override void Show(Guid? folderId = null, Guid? tagId = null)
        {
            Console.WriteLine($"{nameof(ShowState)}.{nameof(Show)} => nothing");
            // nothing
        }
    }
}
