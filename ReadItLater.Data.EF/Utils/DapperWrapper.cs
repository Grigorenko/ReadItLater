using Dapper;
using Microsoft.Data.SqlClient;
using ReadItLater.Data.EF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Data.EF.Utils
{
    public class DapperWrapper<TEntity> : IDapperContext<TEntity>
        where TEntity : class
    {
        private readonly string connectionString;

        public DapperWrapper(Options.IDbConnection dbConnection)
        {
            this.connectionString = dbConnection.ConnectionString;
        }

        public async Task<TEntity> SingleOrDefaultAsync(string command, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default) =>
            await this.UseDefaultConnectionAsync(async db => await db.QuerySingleOrDefaultAsync<TEntity>(new CommandDefinition(command, parameters: param, commandType: commandType, cancellationToken: cancellationToken)));

        public async Task<IEnumerable<TEntity>> SelectAsync(string command, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default) =>
            await this.UseDefaultConnectionAsync(async db => await db.QueryAsync<TEntity>(new CommandDefinition(command, parameters: param, commandType: commandType, cancellationToken: cancellationToken)));

        public async Task<IEnumerable<TEntity>> SelectAsync<T>(string command, Func<TEntity, T, TEntity> map, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default) =>
            await this.UseDefaultConnectionAsync(async db => await db.QueryAsync<TEntity, T, TEntity>(new CommandDefinition(command, parameters: param, commandType: commandType, cancellationToken: cancellationToken), map));

        private async Task<T> UseDefaultConnectionAsync<T>(Func<IDbConnection, Task<T>> func)
        {
            using (IDbConnection db = new SqlConnection(this.connectionString))
                return await func(db);
        }
    }
}
