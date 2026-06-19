using Interfaces.EducationAccount;
using System.Text.Json;

namespace Infrastructure
{
    public class AccountProvisioningWorker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AccountProvisioningWorker> logger)
        : BaseBackgroundJob(serviceScopeFactory, logger)
    {
        protected override string JobName => nameof(AccountProvisioningWorker);

        protected override TimeSpan Interval => TimeSpan.Zero;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nowUtc = DateTime.UtcNow;
                var nowSgt = nowUtc.AddHours(8);
                var nextRunSgt = nowSgt.Date.AddDays(1);
                var delay = nextRunSgt - nowSgt;

                _logger.LogInformation("AccountProvisioningWorker scheduled to run next at {NextRunSgt} SGT (Delay: {Delay})", nextRunSgt, delay);

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
            var educationAccountService = serviceProvider.GetRequiredService<IEducationAccountService>();
            var auditLogWriter = serviceProvider.GetRequiredService<Interfaces.Audit.IAuditLogWriter>();
            var unitOfWork = serviceProvider.GetRequiredService<Repositories.Interfaces.IUnitOfWork>();

            var result = await educationAccountService.AutoCreateAccountsAsync(cancellationToken);

            await auditLogWriter.LogAsync(
                Enums.AuditLogCategory.AccountCreation,
                "Auto Provisioning Sweep Completed",
                JsonSerializer.Serialize(result),
                targetNric: null,
                cancellationToken: cancellationToken);

            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
