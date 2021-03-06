using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ReadItLater.Core.Infrastructure.Decorators
{
    public sealed class AuditLoggingCommandDecorator<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly IAsyncCommandHandler<TCommand> _handler;
        private readonly ILogger<IAsyncCommandHandler<TCommand>> _logger;

        public AuditLoggingCommandDecorator(
            IAsyncCommandHandler<TCommand> handler,
            ILogger<IAsyncCommandHandler<TCommand>> logger)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            var newLineSpace = new string(' ', 6);
            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            var commandJson = JsonSerializer.Serialize(command, serializerOptions);

            _logger.LogInformation($"Command handler of type {command.GetType().Name} processing payload:\n{newLineSpace}{commandJson}");

            try
            {
                var result = await _handler.HandleAsync(command);

                if (result.IsSuccess)
                    _logger.LogInformation($"Command handler of type {command.GetType().Name} result: Success");
                
                else
                    _logger.LogInformation($"Command handler of type {command.GetType().Name} result: Failure. \n{newLineSpace}Reason: \n{newLineSpace}{JsonSerializer.Serialize(result.Errors, serializerOptions)}");

                return result;
            }

            catch (Exception e)
            {
                _logger.LogError($"Command handler of type {command.GetType().Name} error. \n{newLineSpace}Reason: {e.Message}");
                throw e;
            }
        }
    }
}
