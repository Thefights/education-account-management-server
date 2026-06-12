namespace Interfaces.Maintenance
{
    public interface IDataCleanupService
    {
        Task CleanupAsync(
            int authTransientRetentionDays,
            int outboxRetentionDays,
            int batchSize,
            CancellationToken cancellationToken = default);
    }
}
