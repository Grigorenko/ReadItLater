using ReadItLater.Web.Client.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadItLater.Web.Client.Services
{
    public class Context
    {
        public StateType Type => state.Type;
        private State state;
        private Guid? folderId;
        private Guid? tagId;

        public Context()
        {
            folderChangedEventHandlers = new List<IFolderChanged>();
            tagChangedEventHandlers = new List<ITagChanged>();
            sortingChangedEventHandlers = new List<ISortingChanged>();
            stateChangedEventHandlers = new List<IStateChanged>();
            dataChangedEventHandlers = new List<IDataChanged>();
            refAddedEventHandlers = new List<IRefAdded>();
            refEditedEventHandlers = new List<IRefEdited>();
            sortingChosenEventHandlers = new List<ISortingChosen>();
            subMenuClosedEventHandlers = new List<ISubMenuClosed>();
            ChangeState(new CloseState());
        }

        private List<IStateChanged> stateChangedEventHandlers;
        private List<IFolderChanged> folderChangedEventHandlers;
        private List<ITagChanged> tagChangedEventHandlers;
        private List<ISortingChanged> sortingChangedEventHandlers;
        private List<IDataChanged> dataChangedEventHandlers;
        private List<IRefAdded> refAddedEventHandlers;
        private List<IRefEdited> refEditedEventHandlers;
        private List<ISortingChosen> sortingChosenEventHandlers;
        private List<ISubMenuClosed> subMenuClosedEventHandlers;

        public void Subscribe(object component)
        {
            var interfaces = component
                .GetType()
                .GetInterfaces()
                .Where(i => i.GetInterfaces().Any(p => p == typeof(IContext)))
                .ToList();

            var handlerCollections = new Dictionary<Type, Action>
            {
                { typeof(IStateChanged), () => stateChangedEventHandlers.Add((IStateChanged)component) },
                { typeof(IFolderChanged), () => folderChangedEventHandlers.Add((IFolderChanged)component) },
                { typeof(ITagChanged), () => tagChangedEventHandlers.Add((ITagChanged)component) },
                { typeof(ISortingChanged), () => sortingChangedEventHandlers.Add((ISortingChanged)component) },
                { typeof(IDataChanged), () => dataChangedEventHandlers.Add((IDataChanged)component) },
                { typeof(IRefAdded), () => refAddedEventHandlers.Add((IRefAdded)component) },
                { typeof(IRefEdited), () => refEditedEventHandlers.Add((IRefEdited)component) },
                { typeof(ISortingChosen), () => sortingChosenEventHandlers.Add((ISortingChosen)component) },
                { typeof(ISubMenuClosed), () => subMenuClosedEventHandlers.Add((ISubMenuClosed)component) }
            };

            interfaces.ForEach(i => handlerCollections[i]());
        }

        public void Unsubscribe(object component)
        {
            var interfaces = component
                .GetType()
                .GetInterfaces()
                .Where(i => i.GetInterfaces().Any(p => p == typeof(IContext)))
                .ToList();

            var handlerCollections = new Dictionary<Type, Action>
            {
                { typeof(IStateChanged), () => stateChangedEventHandlers.Remove((IStateChanged)component) },
                { typeof(IFolderChanged), () => folderChangedEventHandlers.Remove((IFolderChanged)component) },
                { typeof(ITagChanged), () => tagChangedEventHandlers.Remove((ITagChanged)component) },
                { typeof(ISortingChanged), () => sortingChangedEventHandlers.Remove((ISortingChanged)component) },
                { typeof(IDataChanged), () => dataChangedEventHandlers.Remove((IDataChanged)component) },
                { typeof(IRefAdded), () => refAddedEventHandlers.Remove((IRefAdded)component) },
                { typeof(IRefEdited), () => refEditedEventHandlers.Remove((IRefEdited)component) },
                { typeof(ISortingChosen), () => sortingChosenEventHandlers.Remove((ISortingChosen)component) },
                { typeof(ISubMenuClosed), () => subMenuClosedEventHandlers.Remove((ISubMenuClosed)component) }
            };

            interfaces.ForEach(i => handlerCollections[i]());
        }

        public void FolderChosen(Guid folderId)
        {
            Show(folderId: folderId);
            state.Show(this.folderId, tagId);
            folderChangedEventHandlers.ForEach(x => x.Handle(this.folderId.Value, this.tagId));
        }

        public void TagChosen(Guid? tagId)
        {
            Show(tagId: tagId);
            state.Show(this.folderId, this.tagId);
            tagChangedEventHandlers.ForEach(x => x.Handle(this.folderId.Value, this.tagId));
        }

        public void ContentChanged()
        {
            dataChangedEventHandlers.ForEach(x => x.Handle(folderId, tagId));
        }

        public void RefAdding()
        {
            state.AddNew();
            refAddedEventHandlers.ForEach(x => x.Handle());
        }

        public void Close()
        {
            if (folderId.HasValue)
            {
                state.Show(folderId, tagId); 
                folderChangedEventHandlers.ForEach(x => x.Handle(folderId.Value, tagId));
            }
            else
            {
                state.Close();
                subMenuClosedEventHandlers.ForEach(x => x.Handle());
            }
        }

        public void RefEditing(Guid refId)
        {
            state.Edit(refId);
            refEditedEventHandlers.ForEach(x => x.Handle(refId));
        }

        public void ApplySorting(Badge[] badges)
        {
            state.Close();
            sortingChangedEventHandlers.ForEach(x => x.Handle(badges));
        }

        public void ChooseSorting(Badge[] badges)
        {
            state.Sorting(badges);
            sortingChosenEventHandlers.ForEach(x => x.Handle(badges));
        }

        public override string ToString()
        {
            return new StringBuilder(nameof(Context) + ": ")
                .Append("{ ")
                .Append($"{nameof(Type)}: {Type}, ")
                .Append($"{nameof(folderId)}: {folderId}, ")
                .Append($"{nameof(tagId)}: {tagId}")
                .Append(" }")
                .ToString();
        }

        public void WriteStatusLog(string prefix) => Console.WriteLine(prefix + ": " + ToString());

        internal void ChangeState(State state)
        {
            Console.WriteLine($"State changed: {(this.state?.Type.ToString() ?? "empty")} => {state.Type}");
            this.state = state;
            this.state.SetContext(this);
            stateChangedEventHandlers.ForEach(x => x.Handle());
        }

        private void Show(Guid? folderId = null, Guid? tagId = null)
        {
            if (folderId is null && this.folderId is null)
                throw new ArgumentNullException();

            if (folderId != null)
                this.folderId = folderId;

            this.tagId = tagId;
        }
    }
}
