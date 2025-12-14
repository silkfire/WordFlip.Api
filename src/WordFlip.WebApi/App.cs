using Wordsmith.WordFlip.WebApi;
using Wordsmith.WordFlip.WebApi.Endpoints;
using Wordsmith.WordFlip.WebApi.Extensions;
using Wordsmith.WordFlip.WebApi.Services;

using Wordsmith.WordFlip.Domain.AggregatesModel.FlippedSentenceAggregate;

using Wordsmith.WordFlip.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Settings.Configuration;

using System;
using System.Net;
using System.Threading.Tasks;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<Configuration>(builder.Configuration.GetRequiredSection(nameof(Configuration)));

    builder.Services.AddScoped<IFlippedSentenceRepository>(sp => new FlippedSentenceRepository(new SqlConnection(sp.GetRequiredService<IConfiguration>().GetRequiredSection("ConnectionStrings:DefaultConnection").Value)))
                    .AddScoped<FlipSentenceService>();

    builder.WebHost.ConfigureKestrel(o => o.AddServerHeader = false);

    string? corsOrigin;

    if (builder.Environment.IsDevelopment())
    {
        // TODO: Change to https when https://github.com/oven-sh/bun/issues/14825 is resolved

        corsOrigin = "http://localhost:3000";
    }
    else
    {
        corsOrigin = builder.Configuration.GetRequiredSection("CorsOrigin").Value;
    }

    var defaultCorsPolicy = new CorsPolicyBuilder().WithOrigins(corsOrigin!)
                                                   .AllowAnyHeader()
                                                   .WithMethods(HttpMethods.Get, HttpMethods.Post)
                                                   .Build();

    builder.Services.AddCors(o =>
                            {
                                o.AddDefaultPolicy(defaultCorsPolicy);
                            });

    builder.Services.AddSerilog((sp, lc) =>
    {
        SelfLog.Enable(l => Console.WriteLine($"[Serilog SelfLog] {l}"));

        var configuration = sp.GetRequiredService<IConfiguration>();

        lc.ReadFrom.Configuration(configuration.GetRequiredSection(ConfigurationLoggerConfigurationExtensions.DefaultSectionName),
                                  new ConfigurationReaderOptions
                                  {
                                      SectionName = null
                                  });
    });

    var app = builder.Build();

    app.UsePathBase("/api");

    app.UseCors();

    if (app.Environment.IsDevelopment())
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
                var loggerFactory = ctx.RequestServices.GetRequiredService<ILoggerFactory>();

                loggerFactory.CreateLogger("WordFlip.Api").LogError(e, "An unhandled exception occurred");

                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await ctx.RespondWithJsonError("An unexpected error occurred");
            }
        });
    }

    app.Use(async (ctx, next) =>
    {
        if (HttpMethods.IsOptions(ctx.Request.Method))
        {
            // Preflight requests should not be treated as invalid API methods
            ctx.Response.StatusCode = StatusCodes.Status204NoContent;

            return;
        }

        var matchedEndpoint = ctx.GetEndpoint();

        if (matchedEndpoint == null || matchedEndpoint.DisplayName == "405 HTTP Method Not Supported")
        {
            ctx.Response.StatusCode = (int)HttpStatusCode.NotFound;
            ctx.Response.OnStarting(() =>
            {
                ctx.Response.Headers.Remove("Allow");

                return Task.CompletedTask;
            });

            await ctx.RespondWithJsonError("Invalid API method");
        }
        else
        {
            await next();
        }
    });

    app.RegisterFlipEndpoints();

    await app.RunAsync();
}
catch (Exception e)
{
    const string standardOutputTemplate = "{SourceContext} [{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    var logger = new LoggerConfiguration().WriteTo.Debug(outputTemplate: standardOutputTemplate)
                                          .WriteTo.Console(outputTemplate: standardOutputTemplate)
                                          .CreateLogger()
                                          .ForContext(Constants.SourceContextPropertyName, "WordFlip.Api");

    logger.Fatal(e, "An unhandled exception occurred during startup of the application");
}
finally
{
    await Log.CloseAndFlushAsync();
}
