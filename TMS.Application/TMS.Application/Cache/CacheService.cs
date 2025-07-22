using Microsoft.Extensions.Logging;
using System.Text.Json;
using TMS.Application.Abstractions.Cache;
using TMS.Infrastructure.Abstractions.Cache;

namespace TMS.Application.Cache
{
    /// <summary>
    /// Service for interacting with Redis cache storage.
    /// Provides methods for storing, retrieving, checking existence, removing, and clearing cached data.
    /// Handles serialization and deserialization of objects to and from JSON.
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IRedisCacheContext _cacheContext;
        private readonly ILogger<CacheService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="cacheContext">The Redis cache context for low-level cache operations.</param>
        /// <param name="logger">The logger for logging cache events and errors.</param>
        public CacheService(
            IRedisCacheContext cacheContext,
            ILogger<CacheService> logger)
        {
            _cacheContext = cacheContext ?? throw new ArgumentNullException(nameof(cacheContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false
            };
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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