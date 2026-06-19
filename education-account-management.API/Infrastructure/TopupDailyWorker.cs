using Common.HttpResults;
using Interfaces;
using Interfaces.Audit;
using Interfaces.TopUp;
using Repositories.Interfaces;

namespace Infrastructure
{
    public class TopupDailyWorker(IServiceScopeFactory serviceScopeFactory,
        ILogger<TopupDailyWorker> logger)
        : BaseBackgroundJob(serviceScopeFactory, logger)
    {
        protected override string JobName => nameof(TopupDailyWorker);

        protected override TimeSpan Interval => TimeSpan.Zero;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nowUtc = DateTime.UtcNow;
                var nowSgt = nowUtc.AddHours(8);
                var nextRunSgt = nowSgt.Date.AddDays(1);
                var delay = nextRunSgt - nowSgt;

                _logger.LogInformation("TopupDailyWorker scheduled to run next at {NextRunSgt} SGT (Delay: {Delay})", nextRunSgt, delay);

                try
                {
                    await Task.Delay(delay, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    _logger.LogInformation("Background job {JobName} started.", JobName);
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
            }
        }

        protected override async Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var topupService = serviceProvider.GetRequiredService<ITopupService>();
            var auditLogWriter = serviceProvider.GetRequiredService<IAuditLogWriter>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            var results = await topupService.DailyExecuteTopupAsync(cancellationToken);

            await auditLogWriter.LogAsync(
                Enums.AuditLogCategory.Transaction,
                "Daily Scheduled Batch Top-Up Sweep Completed",
                System.Text.Json.JsonSerializer.Serialize(results),
                targetNric: null,
                cancellationToken: cancellationToken);

            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
