using System;

namespace ReadItLater.Core.Infrastructure
{
    public class ResultFailureException : Exception
    {
        public string Error { get; }

        internal ResultFailureException(string error)
            : base(Result.Messages.ValueIsInaccessibleForFailure)
        {
            Error = error;
        }
    }

    public class ResultFailureException<TException> : ResultFailureException
    {
        public new TException Error { get; }

        internal ResultFailureException(TException error)
            : base(Result.Messages.ValueIsInaccessibleForFailure)
        {
            Error = error;
        }
    }
}
