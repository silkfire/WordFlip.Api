namespace Wordsmith.WordFlip.WebApi
{
    using Data.Repositories;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StructureMap;

    using System;
    using System.Data;
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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddControllersAsServices()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);         // Enables the new [ApiController] attribute

            return ConfigureIoC(services);
        }


        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container(_ =>
            {
                _.For<IDbConnection>()
                 .Use(() => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                 .ContainerScoped();

                _.For(typeof(IWordFlipRepository<>))
                 .Use(typeof(WordFlipRepository))
                 .ContainerScoped();


                _.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable CORS to allow requests from client
            // TODO: Add stricter CORS policy
            app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());



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
