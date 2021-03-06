using System;
using System.Collections.Generic;

namespace ReadItLater.Data
{
    public class Ref : IEntity
    {
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string? Image { get; set; }
        public Priority Priority { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public Folder? Folder { get; set; }
        public IEnumerable<TagRef>? TagRels { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}
