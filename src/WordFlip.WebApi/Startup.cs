namespace Wordsmith.WordFlip.WebApi
{
    using Domain.AggregatesModel.FlippedSentenceAggregate;
    using Extensions;
    using Infrastructure.Repositories;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Services.SentenceFlipping;
    using SpanJson.AspNetCore.Formatter;

    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class Startup
    {
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddSpanJson();


            // Set up a configuration object for the API
            services.Configure<Configuration>(_configuration.GetSection(nameof(Configuration)));

            services.AddScoped<IFlippedSentenceRepository>(sp => new FlippedSentenceRepository(new SqlConnection(sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"))))
                    .AddScoped<FlipSentenceService>()
                    .AddScoped<GetLastFlippedSentencesService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
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
                        loggerFactory.CreateLogger("WordFlip.Api").LogError(e, "An unhandled exception occurred.");

                        ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

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
