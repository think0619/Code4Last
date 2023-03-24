using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MsgInnerWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region SeriLog Config
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build(); 
            string logPath = Path.Combine("logs", "log.log");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();  
            #endregion 
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<UDPWorker>();
                }).UseSerilog();
    }
}
