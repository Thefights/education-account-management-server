namespace Interfaces.Email
{
    public interface IOutboxMessageProcessor
    {
        Task ProcessPendingAsync(CancellationToken cancellationToken = default);
    }
}
