using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client.Services
{
    public static class ServiceCollectionExtensions
    {
        private const string DefaultHttpClientName = "ServerAPI";

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IWebAssemblyHostEnvironment env)
        {
            services.AddTransient<ValidateHeaderHandler>();
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(DefaultHttpClientName));
            services
                .AddHttpClient(DefaultHttpClientName, client => client.BaseAddress = new Uri(env.BaseAddress))
                .AddHttpMessageHandler<ValidateHeaderHandler>();

            services.AddHttpClient<RefHttpService>(DefaultHttpClientName);
            services.AddHttpClient<FolderHttpService>(DefaultHttpClientName);
            services.AddHttpClient<TagHttpService>(DefaultHttpClientName);
            services.AddHttpClient<AuthHttpService>(DefaultHttpClientName);

            return services;
        }
    }

    public class ValidateHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorage;

        public ValidateHeaderHandler(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Authorization"))
            {
                var token = await localStorage.GetItemAsStringAsync("token");

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
