using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class DistributedCacheExtensions
    {
        public static IServiceCollection ConfigureDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheCNN = configuration.GetConnectionString("CacheCNN");
            if (string.IsNullOrWhiteSpace(cacheCNN))
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = cacheCNN;
                    options.InstanceName = "Logon:";
                });
            }

            return services;
        }
    }
}
