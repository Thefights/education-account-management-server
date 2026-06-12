using AvepointMosPlatform.BLL;
using Emails;
using Interfaces.Auth;
using Interfaces.Email;

namespace Services.Auth
{
    public class AuthEmailService(
        AppConfiguration configuration,
        IOutboxWriter outboxWriter,
        EmailTemplateBuilder emailTemplateBuilder)
        : IAuthEmailService
    {
        private readonly AppConfiguration _configuration = configuration;
        private readonly IOutboxWriter _outboxWriter = outboxWriter;
        private readonly EmailTemplateBuilder _emailTemplateBuilder = emailTemplateBuilder;

        public async Task SendPasswordResetEmailAsync(
            string toEmail,
            string resetToken,
            DateTime expiresAt,
            CancellationToken cancellationToken = default)
        {
            var resetUrl = _configuration.EmailConfig.PasswordResetUrlTemplate
                .Replace("{token}", Uri.EscapeDataString(resetToken), StringComparison.Ordinal);
            var template = _emailTemplateBuilder.BuildPasswordResetEmail(
                _configuration.AppInfo.Name,
                resetUrl,
                expiresAt);

            await _outboxWriter.EnqueueEmailAsync(toEmail, template, cancellationToken);
        }

        public async Task SendPasswordChangedEmailAsync(
            string toEmail,
            CancellationToken cancellationToken = default)
        {
            var template = _emailTemplateBuilder.BuildPasswordChangedEmail(_configuration.AppInfo.Name);

            await _outboxWriter.EnqueueEmailAsync(toEmail, template, cancellationToken);
        }

        public async Task SendWelcomeEmailAsync(
            string toEmail,
            string displayName,
            CancellationToken cancellationToken = default)
        {
            var template = _emailTemplateBuilder.BuildWelcomeEmail(_configuration.AppInfo.Name, displayName);

            await _outboxWriter.EnqueueEmailAsync(toEmail, template, cancellationToken);
        }

        public async Task SendOtpEmailAsync(
            string toEmail,
            string otpCode,
            DateTime expiresAt,
            CancellationToken cancellationToken = default)
        {
            var template = _emailTemplateBuilder.BuildOtpEmail(_configuration.AppInfo.Name, otpCode, expiresAt);

            await _outboxWriter.EnqueueEmailAsync(toEmail, template, cancellationToken);
        }
    }
}
