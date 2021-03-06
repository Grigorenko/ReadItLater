using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReadItLater.Core.Infrastructure
{
    [Serializable]
    public class ConflictException : ExtendedException, ICustomResultException
    {
        public IEnumerable<IResultError> Errors { get; private set; }

        private ConflictException(string errorMessage) : this(string.Empty, errorMessage) { }

        public ConflictException(string propertyName, string errorMessage) : this(propertyName, errorMessage, default) { }

        public ConflictException(string propertyName, string errorMessage, string? errorCode) : base(errorMessage)
        {
            Errors = new DefaultResultError[] { new DefaultResultError(propertyName, errorCode, errorMessage) };
        }

        public static ConflictException MakeAlreadyExist<TEntity>(ICommand command, Expression<Func<ICommand, object>> predicate)
        {
            var propName = GetPropertyInfo(predicate);
            var value = typeof(ICommand).GetProperty(propName).GetValue(command);

            return new ConflictException($"{typeof(TEntity).Name} with {propName} equal '{value}' already exist.");
        }
    }
}
