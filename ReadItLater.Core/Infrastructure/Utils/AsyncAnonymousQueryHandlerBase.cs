using ReadItLater.Core.Data.Dapper;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Infrastructure
{
    public abstract class AsyncAnonymousQueryHandlerBase<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
        where TResult : class
    {
        protected readonly IDapperContext dapperContext;

        public AsyncAnonymousQueryHandlerBase(IDapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public abstract Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken token = default);
    }
}
