using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
using ReadItLater.Core.Web;
using Microsoft.Extensions.Configuration;

namespace ReadItLater.Core.Api.Actions
{
    public static class AddValidatorsConfigurationAction
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            var assemblyName = configuration
                .GetSection(AssembliesConfiguration.Section)
                .GetValue<string?>(nameof(AssembliesConfiguration.Infrastructure));

            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(AssembliesConfiguration.Section));

            var assembly = Assembly.Load(new AssemblyName(assemblyName));

            assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(t => t == typeof(IValidator)))
                .ToList()
                .ForEach(t => services.AddScoped(typeof(IValidator<>).MakeGenericType(t.BaseType!.GetGenericArguments()), t));

            return services;
        }
    }
}