using Interfaces.Email;

namespace Infrastructure
{
    public class OutboxEmailWorker(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<OutboxEmailWorker> logger)
        : BaseBackgroundJob(serviceScopeFactory, logger)
    {
        protected override string JobName => nameof(OutboxEmailWorker);

        protected override TimeSpan Interval => TimeSpan.FromSeconds(30);

        protected override async Task ExecuteJobAsync(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var processor = serviceProvider.GetRequiredService<IOutboxMessageProcessor>();
            await processor.ProcessPendingAsync(cancellationToken);
        }
    }
}
