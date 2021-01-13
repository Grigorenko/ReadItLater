using System;

namespace ReadItLater.Data
{
    public class TagListItemProjection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RefsCount { get; set; }
    }
}
