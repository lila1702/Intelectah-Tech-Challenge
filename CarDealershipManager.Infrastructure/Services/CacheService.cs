using CarDealershipManager.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CarDealershipManager.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CacheService> _logger;
        private static bool _isRedisAvailable = true;
        private static DateTime _lastFailureTime = DateTime.MinValue;
        private static readonly TimeSpan _retryInterval = TimeSpan.FromMinutes(2);

        public CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            if (!ShouldTryRedis()) return null;

            try
            {
                var cachedValue = await _distributedCache.GetStringAsync(key);

                if (string.IsNullOrEmpty(cachedValue))
                    return null;

                MarkRedisAsAvailable();
                return JsonSerializer.Deserialize<T>(cachedValue);
            }
            catch (Exception ex)
            {
                MarkRedisAsUnavailable();
                _logger.LogWarning(ex, "Cache failed for key {Key}, continuing without cache", key);
                return null;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            if (!ShouldTryRedis()) return;

            try
            {
                var options = new DistributedCacheEntryOptions();
                options.SetAbsoluteExpiration(expiration ?? TimeSpan.FromMinutes(60));

                var serializedValue = JsonSerializer.Serialize(value);
                await _distributedCache.SetStringAsync(key, serializedValue, options);

                MarkRedisAsAvailable();
            }
            catch (Exception ex)
            {
                MarkRedisAsUnavailable();
                _logger.LogWarning(ex, "Failed to cache key {Key}, continuing without caching", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            if (!ShouldTryRedis()) return;

            try
            {
                await _distributedCache.RemoveAsync(key);
                MarkRedisAsAvailable();
            }
            catch (Exception ex)
            {
                MarkRedisAsUnavailable();
                _logger.LogWarning(ex, "Failed to remove key {Key}", key);
            }
        }

        public async Task RemoveByPatternsAsync(string pattern)
        {
            // Implementação simples - apenas loga
            _logger.LogInformation("RemoveByPatternsAsync called with pattern: {Pattern}", pattern);
            await Task.CompletedTask;
        }

        private static bool ShouldTryRedis()
        {
            if (_isRedisAvailable) return true;

            // Tenta novamente após o intervalo de retry
            if (DateTime.UtcNow - _lastFailureTime > _retryInterval)
            {
                _isRedisAvailable = true;
                return true;
            }

            return false;
        }

        private static void MarkRedisAsUnavailable()
        {
            _isRedisAvailable = false;
            _lastFailureTime = DateTime.UtcNow;
        }

        private static void MarkRedisAsAvailable()
        {
            _isRedisAvailable = true;
        }
    }
}