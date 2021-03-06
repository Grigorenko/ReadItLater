using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ReadItLater.Web.Client.Services.Http
{
    public class Result<TValue> : ErrorResult
        where TValue : class
    {
        public TValue? Value { get; set; }
    }

    public class ErrorResult
    {
        public IEnumerable<DefaultResultError>? Errors { get; set; }

        public bool IsSuccess => Errors is null || !Errors.Any();
    }

    public sealed class DefaultResultError
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
