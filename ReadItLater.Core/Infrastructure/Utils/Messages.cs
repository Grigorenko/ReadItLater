using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ReadItLater.Core.Infrastructure.Utils
{
    public class Messages : IMessages
    {
        private readonly IServiceProvider _provider;

        public Messages(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<Result> DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class, ICommand
        {
            var handler = _provider.GetService<IAsyncCommandHandler<TCommand>>();

            if (handler is null)
                throw new ArgumentNullException($"{typeof(IAsyncCommandHandler<TCommand>).FullName} not registered in di container.");

            Result result = await handler.HandleAsync(command, cancellationToken);

            return result;
        }

        public async Task<Result<TResult>> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var type = typeof(IAsyncQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(TResult) };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            Result<TResult> result = await handler.HandleAsync((dynamic)query, (dynamic)cancellationToken);

            return result;
        }
    }
}
