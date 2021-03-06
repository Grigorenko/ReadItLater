using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Data.Dapper
{
    public interface IDapperContext
    {
        Task ExecuteProcedureAsync(string spName, object? param = null, CancellationToken cancellationToken = default);
        Task<TEntity> SingleOrDefaultAsync<TEntity>(string command, object? param = null, CommandType? commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync<TEntity>(string command, object? param = null, CommandType? commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync<TEntity, T>(string command, Func<TEntity, T, TEntity> map, object? param = null, CommandType? commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
    }

    public interface IDapperContext<TEntity>
        where TEntity : class
    {
        Task<TEntity> SingleOrDefaultAsync(string command, object? param = null, CommandType? commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync(string command, object? param = null, CommandType? commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync<T>(string command, Func<TEntity, T, TEntity> map, object? param = null, CommandType? commandType = CommandType.StoredProcedure, CancellationToken cancellationToken = default);
    }
}
