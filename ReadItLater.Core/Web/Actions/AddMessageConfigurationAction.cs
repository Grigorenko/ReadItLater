using Microsoft.Extensions.DependencyInjection;
using ReadItLater.Core.Infrastructure.Utils;

namespace ReadItLater.Core.Api.Actions
{
    public static class AddMessageConfigurationAction
    {
        public static IServiceCollection AddMessages(this IServiceCollection services)
        {
            services.AddTransient<IMessages, Messages>();

            return services;
        }
    }
}
