using System;

namespace ReadItLater.Core.Infrastructure.Utils
{
    public interface IUserProvider
    {
        public Guid? CurrentUserId { get; }
    }
}
