using System;
using ReadItLater.Core.Infrastructure.Decorators;

namespace ReadItLater.Core.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AuditLogQueryAttribute : Attribute, IQueryHandlerDecoratorAttribute
    {
        public AuditLogQueryAttribute()
        {
        }

        public Type DecoratorType => typeof(AuditLoggingQueryDecorator<,>);
    }
}
