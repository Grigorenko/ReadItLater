using System;
using System.Linq.Expressions;
using System.Text;

namespace ReadItLater.Core.Infrastructure
{
    public sealed class ExceptionResultError : IResultError
    {
        private readonly Exception exception;
        private readonly string propName;

        public ExceptionResultError(Exception exception) => (this.exception, propName) = (exception, string.Empty);
        public ExceptionResultError(Expression<Func<object>> field, Exception exception) => 
            (this.exception, propName) = (exception, GetPropertyInfo<object>(field));

        public IResultError[] GetResults() => new IResultError[] { this };

        public string PropertyName => propName;

        public string ErrorMessage => this.GetResultFromException();

        public string? ErrorCode => null;

        private string GetResultFromException()
        {
            var message = new StringBuilder(exception?.Message);
            var currentException = exception;

            while (currentException.HasValue() && currentException!.InnerException.HasValue())
            {
                message.AppendLine("InnerException: " + currentException!.InnerException!.Message);
                currentException = currentException.InnerException;
            }

            return message.ToString();
        }

        private string GetPropertyInfo<TProperty>(Expression<Func<TProperty>> propertyLambda)
        {
            if (propertyLambda.Body is MemberExpression body)
                throw new NotImplementedException();

            if (propertyLambda.Body is UnaryExpression expr)
                if (expr.Operand is MemberExpression operand)
                    return operand.Member.Name;

            throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));
        }
    }
}
