
namespace ReadItLater.Core.Infrastructure
{
    public interface IQuery { }

    public interface IQuery<TResult> : IQuery
    {
    }
}
