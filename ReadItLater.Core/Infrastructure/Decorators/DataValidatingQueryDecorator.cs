using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ReadItLater.Core.Infrastructure.Decorators
{
    public class DataValidatingQueryDecorator<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
        where TResult : class
    {
        private readonly ILogger logger;
        private readonly IAsyncQueryHandler<TQuery, TResult> handler;
        private readonly IValidator<TQuery> validator;

        public DataValidatingQueryDecorator(
            ILogger<DataValidatingQueryDecorator<TQuery, TResult>> logger,
            IAsyncQueryHandler<TQuery, TResult> handler,
            IEnumerable<IValidator<TQuery>> validators)
        {
            this.logger = logger;
            this.handler = handler;
            this.validator = validators.FirstOrDefault();
        }

        public Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken token = default)
        {
            if (validator is null)
            {
                logger.LogWarning($"Query handler of type {query.GetType().Name} has not any validator.");
            }
            else
            {
                var validationResult = validator.Validate(query);

                if (!validationResult.IsValid)
                    return Task.FromResult(Result.Failure<TResult, DataValidationException>(new DataValidationException(validationResult.Errors)));
            }

            return handler.HandleAsync(query, token);
        }
    }
}
