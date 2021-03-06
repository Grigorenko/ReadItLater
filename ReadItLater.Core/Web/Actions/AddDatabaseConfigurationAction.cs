using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadItLater.Core.Data.Dapper;
using ReadItLater.Core.Data.Options;
using ReadItLater.Core.Web;

namespace ReadItLater.Core.Api.Actions
{
    public static class AddDatabaseConfigurationAction
    {
        public static IServiceCollection AddMsSqlDbConnection(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(MsSqlDbConnection.Section);

            if (!section.Exists())
                throw new ArgumentNullException(nameof(MsSqlDbConnection.Section));

            var connection = new MsSqlDbConnection
            {
                ConnectionString = section.GetValue<string>(nameof(MsSqlDbConnection.Default))
            };

            services.AddSingleton<IDbConnection>(connection);

            return services;
        }

        public static IServiceCollection AddAssembliesConfiguration(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            var assemblies = configuration.GetSection(AssembliesConfiguration.Section);

            if (assemblies is null)
                throw new ArgumentNullException(nameof(AssembliesConfiguration.Section));

            services.Configure<AssembliesConfiguration>(options =>
            {
                options.Dtos = assemblies.GetValue<string?>(nameof(AssembliesConfiguration.Dtos));
                options.Entities = assemblies.GetValue<string?>(nameof(AssembliesConfiguration.Entities));
                options.Infrastructure = assemblies.GetValue<string?>(nameof(AssembliesConfiguration.Infrastructure));
            });

            return services;
        }

        public static IServiceCollection AddDapperRepositoryProvider(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDapperContext<>), typeof(DapperWrapper<>));
            services.AddScoped<IDapperContext, DapperWrapper>();

            return services;
        }
    }
}
