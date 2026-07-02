using Emails;

namespace Interfaces.Email
{
    public interface IOutboxWriter
    {
        Task EnqueueEmailAsync(
            string toEmail,
            EmailTemplate template,
            CancellationToken cancellationToken = default);

        Task EnqueueEmailOnceAsync(
            string toEmail,
            EmailTemplate template,
            CancellationToken cancellationToken = default);
    }
}
