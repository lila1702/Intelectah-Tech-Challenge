using CarDealershipManager.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace CarDealershipManager.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IDistributedCache distributedCache, ILogger<CacheService> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            try
            {
                var cachedValue = await _distributedCache.GetStringAsync(key);

                if (cachedValue == null)
                {
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                    return null;
                }

                _logger.LogDebug("Cache hit for key: {Key}", key);
                return JsonSerializer.Deserialize<T>(cachedValue);
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogWarning(ex, "Redis connection failed for key {Key}. Returning null and continuing without cache.", key);
                return null;
            }
            catch (RedisTimeoutException ex)
            {
                _logger.LogWarning(ex, "Redis timeout for key {Key}. Returning null and continuing without cache.", key);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize cached value for key {Key}. Removing invalid cache entry.", key);

                // Tentar remover a entrada inválida do cache
                try
                {
                    await _distributedCache.RemoveAsync(key);
                }
                catch (Exception removeEx)
                {
                    _logger.LogWarning(removeEx, "Failed to remove invalid cache entry for key {Key}", key);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving cached value for key {Key}. Returning null.", key);
                return null;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            try
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

                _logger.LogDebug("Successfully cached value for key: {Key} with expiration: {Expiration}",
                    key, expiration?.ToString() ?? "60 minutes");
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogWarning(ex, "Redis connection failed when setting key {Key}. Continuing without caching.", key);
            }
            catch (RedisTimeoutException ex)
            {
                _logger.LogWarning(ex, "Redis timeout when setting key {Key}. Continuing without caching.", key);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to serialize value for key {Key}. Cannot cache this object.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error setting cached value for key {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
                _logger.LogDebug("Successfully removed cache entry for key: {Key}", key);
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogWarning(ex, "Redis connection failed when removing key {Key}. Key may still be cached.", key);
            }
            catch (RedisTimeoutException ex)
            {
                _logger.LogWarning(ex, "Redis timeout when removing key {Key}. Key may still be cached.", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error removing cached value for key {Key}", key);
            }
        }

        public async Task RemoveByPatternsAsync(string pattern)
        {
            try
            {
                // Implementação específica depende do provedor de cache distribuído.
                // Por enquanto, apenas registra que foi chamado
                _logger.LogInformation("RemoveByPatternsAsync called with pattern: {Pattern}. Implementation pending.", pattern);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RemoveByPatternsAsync for pattern {Pattern}", pattern);
            }
        }
    }
}