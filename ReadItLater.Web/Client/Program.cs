using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ReadItLater.Web.Client.Services;
using System.Threading.Tasks;
using ReadItLater.Web.Client.Services.Auth;
using Blazored.LocalStorage;

namespace ReadItLater.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            ConfigureServices(builder.Services, builder.HostEnvironment);

            var host = builder.Build();

            await host.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment env)
        {
            services.AddSingleton<Context>();
            services.AddBlazoredLocalStorage();
            services.ConfigureHttpClients(env);

            services.AddAuthorizationCore();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<CustomAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<CustomAuthStateProvider>());
        }
    }
}
