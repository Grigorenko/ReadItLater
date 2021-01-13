using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadItLater.BL;
using ReadItLater.Data.EF;
using ReadItLater.Data.EF.Interfaces;
using ReadItLater.Data.EF.Options;
using ReadItLater.Data.EF.Utils;
using System;

namespace ReadItLater.Web.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var section = Configuration.GetSection(MsSqlDbConnection.DbConnectionSection);

            if (!section.Exists())
                throw new ArgumentNullException(nameof(MsSqlDbConnection.DbConnectionSection));

            var connection = new MsSqlDbConnection
            {
                ConnectionString = section.GetValue<string>(nameof(MsSqlDbConnection.Default))
            };

            services.AddSingleton<IDbConnection>(connection);

            services.AddScoped(typeof(IDapperContext<>), typeof(DapperWrapper<>));
            services.AddScoped<IDapperContext, DapperContext>();

            services.AddSingleton<IHtmlParser, HtmlParser>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
