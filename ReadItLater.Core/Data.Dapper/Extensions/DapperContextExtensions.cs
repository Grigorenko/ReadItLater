using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Data.Dapper
{
    public static class DapperContextExtensions
    {
        public static async Task<TResult> ExecStoredProcedureSingleWithMapping<TResult, T>(
            this IDapperContext context,
            string command,
            object param,
            Func<TResult, Guid> keySelector,
            Action<TResult, T> map,
            CancellationToken cancellationToken)
            where TResult : class
        {
            var results = await context.ExecStoredProcedureWithMapping(command, param, keySelector, map, cancellationToken);

            return results.FirstOrDefault();
        }

        public static async Task<IEnumerable<TResult>> ExecStoredProcedureWithMapping<TResult, T>(
            this IDapperContext context,
            string command,
            object param,
            Func<TResult, Guid> keySelector,
            Action<TResult, T> map,
            CancellationToken cancellationToken)
            where TResult : class
        {
            IEnumerable<TResult> results = new List<TResult>();

            await context.SelectAsync(
                command,
                map: Map(keySelector, map, out results),
                param: param,
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: cancellationToken);

            return results;
        }

        private static Func<TResult, T, TResult> Map<TResult, T>(Func<TResult, Guid> keySelector, Action<TResult, T> map, out IEnumerable<TResult> results)
            where TResult : class =>
            Map<Guid, TResult, T>(keySelector, map, out results);

        private static Func<TResult, T, TResult> Map<TKey, TResult, T>(Func<TResult, TKey> keySelector, Action<TResult, T> map, out IEnumerable<TResult> results)
            where TResult : class
        {
            var lookup = new Dictionary<TKey, TResult>();
            results = lookup.Values;

            return (r, t) =>
            {
                TResult current = r;
                var key = keySelector(r);

                if (!lookup.TryGetValue(key, out current))
                    lookup.Add(key, current = r);

                map(r, t);

                return r;
            };
        }

        private static Func<TResult, T, V, TResult> Map<TKey, TResult, T, V>(Func<TResult, TKey> keySelector, Action<TResult, T, V> map, out IEnumerable<TResult> results)
            where TResult : class
        {
            var lookup = new Dictionary<TKey, TResult>();
            results = lookup.Values;

            return (r, t, v) =>
            {
                TResult current = r;
                var key = keySelector(r);

                if (!lookup.TryGetValue(key, out current))
                    lookup.Add(key, current = r);

                map(r, t, v);

                return r;
            };
        }
    }
}
