
namespace ReadItLater.Core.Infrastructure
{
    public interface IValue<TValue>
        where TValue : class
    {
        TValue? Value { get; }
    }
}
