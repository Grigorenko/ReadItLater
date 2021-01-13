using System;

namespace ReadItLater.Data.EF.Options
{
    public sealed class MsSqlDbConnection : IDbConnection
    {
        public static string DbConnectionSection = "DbConnection";
        public static string Default = "Default";

        private string? connectionString;

        public string ConnectionString
        {
            get => connectionString ?? throw new ArgumentException($"{DbConnectionSection}:{Default} not valid.");
            set => connectionString = value;
        }
    }
}
