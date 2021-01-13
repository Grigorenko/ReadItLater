using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Core.Data.EFCore.Interfaces;
using System.Reflection;
using ReadItLater.Data.EF.Options;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Core.Data.EFCore.Utils
{
    public class EfDbContext : DbContext, IEfDbContext
    {
        private readonly string connectionString;
        private readonly Assembly entityAssembly;

        public EfDbContext()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{Environment.UserName.Replace(" ", string.Empty)}.json", optional: true, reloadOnChange: true)
              .Build();

            var section = configuration.GetSection(MsSqlDbConnection.DbConnectionSection);

            if (!section.Exists())
                throw new ArgumentNullException(nameof(MsSqlDbConnection.DbConnectionSection));

            connectionString = section.GetValue<string>(nameof(MsSqlDbConnection.Default));

            var assebliesOptions = configuration.GetSection(AssembliesConfiguration.AssembliesSection);

            if (assebliesOptions is null)
                throw new ArgumentNullException(nameof(AssembliesConfiguration.AssembliesSection));

            var entity = assebliesOptions.GetValue<string>(nameof(AssembliesConfiguration.Entities));
            this.entityAssembly = GetTargetAssembly(entity);
        }

        public EfDbContext(IDbConnection dbConnection, IOptions<AssembliesConfiguration> assebliesOptions)
        {
            this.connectionString = dbConnection.ConnectionString;

            if (string.IsNullOrEmpty(assebliesOptions.Value.Entities))
                throw new ArgumentNullException(nameof(AssembliesConfiguration.Entities));

            this.entityAssembly = GetTargetAssembly(assebliesOptions.Value.Entities!);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.entityAssembly);
        }

        private Assembly GetTargetAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(new AssemblyName(assemblyName));
            }

            catch (System.IO.FileNotFoundException ex)
            {
                //this.logger.LogCritical(ex, "Load assembly from configuration section 'DbContext:EntityAssemblyName' does not success.");

                throw new Exception($"Load assembly with name '{assemblyName}' does not success.", ex);
            }
        }

    }

    public partial class EfDbContext<TEntity> : EfDbContext
        where TEntity : class, IEntity
    {
        protected DbSet<TEntity> dbSet;
        protected DbContext context;

        public EfDbContext(IDbConnection dbConnection, IOptions<AssembliesConfiguration> assebliesOptions)
            : base(dbConnection, assebliesOptions) =>
                (dbSet, context) = (Set<TEntity>(), this);
    }
}
