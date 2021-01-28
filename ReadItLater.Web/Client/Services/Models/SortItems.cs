using System;

namespace ReadItLater.Web.Client.Services.Models
{
    public class SortItems
    {
        private const string name = "Name";
        private const string count = "RefsCount";

        private string sortProperty;
        private Direction direction;

        private Action<string, Direction> sortChangedHandler;

        public SortItems(Action<string, Direction> sortChangedHandler)
        {
            this.sortChangedHandler = sortChangedHandler;
            sortProperty = count;
            direction = Direction.Descending;
        }

        public void SortByNameAsc() => AppleSorting(name, Direction.Ascending);
        public void SortByNameDesc() => AppleSorting(name, Direction.Descending);
        public void SortByCountAsc() => AppleSorting(count, Direction.Ascending);
        public void SortByCountDesc() => AppleSorting(count, Direction.Descending);

        public string SortByNameAscClassName => string.Equals(sortProperty, name) && direction == Direction.Ascending ? "active" : NameAvailable ? string.Empty : "inactive";
        public string SortByNameDescClassName => string.Equals(sortProperty, name) && direction == Direction.Descending ? "active" : "inactive";
        public string SortByCountAscClassName => string.Equals(sortProperty, count) && direction == Direction.Ascending ? "active" : "inactive";
        public string SortByCountDescClassName => string.Equals(sortProperty, count) && direction == Direction.Descending ? "active" : CountAvailable ? string.Empty : "inactive";

        private bool CountAvailable => !string.Equals(sortProperty, count);
        private bool NameAvailable => !string.Equals(sortProperty, name);

        private void AppleSorting(string sortProperty, Direction direction)
        {
            this.sortProperty = sortProperty;
            this.direction = direction;

            sortChangedHandler.Invoke(this.sortProperty, this.direction);
        }
    }
}
