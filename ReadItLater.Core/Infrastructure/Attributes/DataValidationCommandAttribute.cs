using System;
using ReadItLater.Core.Infrastructure.Decorators;

namespace ReadItLater.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DataValidationCommandAttribute : Attribute, ICommandHandlerDecoratorAttribute
    {
        public DataValidationCommandAttribute()
        {
        }

        public Type DecoratorType => typeof(DataValidatingCommandDecorator<>);
    }
}
