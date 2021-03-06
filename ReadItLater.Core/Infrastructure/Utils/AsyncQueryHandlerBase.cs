using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Infrastructure
{
    public abstract class AsyncQueryHandlerBase<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
        where TResult : class
    {
        protected readonly IDapperContext dapperContext;
        protected readonly IUserProvider userProvider;
        protected readonly Guid currentUserId;

        public AsyncQueryHandlerBase(
            IDapperContext dapperContext,
            IUserProvider userProvider)
        {
            this.dapperContext = dapperContext;
            this.userProvider = userProvider;

            if (userProvider is null)
                throw new Exception($"{nameof(IUserProvider)} does not registered.");

            this.currentUserId = userProvider.CurrentUserId.HasValue
                ? userProvider.CurrentUserId.Value
                : throw new NullReferenceException($"{nameof(IUserProvider.CurrentUserId)} is null.");
        }

        public abstract Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken token = default);
    }
}
