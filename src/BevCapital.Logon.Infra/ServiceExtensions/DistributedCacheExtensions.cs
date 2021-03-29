using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class DistributedCacheExtensions
    {
        public static IServiceCollection AddAppDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheCNN = configuration.GetConnectionString("CacheCNN");
            if (string.IsNullOrWhiteSpace(cacheCNN))
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                var cacheEndpoint = Environment.GetEnvironmentVariable("CACHE_ENDPOINT");
                var cachePassword = Environment.GetEnvironmentVariable("CACHE_PASSWORD");
                cacheCNN = cacheCNN.Replace("CACHE_ENDPOINT", cacheEndpoint)
                                   .Replace("CACHE_PASSWORD", cachePassword);

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
