using System;

namespace ReadItLater.Data
{
    public class UserProjection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
