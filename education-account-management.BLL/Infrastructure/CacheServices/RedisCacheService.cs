using Infrastructure.Interface;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text;

namespace Infrastructure.CacheServices
{
    public class RedisCacheService(
        IDistributedCache distributedCache,
        IConnectionMultiplexer connectionMultiplexer)
        : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }

        public async Task<string?> GetAsync(string key)
        {
            var bytes = await _distributedCache.GetAsync(key);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        public async Task DeleteAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var value = await GetAsync(key);
            return value != null;
        }

        public async Task<long> IncrementAsync(string key)
        {
            return await _database.StringIncrementAsync(key);
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan expiry)
        {
            return await _database.KeyExpireAsync(key, expiry);
        }
    }
}
