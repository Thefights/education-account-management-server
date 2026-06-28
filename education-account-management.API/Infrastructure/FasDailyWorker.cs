using Interfaces.Audit;
using Interfaces.FasApplications;
using Repositories.Interfaces;

namespace Infrastructure
{
    public class FasDailyWorker(IServiceScopeFactory serviceScopeFactory,
        ILogger<FasDailyWorker> logger)
        : BaseBackgroundJob(serviceScopeFactory, logger)
    {
        protected override string JobName => nameof(FasDailyWorker);

        protected override TimeSpan Interval => TimeSpan.FromMinutes(1);

        protected override async Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var backgroundService = serviceProvider.GetRequiredService<IFasBackgroundService>();
            var auditLogWriter = serviceProvider.GetRequiredService<IAuditLogWriter>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            var count = await backgroundService.SweepExpiredApplicationsAsync(cancellationToken);

            if (count == 0)
                return;

            await auditLogWriter.LogAsync(
                Enums.AuditLogCategory.StatusChange,
                $"FAS Sweep Completed: Marked {count} application(s) as Expired",
                cancellationToken: cancellationToken);

            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
