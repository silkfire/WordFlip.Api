namespace Wordsmith.WordFlip.WebApi
{
    using Grace.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Singularity.Microsoft.DependencyInjection;

    using System.IO;

    public class App
    {
        public static void Main()
        {
            new HostBuilder().UseGrace()
                             .ConfigureWebHostDefaults(whb =>
                             {
                                 whb.UseKestrel()
                                    .UseIIS()
                                    .UseStartup<Startup>();
                             })
                             //.UseServiceProviderFactory(new SingularityServiceProviderFactory())
                             //.ConfigureLogging((ctx, logging) =>
                             //{
                             //    if (ctx.HostingEnvironment.IsDevelopment())
                             //    {
                             //        logging.AddDebug();
                             //    }
                             //})
                             .UseContentRoot(Directory.GetCurrentDirectory())
                             .Build()
                             .Run();
        }
    }
}
