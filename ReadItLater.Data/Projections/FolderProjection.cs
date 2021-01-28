using System;
using System.Text.Json;

namespace ReadItLater.Data
{
    public class FolderProjection
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
