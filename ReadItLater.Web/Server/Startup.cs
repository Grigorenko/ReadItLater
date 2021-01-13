using Core.Data.EFCore.Interfaces;
using Core.Data.EFCore.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadItLater.BL;
using ReadItLater.Data.EF.Interfaces;
using ReadItLater.Data.EF.Options;
using ReadItLater.Data.EF.Utils;
using System;
using System.Linq;

namespace ReadItLater.Web.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //Seed.Tags();
            //Seed.Folders();
            //Seed.Refs();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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

            //var assemblies = Configuration.GetSection(AssembliesConfiguration.AssembliesSection);

            //if (assemblies is null)
            //    throw new ArgumentNullException(nameof(AssembliesConfiguration.AssembliesSection));

            //services.Configure<AssembliesConfiguration>(options =>
            //{
            //    //options.Dtos = assemblies.GetValue<string>(nameof(AssembliesConfiguration.Dtos));
            //    options.Entities = assemblies.GetValue<string>(nameof(AssembliesConfiguration.Entities));
            //    //options.Infrastructure = assemblies.GetValue<string>(nameof(AssembliesConfiguration.Infrastructure));
            //});

            //services.AddScoped<IEfDbContext, EfDbContext>();
            //services.AddScoped(typeof(IEfDbContext<>), typeof(EfDbContext<>));
            services.AddScoped(typeof(IDapperContext<>), typeof(DapperWrapper<>));
            services.AddScoped<IDapperContext, DapperContext>();

            services.AddSingleton<IHtmlParser, HtmlParser>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
