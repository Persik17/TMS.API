namespace TMS.Infrastructure.Abstractions.Cache
{
    public interface IRedisCacheContext
    {
        /// <summary>
        /// Gets a string value by key asynchronously.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the string value, or <c>null</c> if not found.</returns>
        Task<string> GetStringAsync(string key);

        /// <summary>
        /// Sets a string value by key asynchronously.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The string value to set.</param>
        /// <param name="expiry">Optional expiration time for the cache entry. If not specified, the cache entry will not expire.</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        Task SetStringAsync(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Gets multiple string values by a list of keys asynchronously.
        /// </summary>
        /// <param name="keys">A collection of cache keys.</param>
        /// <returns>A task that represents the asynchronous get many operation. The task result contains a dictionary of key-value pairs, where the keys are the provided keys and the values are the corresponding string values from the cache. If a key is not found, its corresponding value will be <c>null</c>.</returns>
        Task<Dictionary<string, string>> GetManyStringAsync(IEnumerable<string> keys);

        /// <summary>
        /// Gets a value by key asynchronously, deserializing it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the cached value to.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the deserialized value, or the default value of type <typeparamref name="T"/> if not found.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Sets a value by key asynchronously, serializing it.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize and cache.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The value to serialize and cache.</param>
        /// <param name="expiry">Optional expiration time for the cache entry. If not specified, the cache entry will not expire.</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// Checks if a key exists asynchronously.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>A task that represents the asynchronous key exists operation. The task result is <c>true</c> if the key exists; otherwise, <c>false</c>.</returns>
        Task<bool> KeyExistsAsync(string key);

        /// <summary>
        /// Removes a value by key asynchronously.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>A task that represents the asynchronous remove operation. The task result is <c>true</c> if the key was successfully removed; otherwise, <c>false</c>.</returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Sets the expiration time for a key asynchronously.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="expiry">The expiration time to set.</param>
        /// <returns>A task that represents the asynchronous set expiry operation. The task result is <c>true</c> if the expiration time was successfully set; otherwise, <c>false</c>.</returns>
        Task<bool> SetExpiryAsync(string key, TimeSpan expiry);

        /// <summary>
        /// Clears all caches asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous flush all operation.</returns>
        Task FlushAllAsync();
    }
}