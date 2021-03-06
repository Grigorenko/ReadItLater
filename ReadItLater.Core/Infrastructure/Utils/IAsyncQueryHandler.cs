using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Infrastructure
{
    public interface IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
        where TResult : class
    {
        Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken token = default);
    }
}
