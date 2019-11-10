namespace Wordsmith.WordFlip.WebApi
{
    using Models;
    using Utils;

    using Data.Entities;
    using Data.Repositories;

    using Grace.AspNetCore.MVC;
    using Grace.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SpanJson.AspNetCore.Formatter;
    using Singularity;
    using Singularity.Microsoft.DependencyInjection;

    using System;
    using System.Data.SqlClient;
    using System.Net;
    using System.Threading.Tasks;


    public class Startup
    {
        private readonly IHostEnvironment _hostingEnvironment;

        public Startup(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            Configuration = new ConfigurationBuilder().SetBasePath(_hostingEnvironment.ContentRootPath)
                                                      .AddJsonFile("appsettings.json")
                                                      .AddEnvironmentVariables()
                                                      .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    //.AddControllersAsServices()
                    .AddSpanJson()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);         // Enables the new [ApiController] attribute


            // Set up a configuration object for the API

            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
        }

        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    builder.Register<IWordFlipRepository<FlippedSentence>>(_ => _.Inject(() => new WordFlipRepository(new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))).With(Lifetimes.PerContainer));
        //}


        public void ConfigureContainer(IInjectionScope scope)
        {
            scope.SetupMvc();

            scope.Configure(c =>
            {
                c.ExportFactory(() => new WordFlipRepository(new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))).As<IWordFlipRepository<FlippedSentence>>().Lifestyle.Singleton();
            });
        }




        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            if (_hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use(async (ctx, next) =>
                {
                    try
                    {
                        await next();
                    }
                    catch (Exception e)
                    {
                        // TODO: Log error

                        ctx.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                        await ctx.RespondWithJsonError("An unexpected error occurred.");
                    }
                });
            }

            app.Use(async (ctx, next) =>
            {
                var matchedEndpoint = ctx.GetEndpoint();

                if (matchedEndpoint == null || matchedEndpoint.DisplayName == "405 HTTP Method Not Supported")
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    ctx.Response.OnStarting(() =>
                    {
                        ctx.Response.Headers.Remove("Allow");

                        return Task.CompletedTask;
                    });

                    await ctx.RespondWithJsonError("Invalid API method.");
                }
                else
                {
                    await next();
                }
            });

            app.UseEndpoints(erb => erb.MapControllers());
        }
    }
}
