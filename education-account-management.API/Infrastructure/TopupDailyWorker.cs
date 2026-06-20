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

        protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

        protected override async Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var topupService = serviceProvider.GetRequiredService<ITopupService>();
            var auditLogWriter = serviceProvider.GetRequiredService<IAuditLogWriter>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            var results = await topupService.ExecuteDueTopupsAsync(cancellationToken);

            if (results.Count == 0)
                return;

            await auditLogWriter.LogAsync(
                Enums.AuditLogCategory.Transaction,
                "Due Top-Up Sweep Completed",
                System.Text.Json.JsonSerializer.Serialize(results),
                targetNric: null,
                cancellationToken: cancellationToken);

            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
