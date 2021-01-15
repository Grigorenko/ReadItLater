using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReadItLater.BL;
using ReadItLater.Web.Client.Pages;
using ReadItLater.Web.Client.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReadItLater.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddSingleton<IHtmlParser, HtmlParser>();
            builder.Services.AddSingleton(typeof(Context));

            var host = builder.Build();

            //var c1 = host.Services.GetRequiredService<Menu>();

            await host.RunAsync();
        }
    }
}
