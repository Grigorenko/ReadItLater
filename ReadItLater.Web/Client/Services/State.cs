using System;

namespace ReadItLater.Web.Client.Services
{
    public abstract class State
    {
        protected AppState context;
        public StateType Type { get; protected set; }

        public void SetContext(AppState appState)
        {
            context = appState;
        }

        public abstract void AddNew();
        public abstract void Edit(Guid refId);
        public abstract void Show(Guid? folderId = null, Guid? tagId = null);
        public abstract void Close();
    }
}
