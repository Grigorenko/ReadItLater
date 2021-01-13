using System;

namespace ReadItLater.Web.Client.Services
{
    public class CloseState : State
    {
        public CloseState()
        {
            Type = StateType.Close;
        }

        public override void AddNew()
        {
            Console.WriteLine($"{nameof(CloseState)}.{nameof(AddNew)} => {nameof(AddNewState)}");
            context.ChangeState(new AddNewState());
        }

        public override void Close()
        {
            Console.WriteLine($"{nameof(CloseState)}.{nameof(Close)} => nothing");
            // nothing
        }

        public override void Edit(Guid refId)
        {
            Console.WriteLine($"{nameof(CloseState)}.{nameof(Edit)} => {nameof(EditState)}");
            context.ChangeState(new EditState());
        }

        public override void Show(Guid? folderId = null, Guid? tagId = null)
        {
            Console.WriteLine($"{nameof(CloseState)}.{nameof(Show)} => {nameof(ShowState)}");
            context.ChangeState(new ShowState());
        }
    }
}
