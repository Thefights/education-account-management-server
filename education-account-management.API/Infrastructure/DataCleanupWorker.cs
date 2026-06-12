using Interfaces.Maintenance;

namespace Infrastructure
{
    public class DataCleanupWorker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<DataCleanupWorker> logger,
        AppConfiguration configuration)
        : BaseBackgroundJob(serviceScopeFactory, logger)
    {
        private readonly AppConfiguration _configuration = configuration;

        protected override string JobName => nameof(DataCleanupWorker);

        protected override TimeSpan Interval => TimeSpan.FromHours(
            Math.Max(1, _configuration.BackgroundJobConfig.DataCleanupIntervalHours));

        protected override async Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var cleanupService = serviceProvider.GetRequiredService<IDataCleanupService>();
            await cleanupService.CleanupAsync(
                _configuration.BackgroundJobConfig.AuthTransientRetentionDays,
                _configuration.BackgroundJobConfig.OutboxRetentionDays,
                _configuration.BackgroundJobConfig.CleanupBatchSize,
                cancellationToken);
        }
    }
}
