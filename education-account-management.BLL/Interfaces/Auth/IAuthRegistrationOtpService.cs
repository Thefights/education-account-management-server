using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthRegistrationOtpService
    {
        Task<SendRegisterEmailOtpResponseDTO> SendEmailOtpAsync(
            SendRegisterEmailOtpRequestDTO request,
            CancellationToken cancellationToken = default);

        Task VerifyEmailOtpAsync(
            VerifyRegisterEmailOtpRequestDTO request,
            CancellationToken cancellationToken = default);

        Task EnsureEmailVerifiedAsync(
            string? email,
            string? sessionId,
            CancellationToken cancellationToken = default);
    }
}
