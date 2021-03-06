using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Infrastructure
{
    public interface IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        Task<Result> HandleAsync(TCommand command, CancellationToken token = default);
    }
}
