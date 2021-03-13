using Core.Api.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadItLater.Core.Api.Actions;
using ReadItLater.Core.Infrastructure.Utils;
using ReadItLater.HtmlParser;
using ReadItLater.Web.Server.Utils.Auth;

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
            services
                .AddMsSqlDbConnection()
                .AddAssembliesConfiguration()
                .AddDapperRepositoryProvider();
            services
                .AddMessages()
                .AddValidators()
                .AddQueryHandlers()
                .AddCommandHandlers();
            services
                //.AddSwaggerConfiguration()
                .ConfigureApiBehavior()
                .AddControllers()
                .AddCustomJsonConfiguration()
                .AddDtoValidation();

            services
                .AddCustomAuthentication()
                .AddScoped<IUserProvider, UserProvider>();

            services.AddSingleton<IHtmlParser, HtmlParser.HtmlParser>();
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
                options.Request.ShowHeaders = true;
                options.Request.ShowBody = false;

                options.Response.IsShowed = true;
                options.Response.ShowHeaders = false;
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
