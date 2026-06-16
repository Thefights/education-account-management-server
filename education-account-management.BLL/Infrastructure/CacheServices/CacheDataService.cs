using Infrastructure.Interface;
using System.Text.Json;

namespace Infrastructure.CacheServices
{
    public class CacheDataService(
        ICacheService cacheService,
        ILogger<CacheDataService> logger)
        : ICacheDataService
    {
        private readonly ICacheService _cacheService = cacheService;
        private readonly ILogger<CacheDataService> _logger = logger;

        public async Task<T> GetOrSetAsync<T>(
            string key,
            TimeSpan ttl,
            Func<CancellationToken, Task<T>> factory,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var cachedValue = await _cacheService.GetAsync(key);
                if (!string.IsNullOrWhiteSpace(cachedValue))
                {
                    var cachedResult = JsonSerializer.Deserialize<T>(cachedValue);
                    if (cachedResult != null)
                    {
                        return cachedResult;
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to read cache key {CacheKey}.", key);
            }

            var value = await factory(cancellationToken);
            if (value == null)
            {
                return value!;
            }

            try
            {
                await _cacheService.SetAsync(key, JsonSerializer.Serialize(value), ttl);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to write cache key {CacheKey}.", key);
            }

            return value;
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cacheService.DeleteAsync(key);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to remove cache key {CacheKey}.", key);
            }
        }
    }
}
