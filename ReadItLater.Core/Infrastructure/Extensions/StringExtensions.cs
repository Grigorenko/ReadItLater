using System;
using System.Collections.Generic;

namespace ReadItLater.Core.Infrastructure
{
    public static class StringExtensions
    {
        public static int[] ToArray(this string source, char separator)
        {
            if (string.IsNullOrEmpty(source))
                return new int[0];

            var parts = source.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<int>();

            foreach (var item in parts)
                if (int.TryParse(item, out int num))
                    result.Add(num);

            return result.ToArray();
        }
    }
}
