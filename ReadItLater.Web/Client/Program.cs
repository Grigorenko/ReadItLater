using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReadItLater.Web.Client.Services;
using ReadItLater.Web.Client.Services.Http;
using System;
using System.Threading.Tasks;

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
            services.AddSingleton(typeof(Context));
            services.AddHttpClient<RefHttpService>(c => c.BaseAddress = new Uri(env.BaseAddress));
            services.AddHttpClient<FolderHttpService>(c => c.BaseAddress = new Uri(env.BaseAddress));
        }
    }
}
