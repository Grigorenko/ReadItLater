using System;

namespace ReadItLater.Core.Infrastructure.Attributes
{
    public interface IHandlerDecoratorAttribute
    {
        Type DecoratorType { get; }
    }
}
