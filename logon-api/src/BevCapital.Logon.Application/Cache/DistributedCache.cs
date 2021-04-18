using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application
{
    public static class DistributedCache
    {
        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            await distributedCache.SetStringAsync(key, jsonValue, options, cancellationToken);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken cancellationToken = default) where T : class
        {
            var value = await distributedCache.GetStringAsync(key, cancellationToken);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static async Task<bool> ExistObjectAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken cancellationToken = default) where T : class
        {
            var value = await distributedCache.GetStringAsync(key, cancellationToken);
            return value != null;
        }
    }
}
