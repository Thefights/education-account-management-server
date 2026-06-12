namespace AvepointMosPlatform.BLL
{
    public class AppConfiguration
    {
        public AppInfo AppInfo { get; set; } = null!;
        public UrlsConfig UrlsConfig { get; set; } = null!;
        public DatabaseConfig DatabaseConfig { get; set; } = null!;
        public JwtConfig JwtConfig { get; set; } = null!;
        public RefreshTokenConfig RefreshTokenConfig { get; set; } = null!;
        public BackgroundJobConfig BackgroundJobConfig { get; set; } = null!;
        public PerformanceMiddlewareConfig PerformanceMiddlewareConfig { get; set; } = null!;
        public ResilienceConfig ResilienceConfig { get; set; } = null!;
        public RateLimitConfig RateLimitConfig { get; set; } = null!;
        public DataCacheConfig DataCacheConfig { get; set; } = null!;
        public EmailConfig EmailConfig { get; set; } = null!;
        public RedisConfig RedisConfig { get; set; } = null!;
        public R2Config R2Config { get; set; } = null!;
        public GoogleConfig GoogleConfig { get; set; } = null!;
        public Microsoft365Config Microsoft365Config { get; set; } = null!;
        public FacebookConfig FacebookConfig { get; set; } = null!;
    }

    #region AppInfo
    public class AppInfo
    {
        public string Name { get; set; } = null!;
        public string Version { get; set; } = null!;
    }
    #endregion

    #region UrlsConfig
    public class UrlsConfig
    {
        public string FrontendUrl { get; set; } = null!;
    }
    #endregion

    #region DatabaseConfig
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; } = null!;
    }
    #endregion

    #region JwtConfig
    public class JwtConfig
    {
        public string SecretKey { get; set; } = null!;
        public int ExpireTimeInMinutes { get; set; }
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
    }
    #endregion

    #region RefreshTokenConfig
    public class RefreshTokenConfig
    {
        public string CookieName { get; set; } = null!;
        public string Domain { get; set; } = null!;
        public bool Secure { get; set; }
        public string SameSite { get; set; } = null!;
        public int ExpirationDays { get; set; }
        public int StaySignedInExpirationDays { get; set; }
    }
    #endregion

    #region BackgroundJobConfig
    public class BackgroundJobConfig
    {
        public int DataCleanupIntervalHours { get; set; }
        public int AuthTransientRetentionDays { get; set; }
        public int OutboxRetentionDays { get; set; }
        public int CleanupBatchSize { get; set; }
    }
    #endregion

    #region PerformanceMiddlewareConfig
    public class PerformanceMiddlewareConfig
    {
        public int SlowRequestThresholdMs { get; set; }
        public string ResponseTimeHeaderName { get; set; } = null!;
        public List<string> ExcludedPathPrefixes { get; set; } = null!;
    }
    #endregion

    #region ResilienceConfig
    public class ResilienceConfig
    {
        public int SocialProviderTimeoutSeconds { get; set; }
        public int SocialProviderRetryCount { get; set; }
        public int SocialProviderCircuitBreakSeconds { get; set; }
        public int EmailTimeoutSeconds { get; set; }
        public int EmailRetryCount { get; set; }
        public int EmailCircuitBreakSeconds { get; set; }
        public int EmailRateLimitPerMinute { get; set; }
        public int R2UploadTimeoutSeconds { get; set; }
        public int R2UploadRetryCount { get; set; }
        public int R2DeleteTimeoutSeconds { get; set; }
        public int R2DeleteRetryCount { get; set; }
    }
    #endregion

    #region RateLimitConfig
    public class RateLimitConfig
    {
        public int GeneralPermitLimitPerMinute { get; set; }
        public int AuthSensitivePermitLimitPerMinute { get; set; }
        public int AuthSensitivePermitLimitPerHour { get; set; }
        public int QueueLimit { get; set; }
    }
    #endregion

    #region DataCacheConfig
    public class DataCacheConfig
    {
        public int SettingsTtlMinutes { get; set; }
    }
    #endregion

    #region EmailConfig
    public class EmailConfig
    {
        public string ApiKey { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
        public string FromDisplayName { get; set; } = null!;
        public string PasswordResetUrlTemplate { get; set; } = null!;
    }
    #endregion

    #region RedisConfig
    public class RedisConfig
    {
        public string Host { get; set; } = null!;
        public string InstanceName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool AbortOnConnectFail { get; set; }
        public int ConnectRetry { get; set; }
        public int ConnectTimeoutMs { get; set; }
        public int SyncTimeoutMs { get; set; }
        public int ReconnectRetryMs { get; set; }
    }
    #endregion

    #region GoogleConfig
    public class GoogleConfig
    {
        public string ClientId { get; set; } = null!;
    }
    #endregion

    #region Microsoft365Config
    public class Microsoft365Config
    {
        public string ClientId { get; set; } = null!;
        public string TenantId { get; set; } = null!;
    }
    #endregion

    #region FacebookConfig
    public class FacebookConfig
    {
        public string AppId { get; set; } = null!;
        public string AppSecret { get; set; } = null!;
        public string GraphVersion { get; set; } = null!;
    }
    #endregion

    #region R2Config
    public class R2Config
    {
        public string AccountId { get; set; } = null!;
        public string AccessKeyId { get; set; } = null!;
        public string SecretAccessKey { get; set; } = null!;
        public string Bucket { get; set; } = null!;
        public string PublicBaseUrl { get; set; } = null!;
    }
    #endregion
}
