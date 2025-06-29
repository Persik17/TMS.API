namespace TMS.Application.Abstractions.Cache
{
    /// <summary>
    /// High-level interface for working with the cache.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets an object from the cache by key asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve. Must be a class.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the cached object, or <c>null</c> if not found.</returns>
        Task<T> GetAsync<T>(string key) where T : class;

        /// <summary>
        /// Sets an object in the cache with a key asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to cache. Must be a class.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The object to cache.</param>
        /// <param name="expiry">Optional expiration time for the cache entry. If not specified, the cache entry will not expire.</param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class;

        /// <summary>
        /// Removes an object from the cache by key asynchronously.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>A task that represents the asynchronous remove operation.</returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Checks if a key exists in the cache asynchronously.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>A task that represents the asynchronous exists operation. The task result is <c>true</c> if the key exists; otherwise, <c>false</c>.</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Clears the entire cache asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous clear all operation.</returns>
        Task ClearAllAsync();
    }
}