using System.Text.Json;
using Microsoft.Extensions.Logging;
using TMS.Abstractions.Interfaces.Cache;

namespace TMS.Infrastructure.Cache.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с Redis кэшем
    /// </summary>
    public class RedisCacheRepository : ICacheRepository
    {
        private readonly IRedisCacheContext _cacheContext;
        private readonly ILogger<RedisCacheRepository> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisCacheRepository(
            IRedisCacheContext cacheContext,
            ILogger<RedisCacheRepository> logger)
        {
            _cacheContext = cacheContext ?? throw new ArgumentNullException(nameof(cacheContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            try
            {
                var json = await _cacheContext.GetStringAsync(key);
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize object from Redis. Key: {Key}, Type: {Type}", key, typeof(T).Name);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving data from Redis cache. Key: {Key}", key);
                return null;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            try
            {
                var json = JsonSerializer.Serialize(value, _jsonOptions);
                await _cacheContext.SetStringAsync(key, json, expiry);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to serialize object for Redis. Key: {Key}, Type: {Type}", key, typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting data to Redis cache. Key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cacheContext.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing data from Redis cache. Key: {Key}", key);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _cacheContext.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking key existence in Redis cache. Key: {Key}", key);
                return false;
            }
        }

        public async Task ClearAllAsync()
        {
            try
            {
                await _cacheContext.FlushAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing all Redis cache");
            }
        }
    }
}