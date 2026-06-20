using Interfaces.EducationAccounts;
using Interfaces.Audit;
using Enums;
using Repositories.Interfaces;

namespace Infrastructure
{
    public class AccountProvisioningWorker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AccountProvisioningWorker> logger)
        : BaseBackgroundJob(serviceScopeFactory, logger)
    {
        protected override string JobName => nameof(AccountProvisioningWorker);

        protected override TimeSpan Interval => TimeSpan.FromDays(1);

        protected override bool RunImmediately => false;

        protected override TimeSpan GetDelayBeforeNextExecution()
        {
            var nowSgt = DateTime.UtcNow.AddHours(8);
            return nowSgt.Date.AddDays(1) - nowSgt;
        }

        protected override async Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var educationAccountService = serviceProvider.GetRequiredService<IEducationAccountService>();
            var auditLogWriter = serviceProvider.GetRequiredService<IAuditLogWriter>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            var report = await educationAccountService.SweepAccountsAsync(cancellationToken);
            await auditLogWriter.LogAsync(
                AuditLogCategory.AccountCreation,
                "Auto Provisioning Sweep Completed",
                cancellationToken: cancellationToken);
            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
