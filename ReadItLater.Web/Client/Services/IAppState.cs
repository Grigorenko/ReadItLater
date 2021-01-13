using System;

namespace ReadItLater.Web.Client.Services
{
    public interface IAppState
    {
        //Guid? FolderId { get; }
        //Guid? TagId { get; }
        //bool IsRefAdding { get; }
        //bool IsRefEditing { get; }
        //Guid EditingRefId { get; }
        State State { get; }
        StateType Type { get; }

        event Action<Guid> FolderChanged;
        event Action<Guid?> TagChanged;
        event Action DataChanged;
        event Action StartRefAdded;
        event Action EndRefAdded;
        event Action<Guid> StartRefEdited;
        event Action<Guid> EndRefEdited;

        void FolderChosen(Guid folderId);
        void TagChosen(Guid? tagId);
        void ContentChanged();

        void StartRefAdding();
        void EndRefAdding();

        void StartRefEditing(Guid refId);
        void EndRefEditing(Guid refId);

        string ToString();
        void WriteStatusLog();
    }
}