using Microsoft.AspNetCore.Builder;
using ReadItLater.Web.Server.Utils;
using System;

namespace ReadItLater.Web.Server
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestResponseMiddleware(this IApplicationBuilder app, Action<RequestResponseOptions> options = null)
        {
            var newOptions = new RequestResponseOptions();
            options?.Invoke(newOptions);

            app.UseMiddleware<RequestResponseMiddleware>(newOptions);

            return app;
        }
    }
}
