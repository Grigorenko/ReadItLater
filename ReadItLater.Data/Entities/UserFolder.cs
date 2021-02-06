using System;

namespace ReadItLater.Data
{
    public class UserFolder : IEntity
    {
        public Guid UserId { get; set; }
        public Guid FolderId { get; set; }
    }
}
