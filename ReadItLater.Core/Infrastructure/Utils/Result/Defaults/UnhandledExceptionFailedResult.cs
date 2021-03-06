using System;
using System.Collections.Generic;

namespace ReadItLater.Core.Infrastructure
{
    public sealed class UnhandledExceptionFailedResult : IFailedResult
    {
        public IEnumerable<IResultError>? Errors { get; private set; }

        public UnhandledExceptionFailedResult(Exception exception) => Errors = new ExceptionResultError(exception).GetResults();
    }
}
