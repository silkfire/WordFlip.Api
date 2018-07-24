namespace Wordsmith.WordFlip.WebApi
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;


    public class App
    {
        public static void Main()
        {
            CreateWebHostBuilder().Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder() => WebHost.CreateDefaultBuilder()
                                                                       .UseStartup<Startup>();
    }
}
