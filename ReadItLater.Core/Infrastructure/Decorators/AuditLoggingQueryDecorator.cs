using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ReadItLater.Core.Infrastructure.Decorators
{
    public sealed class AuditLoggingQueryDecorator<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
        where TResult : class
    {
        private readonly IAsyncQueryHandler<TQuery, TResult> _handler;
        private readonly ILogger<IAsyncQueryHandler<TQuery, TResult>> _logger;

        public AuditLoggingQueryDecorator(
            IAsyncQueryHandler<TQuery, TResult> handler,
            ILogger<IAsyncQueryHandler<TQuery, TResult>> logger)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            var queryName = query.GetType().Name;
            var newLineSpace = new string(' ', 6);
            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            var queryJson = JsonSerializer.Serialize(query, serializerOptions);
            _logger.LogInformation($"Query handler of type {queryName} processing payload: \n{newLineSpace}{queryJson}");

            try
            {
                var result = await _handler.HandleAsync(query);

                if (result.IsSuccess)
                    _logger.LogInformation($"Query handler of type {queryName} result: Success");

                else
                    _logger.LogInformation($"Query handler of type {queryName} result: Failure. \n{newLineSpace}Reason: \n{newLineSpace}{JsonSerializer.Serialize(result.Errors, serializerOptions)}");

                return result;
            }

            catch (Exception e)
            {
                _logger.LogError($"Query handler of type {queryName} error. \n{newLineSpace}Reason: {e}");
                throw;
            }
        }
    }
}
