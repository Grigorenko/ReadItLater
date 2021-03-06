using System;
using System.Linq.Expressions;

namespace ReadItLater.Core.Infrastructure
{
    public abstract class ExtendedException : Exception
    {
        public ExtendedException(string message) : base(message) { }

        protected static string GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            if (propertyLambda.Body is MemberExpression expr)
                return expr.Member.Name;

            if (propertyLambda.Body is UnaryExpression unary)
                if (unary.Operand is MemberExpression member)
                    return member.Member.Name;

            throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", propertyLambda.ToString()));
        }
    }
}
