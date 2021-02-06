using System;

namespace ReadItLater.Data
{
    public class TagRef : IEntity
    {
        public Guid TagId { get; set; }
        public Guid RefId { get; set; }

        public Tag Tag { get; set; }
        public Ref Ref { get; set; }
    }
}
