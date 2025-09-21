using CarDealershipManager.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CarDealershipManager.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var cachedValue = await _distributedCache.GetStringAsync(key);

            if (cachedValue == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.SetAbsoluteExpiration(expiration.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(60));
            }

            var serializedValue = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, serializedValue, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task RemoveByPatternsAsync(string pattern)
        {
            // Implementação específica depende do provedor de cache distribuído.
            // Implementar depois

            await Task.CompletedTask;
        }
    }
}
