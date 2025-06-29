using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using TMS.Infrastructure.Abstractions.Cache;

namespace TMS.Infrastructure.DataAccess.Contexts
{
    /// <summary>
    /// Implementation of a Redis context for working with StackExchange.Redis.
    /// Provides access to the Redis connection and database.
    /// </summary>
    public class RedisCacheContext : IRedisCacheContext
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly ILogger<RedisCacheContext> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheContext"/> class.
        /// </summary>
        /// <param name="redis">The Redis connection multiplexer.</param>
        /// <param name="logger">The logger for logging Redis context events.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="redis"/> or <paramref name="logger"/> is <c>null</c>.</exception>
        public RedisCacheContext(IConnectionMultiplexer redis, ILogger<RedisCacheContext> logger)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
            _database = _redis.GetDatabase();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<string> GetStringAsync(string key)
        {
            try
            {
                var result = await _database.StringGetAsync(key);
                if (result.HasValue)
                {
                    _logger.LogDebug("String retrieved from Redis. Key: {Key}", key);
                    return result.ToString();
                }
                else
                {
                    _logger.LogDebug("Key not found in Redis. Key: {Key}", key);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get string from Redis. Key: {Key}", key);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            try
            {
                await _database.StringSetAsync(key, value, expiry);
                _logger.LogDebug("String set in Redis. Key: {Key}, Expiry: {Expiry}", key, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set string in Redis. Key: {Key}", key);
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> GetManyStringAsync(IEnumerable<string> keys)
        {
            try
            {
                var redisKeys = keys.Select(key => (RedisKey)key).ToArray();
                var values = await _database.StringGetAsync(redisKeys);

                var result = new Dictionary<string, string>();
                for (int i = 0; i < keys.Count(); i++)
                {
                    var key = keys.ElementAt(i);
                    var value = values[i];
                    if (value.HasValue)
                    {
                        result[key] = value.ToString();
                    }
                    else
                    {
                        result[key] = null;
                    }
                }

                _logger.LogDebug("Retrieved multiple strings from Redis. Keys: {Keys}", string.Join(", ", keys));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get multiple strings from Redis. Keys: {Keys}", string.Join(", ", keys));
                return [];
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                string serializedValue = await _database.StringGetAsync(key);
                if (string.IsNullOrEmpty(serializedValue))
                {
                    _logger.LogDebug("Key not found in Redis. Key: {Key}", key);
                    return default;
                }

                T value = JsonSerializer.Deserialize<T>(serializedValue);
                _logger.LogDebug("Object retrieved from Redis. Key: {Key}, Type: {Type}", key, typeof(T));
                return value;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize object from Redis. Key: {Key}, Type: {Type}", key, typeof(T));
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get object from Redis. Key: {Key}, Type: {Type}", key, typeof(T));
                return default;
            }
        }

        /// <inheritdoc/>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                string serializedValue = JsonSerializer.Serialize(value);
                await _database.StringSetAsync(key, serializedValue, expiry);
                _logger.LogDebug("Object set in Redis. Key: {Key}, Type: {Type}, Expiry: {Expiry}", key, typeof(T), expiry);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to serialize object to Redis. Key: {Key}, Type: {Type}", key, typeof(T));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set object in Redis. Key: {Key}, Type: {Type}", key, typeof(T));
            }
        }

        /// <inheritdoc/>
        public async Task<bool> KeyExistsAsync(string key)
        {
            try
            {
                bool exists = await _database.KeyExistsAsync(key);
                _logger.LogDebug("Key existence checked in Redis. Key: {Key}, Exists: {Exists}", key, exists);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check key existence in Redis. Key: {Key}", key);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                bool removed = await _database.KeyDeleteAsync(key);
                if (removed)
                {
                    _logger.LogDebug("Key removed from Redis. Key: {Key}", key);
                }
                else
                {
                    _logger.LogDebug("Key not found in Redis, remove operation skipped. Key: {Key}", key);
                }
                return removed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove key from Redis. Key: {Key}", key);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
        {
            try
            {
                bool expired = await _database.KeyExpireAsync(key, expiry);
                if (expired)
                {
                    _logger.LogDebug("Expiry set for key in Redis. Key: {Key}, Expiry: {Expiry}", key, expiry);
                }
                else
                {
                    _logger.LogDebug("Key not found in Redis, expiry operation skipped. Key: {Key}", key);
                }
                return expired;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set expiry for key in Redis. Key: {Key}", key);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task FlushAllAsync()
        {
            try
            {
                var endpoints = _redis.GetEndPoints();
                foreach (var endpoint in endpoints)
                {
                    var server = _redis.GetServer(endpoint);
                    await server.FlushAllDatabasesAsync();
                }
                _logger.LogInformation("All Redis databases flushed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to flush all Redis databases");
            }
        }
    }
}