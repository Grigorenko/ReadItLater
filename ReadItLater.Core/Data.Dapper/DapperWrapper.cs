using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Core.Data.Dapper
{
    public class DapperWrapper : DapperWrapperBase, IDapperContext
    {
        public DapperWrapper(Options.IDbConnection dbConnection) : base(dbConnection) { }

        public async Task ExecuteProcedureAsync(string spName, object? param = null, CancellationToken cancellationToken = default)
        {
            await this.UseDefaultConnectionAsync(async db => await db.ExecuteAsync(
                new CommandDefinition(spName, parameters: param, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken))
            );
        }
    }

    public class DapperWrapper<TEntity> : DapperWrapperBase, IDapperContext<TEntity>
        where TEntity : class
    {
        public DapperWrapper(Options.IDbConnection dbConnection) : base(dbConnection) { }

        public async Task<TEntity> SingleOrDefaultAsync(
            string command, 
            object? param = null, 
            CommandType? commandType = CommandType.StoredProcedure, 
            CancellationToken cancellationToken = default) =>
            await base.SingleOrDefaultAsync<TEntity>(command, param, commandType, cancellationToken);

        public async Task<IEnumerable<TEntity>> SelectAsync(
            string command, 
            object? param = null, 
            CommandType? commandType = CommandType.StoredProcedure, 
            CancellationToken cancellationToken = default) =>
            await base.SelectAsync<TEntity>(command, param, commandType, cancellationToken);

        public async Task<IEnumerable<TEntity>> SelectAsync<T>(
            string command, 
            Func<TEntity, T, TEntity> map, 
            object? param = null, 
            CommandType? commandType = CommandType.StoredProcedure, 
            CancellationToken cancellationToken = default) =>
            await base.SelectAsync<TEntity, T>(command, map, param, commandType, cancellationToken);
    }
}
