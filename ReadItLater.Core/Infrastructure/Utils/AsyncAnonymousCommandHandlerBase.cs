using ReadItLater.Core.Data.Dapper;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Infrastructure
{
    public abstract class AsyncAnonymousCommandHandlerBase<TCommand> : IAsyncCommandHandler<TCommand>
       where TCommand : class, ICommand
    {
        protected readonly IDapperContext dapperContext;

        public AsyncAnonymousCommandHandlerBase(IDapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public abstract Task<Result> HandleAsync(TCommand command, CancellationToken token = default);
    }
}
