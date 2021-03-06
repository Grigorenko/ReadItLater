using System.Collections.Generic;

namespace ReadItLater.Core.Infrastructure
{
    public sealed class DefaultFailedResult : IFailedResult
    {
        public IEnumerable<IResultError>? Errors { get; private set; }

        public DefaultFailedResult(IResult result) => Errors = result.Errors;
    }
}
