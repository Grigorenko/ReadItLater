using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReadItLater.Web.Client.Services.Auth;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ReadItLater.Web.Client.Services
{
    public static class ServiceCollectionExtensions
    {
        private const string DefaultHttpClientName = "ServerAPI";

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IWebAssemblyHostEnvironment env)
        {
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultHttpClientName));
            services.AddHttpClient(DefaultHttpClientName, (serviceProvider, client) =>
            {
                var tokenService = serviceProvider.GetRequiredService<UserToken>();

                if (tokenService is null)
                    throw new Exception($"{nameof(UserToken)} service must be registered before HttpClient service.");

                client.BaseAddress = new Uri(env.BaseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            });

            services.AddHttpClient<RefHttpService>(DefaultHttpClientName);
            services.AddHttpClient<FolderHttpService>(DefaultHttpClientName);
            services.AddHttpClient<TagHttpService>(DefaultHttpClientName);
            services.AddHttpClient<AuthHttpService>(DefaultHttpClientName);

            return services;
        }
    }
}
