using Microsoft.AspNetCore.Components;
using ReadItLater.Data;
using ReadItLater.Web.Client.Services;
using ReadItLater.Web.Client.Services.Http;
using ReadItLater.Web.Client.Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Content : IDisposable, 
        IContext,
        IFolderChanged,
        ITagChanged,
        IDataChanged,
        ISortingChanged
    {
        [Inject]
        public RefHttpService httpService { get; set; }

        [Inject]
        public Context Context { get; set; }

        private Ref[] refs;
        private Badge[] badges;

        protected override async Task OnInitializedAsync()
        {
            await UpdateRefs(null, null, false);

            Context.Subscribe(this);
        }

        async Task IDataChanged.Handle(Guid? folderId, Guid? tagId)
        {
            await UpdateRefs(folderId, tagId);
        }

        async Task IFolderChanged.Handle(Guid folderId, Guid? tagId)
        {
            await UpdateRefs(folderId, tagId);
        }

        async Task ITagChanged.Handle(Guid folderId, Guid? tagId)
        {
            await UpdateRefs(folderId, tagId);
        }

        async Task ISortingChanged.Handle(Badge[] badges)
        {
            this.badges = badges;

            StateHasChanged();

            await Task.CompletedTask;
        }

        private void DeleteBadgeCallbackHandler(string key)
        {
            var badge = new Badges(badges);
            badge.Remove(key);

            Context.ApplySorting(badge.Items.ToArray());
        }

        private void ChooseSorting()
        {
            Context.ChooseSorting(badges);
        }

        private async Task UpdateRefs(Guid? folderId, Guid? tagId, bool stateHasChanged = true)
        {
            refs = await httpService.GetAsync(folderId, tagId);

            if (stateHasChanged)
                StateHasChanged();
        }

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }
}
