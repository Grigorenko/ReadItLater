using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadItLater.Core.Web;
using ReadItLater.Core.Web.Options;

namespace ReadItLater.Core.Api.Actions
{
    public static class AddSwaggerConfigurationAction
    {
        //public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        //{
        //    var configuration = services.GetConfiguration();
        //    var section = configuration.GetSection(SwaggerConfiguration.Section);

        //    if (section is null)
        //        throw new ArgumentNullException(nameof(SwaggerConfiguration.Section));

        //    services.AddSwaggerDocument(config =>
        //    {
        //        config.PostProcess = document =>
        //        {
        //            document.Info.Title = section.GetValue<string?>(nameof(SwaggerConfiguration.Title));
        //            document.Info.Version = section.GetValue<string?>(nameof(SwaggerConfiguration.Version));
        //        };
        //    });

        //    return services;
        //}
    }
}
