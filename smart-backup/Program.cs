using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SmartBackup.service;
using System.IO;

namespace SmartBackup
{
    public class Program
    {
        private static string _pathAppSettings = Directory.GetCurrentDirectory()+ "/Config/appsettings.json";

        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                Business app = serviceProvider.GetService<Business>();
                 // Start up logic here

                if (File.Exists(_pathAppSettings))
                {
                    app.Run();
                }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logFatal.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                    Log.Fatal("FATAL! non esiste appsettings.json in /Config");
                }
            }
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
            .AddTransient<Business>();
        }
    }
}
