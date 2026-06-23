using Interfaces.Payments;
using Interfaces.Audit;
using Enums;
using Repositories.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class MonthlyAutoDeductWorker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MonthlyAutoDeductWorker> logger)
        : BaseBackgroundJob(serviceScopeFactory, logger)
    {
        protected override string JobName => nameof(MonthlyAutoDeductWorker);

        protected override TimeSpan Interval => TimeSpan.FromDays(1);

        protected override bool RunImmediately => false;

        protected override TimeSpan GetDelayBeforeNextExecution()
        {
            TimeZoneInfo sgtZone;
            try
            {
                sgtZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Singapore");
            }
            catch (TimeZoneNotFoundException)
            {
                sgtZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            }
            var nowSgt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sgtZone);
            var targetDate = new DateTime(nowSgt.Year, nowSgt.Month, 5, 0, 0, 0, DateTimeKind.Unspecified);
            if (nowSgt >= targetDate)
            {
                targetDate = targetDate.AddMonths(1);
            }
            return targetDate - nowSgt;
        }

        protected override async Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var autoDeductService = serviceProvider.GetRequiredService<IMonthlyAutoDeductService>();
            var auditLogWriter = serviceProvider.GetRequiredService<IAuditLogWriter>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            await autoDeductService.AutoDeductOutstandingFeesAsync(cancellationToken);
            await auditLogWriter.LogAsync(
                AuditLogCategory.Transaction,
                "Monthly Outstanding Auto-Deduction Run Completed",
                cancellationToken: cancellationToken);
            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
