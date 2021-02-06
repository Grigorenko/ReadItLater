using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReadItLater.Data.EF.Options;
using ReadItLater.Web.Server.Utils;
using System;
using System.Text;

namespace ReadItLater.Web.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbConnection(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            var section = configuration.GetSection(MsSqlDbConnection.DbConnectionSection);

            if (!section.Exists())
                throw new ArgumentNullException(nameof(MsSqlDbConnection.DbConnectionSection));

            var connection = new MsSqlDbConnection
            {
                ConnectionString = section.GetValue<string>(nameof(MsSqlDbConnection.Default))
            };

            services.AddSingleton<IDbConnection>(connection);

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            services.Configure<AuthenticationConfiguration>(configuration.GetSection(AuthenticationConfiguration.AuthenticationSection));

            AuthenticationConfiguration options = new AuthenticationConfiguration();
            configuration.GetSection(AuthenticationConfiguration.AuthenticationSection).Bind(options);

            byte[] secret = Encoding.ASCII.GetBytes(options.Secret);

            services
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        private static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            if (configuration is null)
                throw new Exception($"{nameof(IConfiguration)} haven't registered.");

            return configuration;
        }
    }
}
