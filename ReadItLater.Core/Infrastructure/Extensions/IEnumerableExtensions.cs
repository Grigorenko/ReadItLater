using System.Collections.Generic;
using System.Linq;

namespace ReadItLater.Core.Infrastructure
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Return collection without elements started <paramref name="start"/> and length <paramref name="length"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public static IEnumerable<T> Splice<T>(this IEnumerable<T> collection, int start, int length)
        {
            var list = collection.ToList();

            for (int i = 0; i < list.Count(); i++)
                if (i < start || i >= start + length)
                    yield return list[i];
        }
    }
}
