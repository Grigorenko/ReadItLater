using Microsoft.AspNetCore.Components;
using ReadItLater.Web.Client.Services;
using ReadItLater.Web.Client.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Sorting : IDisposable,
        IContext,
        ISortingChosen,
        IStateChanged
    {
        [Inject]
        public Context Context { get; set; }

        private string orderBy;
        private string direction;
        private Badges badges;

        private IEnumerable<string> orderingItemNames = new[]
        {
            "Date",
            "Priority"
        };

        protected override void OnInitialized()
        {
            orderBy = orderingItemNames.First();
            direction = Direction.Ascending.ToString();
            badges = new Badges();

            Context.Subscribe(this);
        }

        async Task ISortingChosen.Handle(Badge[] badges)
        {
            StateHasChanged();

            await Task.CompletedTask;
        }
        async Task IStateChanged.Handle()
        {
            StateHasChanged();

            await Task.CompletedTask;
        }

        private void DeleteBadgeCallbackHandler(string key)
        {
            badges.Remove(key);

            orderBy = orderingItemNames.FirstOrDefault(i => !badges.Keys.Any(k => string.Equals(k, i)));
        }

        private void AddBadge()
        {
            if (string.IsNullOrEmpty(orderBy) || string.IsNullOrEmpty(direction))
                return;

            //ToDo: add validation above

            badges.Add(orderBy, direction);

            orderBy = orderingItemNames.FirstOrDefault(i => !badges.Keys.Any(k => string.Equals(k, i)));
        }

        private void MoveUp(string key)
        {
            badges.MoveUp(key);
        }

        private void MoveDown(string key)
        {
            badges.MoveDown(key);
        }

        private void ApplySorting()
        {
            Context.ApplySorting(badges.Items.ToArray());
            Context.Close();
        }

        public void Dispose()
        {
            Context.Unsubscribe(this);
        }
    }
}
