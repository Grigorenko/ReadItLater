using Dapper;
using Microsoft.Data.SqlClient;
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

    public class DapperContext : IDapperContext
    {
        private readonly string connectionString;

        public DapperContext(Options.IDbConnection dbConnection)
        {
            this.connectionString = dbConnection.ConnectionString;
        }

        public async Task ExecuteProcedureAsync(string spName, object? param = null, CancellationToken cancellationToken = default)
        {
            await this.UseDefaultConnectionAsync(async db => await db.ExecuteAsync(new CommandDefinition(spName, parameters: param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)));
        }

        private async Task<T> UseDefaultConnectionAsync<T>(Func<IDbConnection, Task<T>> func)
        {
            using (IDbConnection db = new SqlConnection(this.connectionString))
                return await func(db);
        }
    }

    public interface IDapperContext<TEntity>
        where TEntity : class
    {
        Task<TEntity> SingleOrDefaultAsync(string command, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync(string command, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> SelectAsync<T>(string command, Func<TEntity, T, TEntity> map, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default);
    }
}
