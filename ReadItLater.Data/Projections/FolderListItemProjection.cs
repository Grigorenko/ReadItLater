using System;
using System.Collections.Generic;

namespace ReadItLater.Data
{
    public class FolderListItemProjection
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int RefsCount { get; set; }

        public ICollection<FolderListItemProjection> Folders { get; set; }
    }
}
