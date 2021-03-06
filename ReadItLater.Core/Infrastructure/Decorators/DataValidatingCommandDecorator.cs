using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ReadItLater.Core.Infrastructure.Decorators
{
    public class DataValidatingCommandDecorator<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ILogger logger;
        private readonly IAsyncCommandHandler<TCommand> handler;
        private readonly IValidator<TCommand>? validator;

        public DataValidatingCommandDecorator(
            ILogger<DataValidatingCommandDecorator<TCommand>> logger,
            IAsyncCommandHandler<TCommand> handler,
            IEnumerable<IValidator<TCommand>> validators)
        {
            this.logger = logger;
            this.handler = handler;
            this.validator = validators.FirstOrDefault();
        }

        public Task<Result> HandleAsync(TCommand command, CancellationToken token = default)
        {
            if (validator is null)
            {
                logger.LogWarning($"Command handler of type {command.GetType().Name} has not any validator.");
            }
            else
            {
                var validationResult = validator.Validate(command);

                if (!validationResult.IsValid)
                    return Task.FromResult(Result.Failure<DataValidationException>(new DataValidationException(validationResult.Errors)));
            }

            return handler.HandleAsync(command, token);
        }
    }
}
