using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Data.EF.Interfaces
{
    public interface IDapperContext
    {
        Task ExecuteProcedureAsync(string spName, object? param = null, CancellationToken cancellationToken = default);
    }

    public interface IDapperContext<TEntity>
        where TEntity : class
    {
        Task<TEntity> SingleOrDefaultAsync(string command, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync(string command, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync<T>(string command, Func<TEntity, T, TEntity> map, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default);
    }
}
