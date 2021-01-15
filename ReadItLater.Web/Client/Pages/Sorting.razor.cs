using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Pages
{
    public partial class Sorting
    {
        private string orderBy;
        private string direction;
        private IDictionary<string, string> selected;

        private IEnumerable<string> orderingItemNames = new[]
        {
            "Date",
            "Priority"
        };

        protected override void OnInitialized()
        {
            orderBy = orderingItemNames.First();
            direction = Direction.Ascending.ToString();
            selected = new Dictionary<string, string>();
        }

        public void DeleteBadgeCallbackHandler(string key)
        {
            if (!selected.ContainsKey(key))
                throw new ArgumentException(nameof(key));

            selected.Remove(key);
        }

        public void AddBadge()
        {
            if (string.IsNullOrEmpty(orderBy) || string.IsNullOrEmpty(direction))
                return;

            //ToDo: add validation above

            if (selected.ContainsKey(orderBy))
                throw new ArgumentException(nameof(orderBy));

            selected.Add(orderBy, direction);

            orderBy = null;
        }
    }

    public enum Direction
    {
        Ascending,
        Descending
    }
}
