using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class HealthExtensions
    {
        public static IServiceCollection AddAppHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddMySql(
                        connectionString: configuration.GetConnectionString("SqlCNN"),
                        name: "DB - RDS/MySqlServer",
                        timeout: TimeSpan.FromSeconds(60),
                        tags: new string[] { "ready" },
                        failureStatus: HealthStatus.Unhealthy);

            return services;
        }
    }
}
