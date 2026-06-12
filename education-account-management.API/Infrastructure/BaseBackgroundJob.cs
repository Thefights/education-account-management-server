namespace Infrastructure
{
    public abstract class BaseBackgroundJob(
        IServiceScopeFactory serviceScopeFactory,
        ILogger logger)
        : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger _logger = logger;

        protected abstract string JobName { get; }

        protected abstract TimeSpan Interval { get; }

        protected abstract Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    _logger.LogDebug("Background job {JobName} started.", JobName);
                    await ExecuteJobAsync(scope.ServiceProvider, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background job {JobName} failed.", JobName);
                }

                try
                {
                    await Task.Delay(Interval, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }
    }
}
