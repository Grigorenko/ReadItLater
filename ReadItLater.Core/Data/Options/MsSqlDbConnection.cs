using System;

namespace ReadItLater.Core.Data.Options
{
    public sealed class MsSqlDbConnection : IDbConnection
    {
        public static string Section = "DbConnection";
        public static string Default = "Default";

        private string? connectionString;

        public string ConnectionString
        {
            get => connectionString ?? throw new ArgumentException($"{Section}:{Default} not valid.");
            set => connectionString = value;
        }
    }
}
