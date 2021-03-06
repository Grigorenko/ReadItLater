
namespace ReadItLater.Core.Infrastructure
{
    public sealed class DefaultValueResult<TValue> : IValue<TValue>
        where TValue : class
    {
        public TValue? Value { get; private set; }

        public DefaultValueResult(Result<TValue> result)
        {
            Value = result.Value;
        }
    }
}
