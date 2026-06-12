using Infrastructure;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System.Threading.RateLimiting;

namespace Extensions.DependencyInjection;

public static class ResilienceServiceExtensions
{
    public static IServiceCollection AddResilienceServices(
        this IServiceCollection services,
        AppConfiguration configuration)
    {
        var resilienceConfig = configuration.ResilienceConfig;

        services.AddResiliencePipeline(
            ResiliencePipelineNames.EmailProvider,
            builder =>
            {
                builder
                    .AddRateLimiter(new SlidingWindowRateLimiter(
                        new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = resilienceConfig.EmailRateLimitPerMinute,
                            QueueLimit = 0,
                            SegmentsPerWindow = 4,
                            Window = TimeSpan.FromMinutes(1)
                        }))
                    .AddTimeout(TimeSpan.FromSeconds(resilienceConfig.EmailTimeoutSeconds))
                    .AddRetry(CreateRetryOptions(resilienceConfig.EmailRetryCount))
                    .AddCircuitBreaker(CreateCircuitBreakerOptions(resilienceConfig.EmailCircuitBreakSeconds));
            });

        services.AddResiliencePipeline(
            ResiliencePipelineNames.R2Upload,
            builder =>
            {
                builder
                    .AddTimeout(TimeSpan.FromSeconds(resilienceConfig.R2UploadTimeoutSeconds))
                    .AddRetry(CreateRetryOptions(resilienceConfig.R2UploadRetryCount));
            });

        services.AddResiliencePipeline(
            ResiliencePipelineNames.R2Delete,
            builder =>
            {
                builder
                    .AddTimeout(TimeSpan.FromSeconds(resilienceConfig.R2DeleteTimeoutSeconds))
                    .AddRetry(CreateRetryOptions(resilienceConfig.R2DeleteRetryCount));
            });

        return services;
    }

    private static RetryStrategyOptions CreateRetryOptions(int retryCount)
    {
        return new RetryStrategyOptions
        {
            MaxRetryAttempts = retryCount,
            BackoffType = DelayBackoffType.Exponential,
            Delay = TimeSpan.FromSeconds(1),
            UseJitter = true,
            ShouldHandle = new PredicateBuilder()
                .Handle<Exception>(exception => exception is not OperationCanceledException)
        };
    }

    private static CircuitBreakerStrategyOptions CreateCircuitBreakerOptions(int breakSeconds)
    {
        return new CircuitBreakerStrategyOptions
        {
            FailureRatio = 0.5,
            MinimumThroughput = 5,
            SamplingDuration = TimeSpan.FromSeconds(30),
            BreakDuration = TimeSpan.FromSeconds(breakSeconds),
            ShouldHandle = new PredicateBuilder()
                .Handle<Exception>(exception => exception is not OperationCanceledException)
        };
    }
}
