using System.Collections.Generic;

namespace ReadItLater.Core.Infrastructure
{
    public interface IFailedResult
    {
        IEnumerable<IResultError>? Errors { get; }
    }
}
