
namespace ReadItLater.Core.Infrastructure
{
    public interface IResultError
    {
        string PropertyName { get; }
        string ErrorMessage { get; }
        string? ErrorCode { get; }
    }
}
