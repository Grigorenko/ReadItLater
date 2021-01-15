using System;
using System.Text;

namespace ReadItLater.Web.Client.Services
{
    public class Context
    {
        public Context()
        {
            ChangeState(new CloseState());
        }
        public Guid? FolderId { get; set; }
        public Guid? TagId { get; set; }
        public Guid? EditingRefId { get; set; }

        public StateType Type => State.Type;

        public event Action<Guid, Guid?> FolderChanged;

        public void FolderChosen(Guid folderId)
        {
            Show(folderId: folderId);
            State.Show(FolderId, TagId);
            FolderChanged?.Invoke(FolderId.Value, TagId);
        }

        public event Action<Guid, Guid?> TagChanged;

        public void TagChosen(Guid? tagId)
        {
            Show(tagId: tagId);
            State.Show(FolderId, TagId);
            TagChanged?.Invoke(FolderId.Value, TagId);
        }

        private void Show(Guid? folderId = null, Guid? tagId = null)
        {
            if (folderId is null && FolderId is null)
                throw new ArgumentNullException();

            if (folderId != null)
                FolderId = folderId;

            TagId = tagId;
        }

        public event Action<Guid?, Guid?> DataChanged;

        public void ContentChanged()
        {
            DataChanged?.Invoke(FolderId, TagId);
        }

        public event Action RefAdded;

        public void RefAdding()
        {
            State.AddNew();
            RefAdded?.Invoke();
        }

        public event Action Closed;
        public void Close()
        {
            if (FolderId.HasValue)
            {
                State.Show(FolderId, TagId); //??
                FolderChanged?.Invoke(FolderId.Value, TagId);
            }
            else
            {
                State.Close();
                Closed?.Invoke();
            }
        }

        public event Action<Guid> RefEdited;

        public void RefEditing(Guid refId)
        {
            EditingRefId = refId;
            State.Edit(refId);
            RefEdited?.Invoke(EditingRefId.Value);
        }

        public override string ToString()
        {
            return new StringBuilder(nameof(Context) + ": ")
                .Append("{ ")
                .Append($"{nameof(Type)}: {Type}, ")
                .Append($"{nameof(FolderId)}: {FolderId}, ")
                .Append($"{nameof(TagId)}: {TagId}, ")
                .Append($"{nameof(EditingRefId)}: {EditingRefId}")
                .Append(" }")
                .ToString();
        }

        public void WriteStatusLog(string prefix) => Console.WriteLine(prefix + ": " + ToString());


        public event Action StateChanged;
        public State State { get; private set; }
        internal void ChangeState(State state)
        {
            Console.WriteLine($"State changed: {(State?.Type.ToString() ?? "empty")} => {state.Type}");
            State = state;
            State.SetContext(this);
            StateChanged?.Invoke();
        }
    }
}
