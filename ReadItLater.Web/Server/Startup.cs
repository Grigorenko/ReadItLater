using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadItLater.BL;
using ReadItLater.Data.EF;
using ReadItLater.Data.EF.Interfaces;
using ReadItLater.Data.EF.Utils;

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
            services.AddDbConnection();
            services.AddCustomAuthentication();

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
            app.UseRequestResponseMiddleware(options =>
            {
                options.Request.ShowBody = true;
                options.Response.IsShowed = true;
                options.Response.ShowBody = false;
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
