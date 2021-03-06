using System;
using ReadItLater.Core.Infrastructure.Decorators;

namespace ReadItLater.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AuditLogCommandAttribute : Attribute, ICommandHandlerDecoratorAttribute
    {
        public AuditLogCommandAttribute()
        {
        }

        public Type DecoratorType => typeof(AuditLoggingCommandDecorator<>);
    }
}