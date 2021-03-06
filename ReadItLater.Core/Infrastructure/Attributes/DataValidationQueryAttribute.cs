using System;
using ReadItLater.Core.Infrastructure.Decorators;

namespace ReadItLater.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DataValidationQueryAttribute : Attribute, IQueryHandlerDecoratorAttribute
    {
        public DataValidationQueryAttribute()
        {
        }

        public Type DecoratorType => typeof(DataValidatingQueryDecorator<,>);
    }
}
