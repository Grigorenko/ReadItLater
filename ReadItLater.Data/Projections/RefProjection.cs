using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ReadItLater.Data
{
    public class RefProjection
    {
        public bool IsDefault { get; set; } = true;

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? FolderId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public Priority Priority { get; set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;

        public ICollection<Tag> Tags { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
