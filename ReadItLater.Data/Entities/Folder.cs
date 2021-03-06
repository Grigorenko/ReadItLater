using System;
using System.Collections.Generic;

namespace ReadItLater.Data
{
    public class Folder : IEntity
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public ICollection<Ref>? Refs { get; set; }
        public ICollection<Folder>? Folders { get; set; }
        public Folder? Parent { get; set; }
    }
}
