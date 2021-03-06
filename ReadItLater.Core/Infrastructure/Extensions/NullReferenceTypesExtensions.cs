
namespace ReadItLater.Core.Infrastructure
{
    public static class NullReferenceTypesExtensions
    {
        public static bool HasValue(this object? @object) => !(@object is null);
    }
}
