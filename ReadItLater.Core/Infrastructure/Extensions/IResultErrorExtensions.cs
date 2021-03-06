using System.Collections.Generic;
using System.Linq;

namespace ReadItLater.Core.Infrastructure
{
    public static class IResultErrorExtensions
    {
        public static string Format(this IEnumerable<IResultError>? errors) =>
            string.Join(";", errors.Select(e => e.ToString()));
    }
}
