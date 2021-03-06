using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Infrastructure.Utils
{
    public interface IMessages
    {
        Task<Result> DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand;
        Task<Result<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            where TResult : class;
    }
}
