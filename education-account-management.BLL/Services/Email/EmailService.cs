using education_account_management.BLL;
using Emails;
using Infrastructure;
using Interfaces.Email;
using Polly;
using Polly.Registry;
using Resend;

namespace Services.Email
{
    public class EmailService(
        AppConfiguration configuration,
        IResend resend,
        ILogger<EmailService> logger,
        ResiliencePipelineProvider<string> resiliencePipelineProvider)
        : IEmailService
    {
        private readonly AppConfiguration _configuration = configuration;
        private readonly IResend _resend = resend;
        private readonly ILogger<EmailService> _logger = logger;
        private readonly ResiliencePipeline _emailProviderPipeline = resiliencePipelineProvider
            .GetPipeline(ResiliencePipelineNames.EmailProvider);

        public async Task SendAsync(string toEmail, EmailTemplate template, CancellationToken cancellationToken = default)
        {
            var message = new EmailMessage
            {
                From = $"{_configuration.EmailConfig.FromDisplayName} <{_configuration.EmailConfig.FromEmail}>",
                Subject = template.Subject,
                HtmlBody = template.HtmlBody,
                TextBody = template.TextBody
            };
            message.To.Add(toEmail);

            try
            {
                await _emailProviderPipeline.ExecuteAsync(
                    async token => await _resend.EmailSendAsync(message, token),
                    cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(
                    ex,
                    "Failed to send email to {ToEmail} with subject {Subject}.",
                    toEmail,
                    template.Subject);
                throw new InternalAppException("Email delivery failed.", ex);
            }
        }
    }
}
