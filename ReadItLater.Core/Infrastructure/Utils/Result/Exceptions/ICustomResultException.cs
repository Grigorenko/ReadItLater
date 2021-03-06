using System.Collections.Generic;

namespace ReadItLater.Core.Infrastructure
{
    public interface ICustomResultException
    {
        IEnumerable<IResultError> Errors { get; }
        string Message { get; }
        string StackTrace { get; }
    }
}
