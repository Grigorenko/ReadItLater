
namespace ReadItLater.Core.Infrastructure
{
    public interface IResult : IFailedResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
    }
}
