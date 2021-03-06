using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReadItLater.Core.Web;
using ReadItLater.Web.Server.Utils;
using System;
using System.Text;

namespace ReadItLater.Web.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            services.Configure<AuthenticationConfiguration>(configuration.GetSection(AuthenticationConfiguration.AuthenticationSection));

            AuthenticationConfiguration options = new AuthenticationConfiguration();
            configuration.GetSection(AuthenticationConfiguration.AuthenticationSection).Bind(options);

            if (string.IsNullOrEmpty(options.Secret))
                throw new ArgumentNullException(nameof(options.Secret));

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
    }
}
