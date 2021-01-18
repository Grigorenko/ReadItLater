using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadItLater.Web.Client.Services.Models
{
    public class Badges
    {
        private ICollection<Badge> items;
        private int maxPosition;
        public ICollection<Badge> Items
        {
            get
            {
                if (!items.Any())
                    return items;

                var sortedItems = items.OrderBy(b => b.Position).ToList();

                sortedItems
                    .ForEach(i =>
                    {
                        i.IsArrowUpActive = true;
                        i.IsArrowDownActive = true;
                    });
                sortedItems.First().IsArrowUpActive = false;
                sortedItems.Last().IsArrowDownActive = false;

                return sortedItems;
            }
        }
        public string[] Keys => items.Select(p => p.Key).ToArray();

        public Badges()
        {
            items = new List<Badge>();
        }

        public Badges(IEnumerable<Badge> badges)
        {
            items = badges.ToList();
        }

        public void Add(string key, string value)
        {
            if (ContainsKey(key))
                throw new ArgumentException($"Key '{key}' already exist.");

            var badge = new Badge { Key = key, Value = value, Position = maxPosition++ };

            items.Add(badge);
        }

        public void Remove(string key)
        {
            if (!ContainsKey(key))
                return;

            items = items.Where(i => !string.Equals(i.Key, key)).ToList();
        }

        public void MoveUp(string key)
        {
            Console.WriteLine($"MoveUp - {key}");

            var sortedItems = items.OrderBy(b => b.Position).ToArray();

            for (int i = 1; i < sortedItems.Length; i++)
            {
                var current = sortedItems[i];

                if (string.Equals(current.Key, key))
                {
                    int position = sortedItems[i - 1].Position;
                    sortedItems[i - 1].Position = current.Position;
                    current.Position = position;
                }
            }
        }

        public void MoveDown(string key)
        {
            Console.WriteLine($"MoveDown - {key}");

            var sortedItems = items.OrderBy(b => b.Position).ToArray();

            for (int i = 0; i < sortedItems.Length - 1; i++)
            {
                var current = sortedItems[i];

                if (string.Equals(current.Key, key))
                {
                    int position = sortedItems[i + 1].Position;
                    sortedItems[i + 1].Position = current.Position;
                    current.Position = position;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return Keys.Any(k => string.Equals(k, key));
        }
    }
}
