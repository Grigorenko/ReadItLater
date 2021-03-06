using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Data.Dapper
{
    public abstract class DapperWrapperBase
    {
        private readonly string connectionString;

        public DapperWrapperBase(Options.IDbConnection dbConnection)
        {
            this.connectionString = dbConnection.ConnectionString;
        }

        public async Task<TEntity> SingleOrDefaultAsync<TEntity>(
            string command,
            object? param = null,
            CommandType? commandType = CommandType.StoredProcedure,
            CancellationToken cancellationToken = default) =>
            await this.UseDefaultConnectionAsync(async db => await db.QuerySingleOrDefaultAsync<TEntity>(
                new CommandDefinition(command, parameters: param, commandType: commandType, cancellationToken: cancellationToken))
            );

        public async Task<IEnumerable<TEntity>> SelectAsync<TEntity>(
            string command,
            object? param = null,
            CommandType? commandType = CommandType.StoredProcedure,
            CancellationToken cancellationToken = default) =>
            await this.UseDefaultConnectionAsync(async db => await db.QueryAsync<TEntity>(
                new CommandDefinition(command, parameters: param, commandType: commandType, cancellationToken: cancellationToken))
            );

        public async Task<IEnumerable<TEntity>> SelectAsync<TEntity, T>(
            string command,
            Func<TEntity, T, TEntity> map,
            object? param = null,
            CommandType? commandType = CommandType.StoredProcedure,
            CancellationToken cancellationToken = default) =>
            await this.UseDefaultConnectionAsync(async db => 
                await db.QueryAsync<TEntity, T, TEntity>(
                    new CommandDefinition(command, parameters: param, commandType: commandType, cancellationToken: cancellationToken), 
                    map
                )
            );

        protected async Task<T> UseDefaultConnectionAsync<T>(Func<IDbConnection, Task<T>> func)
        {
            using (IDbConnection db = new SqlConnection(this.connectionString))
                return await func(db);
        }
    }
}
