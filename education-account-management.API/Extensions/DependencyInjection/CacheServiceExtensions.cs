using education_account_management.BLL;
using Infrastructure;
using Infrastructure.CacheServices;
using Infrastructure.Interface;
using StackExchange.Redis;

namespace Extensions.DependencyInjection;

public static class CacheServiceExtensions
{
    public static IServiceCollection AddCacheServices(
        this IServiceCollection services,
        AppConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.RedisConfig?.Host))
        {
            AddMemoryCacheServices(services);
            return services;
        }

        try
        {
            var redisConnectionString = BuildRedisConnectionString(configuration.RedisConfig);
            var redisOptions = ConfigurationOptions.Parse(redisConnectionString);
            redisOptions.AbortOnConnectFail = configuration.RedisConfig.AbortOnConnectFail;
            redisOptions.ConnectRetry = Math.Max(0, configuration.RedisConfig.ConnectRetry);
            redisOptions.ConnectTimeout = Math.Max(1, configuration.RedisConfig.ConnectTimeoutMs);
            redisOptions.SyncTimeout = Math.Max(1, configuration.RedisConfig.SyncTimeoutMs);
            redisOptions.ReconnectRetryPolicy = new ExponentialRetry(
                Math.Max(1, configuration.RedisConfig.ReconnectRetryMs));

            var multiplexer = ConnectionMultiplexer.Connect(redisOptions);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisOptions;
                options.InstanceName = configuration.RedisConfig.InstanceName ?? string.Empty;
            });

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<ICacheDataService, CacheDataService>();
            services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();

            return services;
        }
        catch
        {
            AddMemoryCacheServices(services);
            return services;
        }
    }

    private static void AddMemoryCacheServices(IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
        services.AddScoped<ICacheDataService, CacheDataService>();
        services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
    }

    private static string BuildRedisConnectionString(RedisConfig redisConfig)
    {
        return string.IsNullOrWhiteSpace(redisConfig.Password)
            ? redisConfig.Host
            : $"{redisConfig.Host},password={redisConfig.Password}";
    }
}
