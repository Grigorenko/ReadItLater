using System;
using System.Text;

namespace ReadItLater.Web.Client.Services
{
    public class AppState // Context
    {
        public AppState()
        {
            ChangeState(new CloseState());
        }
        public Guid? FolderId { get; set; }
        public Guid? TagId { get; set; }
        //public bool IsRefAdding { get; private set; }
        //public bool IsRefEditing { get; private set; }
        public Guid EditingRefId { get; private set; }

        public StateType Type => State.Type;

        public event Action<Guid, Guid?> FolderChanged;

        public void FolderChosen(Guid folderId)
        {
            //State.FolderId = folderId;
            //State.TagId = null;
            Show(folderId: folderId);
            State.Show(FolderId, TagId);
            //Console.WriteLine("input folderId: " + folderId);
            //Console.WriteLine("State.FolderId: " + FolderId);
            FolderChanged?.Invoke(FolderId.Value, TagId);
        }

        public event Action<Guid, Guid?> TagChanged;

        public void TagChosen(Guid? tagId)
        {
            //State.TagId = tagId;
            Show(tagId: tagId);
            State.Show(FolderId, TagId);
            TagChanged?.Invoke(FolderId.Value, TagId);
        }

        private void Show(Guid? folderId = null, Guid? tagId = null)
        {
            //Console.WriteLine($"{nameof(State)}.{nameof(Show)}(folderId:{folderId}, tagId:{tagId})");
            if (folderId is null && FolderId is null)
                throw new ArgumentNullException();

            if (folderId != null)
                FolderId = folderId;
            //Console.WriteLine("State.FolderId = " + FolderId);

            TagId = tagId;
        }

        public event Action<Guid?, Guid?> DataChanged;

        public void ContentChanged()
        {
            DataChanged?.Invoke(FolderId, TagId);
        }

        public event Action RefAdded;
        //public event Action StartRefAdded;
        //public event Action EndRefAdded;

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

        //public void StartRefAdding()
        //{
        //    State.AddNew();
        //    //if (IsRefAdding)
        //    //    return;

        //    //IsRefEditing = false;
        //    //IsRefAdding = true;
        //    StartRefAdded?.Invoke();
        //}

        //public void EndRefAdding()
        //{
        //    //if (!IsRefAdding)
        //    //    return;

        //    //IsRefAdding = false;
        //    EndRefAdded?.Invoke();
        //}

        public event Action<Guid> StartRefEdited;
        public event Action<Guid> EndRefEdited;

        public void StartRefEditing(Guid refId)
        {
            EditingRefId = refId;
            State.Edit(refId);
            //if (IsRefEditing && EditingRefId == refId)
            //    return;

            //IsRefAdding = false;
            //IsRefEditing = true;
            //EditingRefId = refId;
            StartRefEdited?.Invoke(refId);
        }

        public void EndRefEditing(Guid refId)
        {
            //if (!IsRefEditing)
            //    return;

            //IsRefEditing = false;
            //EditingRefId = new Guid();
            EndRefEdited?.Invoke(refId);
        }

        public override string ToString()
        {
            return new StringBuilder(nameof(AppState) + ": ")
                .Append("{ ")
                .Append($"{nameof(Type)}: {Type}, ")
                .Append($"{nameof(FolderId)}: {FolderId}, ")
                .Append($"{nameof(TagId)}: {TagId}, ")
                //.Append($"{nameof(IsRefAdding)}: {IsRefAdding}, ")
                //.Append($"{nameof(IsRefEditing)}: {IsRefEditing}, ")
                //.Append($"{nameof(EditingRefId)}: {EditingRefId}")
                .Append(" }")
                .ToString();
        }

        public void WriteStatusLog(string prefix) => Console.WriteLine(prefix + ": " + ToString());

        public State State { get; private set; }
        internal void ChangeState(State state)
        {
            Console.WriteLine($"State changed: {(State?.Type.ToString() ?? "empty")} => {state.Type}");
            State = state;
            State.SetContext(this);
        }
    }
}
