using System;
using System.Collections.Generic;

namespace ReadItLater.Data
{
    public class Tag : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<TagRef>? TagRels { get; set; }
    }
}
