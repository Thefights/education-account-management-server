using Amazon.Runtime;
using Amazon.S3;
using Emails;
using Infrastructure;
using Infrastructure.CacheServices;
using Infrastructure.Interface;
using Interfaces.Admin;
using Interfaces.AiAssistantSettings;
using Interfaces.Audit;
using Interfaces.Auth;
using Interfaces.Base;
using Interfaces.BatchReport;
using Interfaces.Courses;
using Interfaces.Csv;
using Interfaces.EducationAccounts;
using Interfaces.Email;
using Interfaces.Maintenance;
using Interfaces.Schools;
using Interfaces.Storage;
using Interfaces.TopUp;
using Interfaces.TransactionHistory;
using Mappers;
using Mappers.Admin;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Repositories.Interfaces;
using Resend;
using Services.Admin;
using Services.AiAssistantSettings;
using Services.Audit;
using Services.Auth;
using Services.Base;
using Services.BatchReport;
using Services.Courses;
using Services.EducationAccounts;
using Services.Email;
using Services.Maintenance;
using Services.Schools;
using Services.Storage;
using Services.TopUp;
using Services.TransactionHistory;
using StackExchange.Redis;
using System.Threading.RateLimiting;

namespace Extensions.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        AppConfiguration configuration)
    {
        AddResilienceServices(services, configuration);
        AddCacheServices(services, configuration);
        AddAuthServices(services);
        AddEmailServices(services, configuration);
        AddStorageServices(services, configuration);

        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IAuditLogWriter, AuditLogWriter>();

        services.AddScoped<ICsvExportService, CsvExportService>();
        services.AddScoped(typeof(CsvImportService<,>));

        services.AddScoped<IEducationAccountService, EducationAccountService>();
        services.AddScoped<IEducationAccountImportService, EducationAccountImportService>();
        services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();

        services.AddScoped<ITopupRuleService, TopupRuleService>();
        services.AddScoped<ITopupScheduleService, TopupScheduleService>();
        services.AddScoped<ITopupService, TopupService>();
        services.AddScoped<ITopupBackgroundService, TopupBackgroundService>();

        services.AddScoped<IBatchReportService, BatchReportService>();
        services.AddScoped<IAiAssistantSettingService, AiAssistantSettingService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ISchoolService, SchoolService>();

        services.AddScoped<IDataCleanupService, DataCleanupService>();
        services.AddHostedService<DataCleanupWorker>();
        services.AddHostedService<AccountProvisioningWorker>();
        services.AddHostedService<TopupDailyWorker>();

        services.AddScoped<AuditLogMapper>();
        services.AddScoped<EducationAccountMapper>();
        services.AddScoped<TransactionHistoryMapper>();
        services.AddScoped<TopupRuleMapper>();
        services.AddScoped<TopupScheduleMapper>();
        services.AddScoped<AiAssistantSettingMapper>();
        services.AddScoped<AdminMapper>();
        services.AddScoped<CourseMapper>();
        services.AddScoped<SchoolMapper>();

        return services;
    }

    private static void AddAuthServices(IServiceCollection services)
    {
        services.AddScoped<CurrentUserService>();
        services.AddScoped<ICurrentUserService>(provider =>
            provider.GetRequiredService<CurrentUserService>());
        services.AddScoped<IAuditUserContext>(provider =>
            provider.GetRequiredService<CurrentUserService>());
        services.AddScoped<ICurrentTokenService, CurrentTokenService>();
        services.AddScoped<IRefreshTokenCookieService, RefreshTokenCookieService>();
        services.AddScoped<IAuthService, AuthService>();
    }

    private static void AddCacheServices(
        IServiceCollection services,
        AppConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.RedisConfig?.Host))
        {
            AddMemoryCacheServices(services);
            return;
        }

        try
        {
            var redisConnectionString = string.IsNullOrWhiteSpace(configuration.RedisConfig.Password)
                ? configuration.RedisConfig.Host
                : $"{configuration.RedisConfig.Host},password={configuration.RedisConfig.Password}";
            var redisOptions = ConfigurationOptions.Parse(redisConnectionString);
            redisOptions.AbortOnConnectFail = configuration.RedisConfig.AbortOnConnectFail;
            redisOptions.ConnectRetry = Math.Max(0, configuration.RedisConfig.ConnectRetry);
            redisOptions.ConnectTimeout = Math.Max(1, configuration.RedisConfig.ConnectTimeoutMs);
            redisOptions.SyncTimeout = Math.Max(1, configuration.RedisConfig.SyncTimeoutMs);
            redisOptions.ReconnectRetryPolicy = new ExponentialRetry(
                Math.Max(1, configuration.RedisConfig.ReconnectRetryMs));

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisOptions));
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisOptions;
                options.InstanceName = configuration.RedisConfig.InstanceName ?? string.Empty;
            });
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<ICacheDataService, CacheDataService>();
            services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
        }
        catch
        {
            AddMemoryCacheServices(services);
        }
    }

    private static void AddMemoryCacheServices(IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
        services.AddScoped<ICacheDataService, CacheDataService>();
        services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
    }

    private static void AddEmailServices(
        IServiceCollection services,
        AppConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<IOutboxMessageProcessor, OutboxMessageProcessor>();
        services.AddHostedService<OutboxEmailWorker>();
        services.AddScoped<EmailTemplateBuilder>();
        services.AddResend(options => options.ApiToken = configuration.EmailConfig.ApiKey);
    }

    private static void AddStorageServices(
        IServiceCollection services,
        AppConfiguration configuration)
    {
        services.AddScoped<IUploadService, UploadService>();
        services.AddScoped<IStorageService, R2StorageService>();
        services.AddScoped<IFileValidator, FileValidator>();
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = $"https://{configuration.R2Config.AccountId}.r2.cloudflarestorage.com",
                ForcePathStyle = true,
                SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA256,
                MaxErrorRetry = 3,
                Timeout = TimeSpan.FromMinutes(5),
                AuthenticationRegion = "auto",
                BufferSize = 65536,
                DisableLogging = false,
                UseHttp = false
            };
            var credentials = new BasicAWSCredentials(
                configuration.R2Config.AccessKeyId,
                configuration.R2Config.SecretAccessKey);
            return new AmazonS3Client(credentials, config);
        });
    }

    private static void AddResilienceServices(
        IServiceCollection services,
        AppConfiguration configuration)
    {
        var resilienceConfig = configuration.ResilienceConfig;
        services.AddResiliencePipeline(
            ResiliencePipelineNames.EmailProvider,
            builder => builder
                .AddRateLimiter(new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = resilienceConfig.EmailRateLimitPerMinute,
                    QueueLimit = 0,
                    SegmentsPerWindow = 4,
                    Window = TimeSpan.FromMinutes(1)
                }))
                .AddTimeout(TimeSpan.FromSeconds(resilienceConfig.EmailTimeoutSeconds))
                .AddRetry(CreateRetryOptions(resilienceConfig.EmailRetryCount))
                .AddCircuitBreaker(CreateCircuitBreakerOptions(
                    resilienceConfig.EmailCircuitBreakSeconds)));

        services.AddResiliencePipeline(
            ResiliencePipelineNames.R2Upload,
            builder => builder
                .AddTimeout(TimeSpan.FromSeconds(resilienceConfig.R2UploadTimeoutSeconds))
                .AddRetry(CreateRetryOptions(resilienceConfig.R2UploadRetryCount)));

        services.AddResiliencePipeline(
            ResiliencePipelineNames.R2Delete,
            builder => builder
                .AddTimeout(TimeSpan.FromSeconds(resilienceConfig.R2DeleteTimeoutSeconds))
                .AddRetry(CreateRetryOptions(resilienceConfig.R2DeleteRetryCount)));
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
