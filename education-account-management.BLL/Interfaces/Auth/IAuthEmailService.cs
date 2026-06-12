namespace Interfaces.Auth
{
    public interface IAuthEmailService
    {
        Task SendPasswordResetEmailAsync(
            string toEmail,
            string resetToken,
            DateTime expiresAt,
            CancellationToken cancellationToken = default);

        Task SendPasswordChangedEmailAsync(
            string toEmail,
            CancellationToken cancellationToken = default);

        Task SendWelcomeEmailAsync(
            string toEmail,
            string displayName,
            CancellationToken cancellationToken = default);

        Task SendOtpEmailAsync(
            string toEmail,
            string otpCode,
            DateTime expiresAt,
            CancellationToken cancellationToken = default);
    }
}
