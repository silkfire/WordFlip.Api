namespace Wordsmith.WordFlip.WebApi;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class App
{
    public static void Main()
    {
        new HostBuilder().ConfigureWebHostDefaults(wb =>
                         {
                             wb.UseKestrel(kso => kso.AddServerHeader = false)
                               .UseStartup<Startup>();
                         })
                         .ConfigureAppConfiguration((ctx, cb) =>
                         {
                             cb.AddJsonFile( "appsettings.json")
                               .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true)
                               .AddEnvironmentVariables();
                         })
                         .ConfigureLogging((ctx, logging) =>
                         {
                             logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));

                             logging.AddConsole();
                         })
                         .Build()
                         .Run();
    }
}
