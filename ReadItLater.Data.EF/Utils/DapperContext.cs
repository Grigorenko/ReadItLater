using Dapper;
using Microsoft.Data.SqlClient;
using ReadItLater.Data.EF.Interfaces;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Data.EF
{
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
}
