namespace Wordsmith.WordFlip.WebApi
{
    using Grace.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;


    public class App
    {
        public static void Main()
        {
            new HostBuilder().UseGrace()
                             .ConfigureWebHostDefaults(wb =>
                             {
                                 wb.UseKestrel()
                                   .UseIIS()
                                   .UseStartup<Startup>();
                             })
                             .ConfigureAppConfiguration(cb =>
                             {
                                 cb.AddJsonFile("appsettings.json")
                                   .AddEnvironmentVariables();
                             })
                             .ConfigureLogging((ctx, logging) =>
                             {
#if DEBUG
                                 logging.AddDebug();
#else
                                 logging.AddConsole();
#endif
                             })
                             .Build()
                             .Run();
        }
    }
}
