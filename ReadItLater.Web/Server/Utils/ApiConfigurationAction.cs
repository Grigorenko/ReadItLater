using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ReadItLater.Core;
using Microsoft.AspNetCore.Mvc;
using ReadItLater.Core.Infrastructure;
using ReadItLater.Core.Web;
using FluentValidation.AspNetCore;

namespace Core.Api.Actions
{
    public static class ApiConfigurationAction
    {
        public static IServiceCollection ConfigureApiBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.SelectMany(v => v.Value.Errors.Select(e => new DefaultResultError(v.Key, e.ErrorMessage)));

                    return new BadRequestObjectResult(new DefaultFailedResult(Result.Failure(errors)));
                };
            });

            return services;
        }

        public static IMvcBuilder AddCustomJsonConfiguration(this IMvcBuilder mvcBuilder) =>
            mvcBuilder
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

        public static IMvcBuilder AddDtoValidation(this IMvcBuilder mvcBuilder)
        {
            var configuration = mvcBuilder.Services.GetConfiguration();
            var assemblyName = configuration
                .GetSection(AssembliesConfiguration.Section)
                .GetValue<string>(nameof(AssembliesConfiguration.Dtos));

            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(AssembliesConfiguration.Section));

            var assembly = Assembly.Load(new AssemblyName(assemblyName));

            return mvcBuilder
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(assembly));
        }
    }
}

