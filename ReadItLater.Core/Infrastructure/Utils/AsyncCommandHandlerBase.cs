using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Infrastructure.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Infrastructure
{
    public abstract class AsyncCommandHandlerBase<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        protected readonly IDapperContext dapperContext;
        protected readonly IUserProvider userProvider;
        protected readonly Guid currentUserId;

        public AsyncCommandHandlerBase(
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

        public abstract Task<Result> HandleAsync(TCommand command, CancellationToken token = default);
    }
}
