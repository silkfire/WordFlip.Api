namespace Wordsmith.WordFlip.WebApi
{
    using Grace.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting;

    using System.IO;


    public class App
    {
        public static void Main()
        {
            new WebHostBuilder().UseGrace()
                                .UseKestrel()
                                .UseIISIntegration()
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup<Startup>()
                                .Build()
                                .Run();
        }
    }
}
