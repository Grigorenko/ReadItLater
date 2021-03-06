using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ReadItLater.Core.Infrastructure
{
    [Serializable]
    public class NotFoundException : ExtendedException, ICustomResultException
    {
        public IEnumerable<IResultError> Errors { get; private set; }

        private NotFoundException(string errorMessage) : this(string.Empty, errorMessage) { }

        public NotFoundException(string propertyName, string errorMessage) : this(propertyName, errorMessage, default) { }

        public NotFoundException(string propertyName, string errorMessage, string? errorCode) : base(errorMessage)
        {
            Errors = new DefaultResultError[] { new DefaultResultError(propertyName, errorCode, errorMessage) };
        }

        public static NotFoundException Make<TResult>(TResult result, Expression<Func<TResult, object>> predicate)
        {
            var propName = GetPropertyInfo(predicate);

            return new NotFoundException(propName, $"{typeof(TResult).Name} does not found with {propName} equal {typeof(TResult).GetProperty(propName).GetValue(result)}");
        }

        public static NotFoundException Make<TResult>(IQuery<TResult> query, params Expression<Func<IQuery<TResult>, object>>[] predicates)
        {
            StringBuilder builder = new StringBuilder($"{typeof(TResult).Name} does not found with ");
            int iterator = default;

            foreach (var predicate in predicates)
            {
                var propName = GetPropertyInfo(predicate);
                var value = query.GetType().GetProperty(propName).GetValue(query);

                if (iterator++ != default)
                    builder.Append(" and ");

                builder.Append(propName).Append(" equal ").Append(value);
            }

            return new NotFoundException(builder.ToString());

            //var propName = GetPropertyInfo(predicate);

            //return new NotFoundException(propName, $"{typeof(TResult).Name} does not found with {propName} equal {typeof(TResult).GetProperty(propName).GetValue(result)}");
        }
    }
}
