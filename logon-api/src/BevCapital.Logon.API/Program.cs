using BevCapital.Logon.Infra.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BevCapital.Logon.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog
                (
                    (hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .AppendConsoleLogger()
                            .AppendAwsCloudwatchLogger("log-aws", hostingContext.HostingEnvironment.EnvironmentName, Serilog.Events.LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .Enrich.WithMachineName()
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning);
                    }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseIISIntegration();
                    webBuilder.UseKestrel(x => x.AddServerHeader = false);
                    webBuilder.UseStartup<Startup>();
                });
    }
}
