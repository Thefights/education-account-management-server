namespace Infrastructure
{
    public abstract class BaseBackgroundJob(
        IServiceScopeFactory serviceScopeFactory,
        ILogger logger)
        : BackgroundService
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        protected readonly ILogger _logger = logger;

        protected abstract string JobName { get; }

        protected abstract TimeSpan Interval { get; }

        protected virtual bool RunImmediately => true;

        protected virtual TimeSpan GetDelayBeforeNextExecution() => Interval;

        protected abstract Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!RunImmediately && !await DelayAsync(stoppingToken))
                return;

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

                if (!await DelayAsync(stoppingToken)) return;
            }
        }

        private async Task<bool> DelayAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.Delay(GetDelayBeforeNextExecution(), stoppingToken);
                return true;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return false;
            }
        }
    }
}
