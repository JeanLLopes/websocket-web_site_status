using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace WebSiteStatus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ADD SERILOG CONFIGURATION
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Users\Jean\AppData\Local\Temp\LogFile.txt")
                .CreateLogger();

            //ADD LOG IN APPLICATION
            try
            {
                Log.Information("Starting applcation");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Theres error when start application");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            } 
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                }).UseSerilog();
        }
            
    }
}
