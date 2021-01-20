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
            await UpdateRefs(null, null, stateHasChanged: false);

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

        async Task ISortingChanged.Handle(Guid? folderId, Guid? tagId, Badge[] badges)
        {
            this.badges = badges;

            StateHasChanged();

            await UpdateRefs(folderId, tagId, sort: GetSortParameterString());
        }

        private async Task Seach(string term)
        {
            refs = await httpService.SearchAsync(null, null, term, sort: GetSortParameterString());
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

        private async Task UpdateRefs(Guid? folderId, Guid? tagId, int offset = 0, int limit = 50, string sort = "", bool stateHasChanged = true)
        {
            refs = await httpService.GetAsync(folderId, tagId, offset, limit, sort);

            if (stateHasChanged)
                StateHasChanged();
        }

        private string GetSortParameterString() => 
            badges is null 
                ? string.Empty 
                : string.Join(",",
                    badges
                        .OrderBy(b => b.Position)
                        .Select(b => string.Equals(b.Value, "descending", StringComparison.OrdinalIgnoreCase)
                            ? $"-{b.Key}"
                            : $"+{b.Key}")
                );

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }
}
