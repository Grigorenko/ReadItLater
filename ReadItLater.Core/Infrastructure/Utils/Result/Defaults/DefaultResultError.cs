
using System.Text.Json.Serialization;

namespace ReadItLater.Core.Infrastructure
{
    public sealed class DefaultResultError : IResultError
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }
        public string? ErrorCode { get; }

        public DefaultResultError(string propertyName, string errorMessage) : this(propertyName, default, errorMessage)
        {
        }

        [JsonConstructor]
        public DefaultResultError(string propertyName, string? errorCode, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public override string ToString() =>
            string.Format("{0}: {1}{2}",
                PropertyName,
                string.IsNullOrEmpty(ErrorCode) ? string.Empty : $"({ErrorCode})",
                ErrorMessage
            );
    }
}
