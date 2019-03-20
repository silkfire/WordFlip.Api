namespace Wordsmith.WordFlip.WebApi
{
    using System.Data;
    using Models;

    using Data.Entities;
    using Data.Repositories;

    using Grace.AspNetCore.MVC;
    using Grace.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using System.Data.SqlClient;


    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(hostingEnvironment.ContentRootPath)
                                                      .AddJsonFile("appsettings.json")
                                                      .AddEnvironmentVariables()
                                                      .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddControllersAsServices()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);         // Enables the new [ApiController] attribute


            // Set up a configuration object for the API

            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
        }

        public void ConfigureContainer(IInjectionScope scope)
        {
            scope.SetupMvc();

            scope.Configure(c =>
            {
                c.ExportFactory(() => new SqlConnection(Configuration.GetConnectionString("DefaultConnection"))).As<IDbConnection>().Lifestyle.Singleton();

                c.ExportAs<WordFlipRepository, IWordFlipRepository<FlippedSentence>>().Lifestyle.Singleton();
            });
        }




        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseExceptionHandler("/error/500");
            }

            app.UseMvc();
        }
    }
}
