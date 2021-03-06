using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ReadItLater.Core.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            return configuration;
        }
    }
}
