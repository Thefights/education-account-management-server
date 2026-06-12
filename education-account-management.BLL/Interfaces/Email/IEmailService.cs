using Emails;

namespace Interfaces.Email
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, EmailTemplate template, CancellationToken cancellationToken = default);
    }
}
