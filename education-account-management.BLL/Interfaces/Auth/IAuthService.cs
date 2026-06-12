using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken = default);

        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);

        Task<AuthTokenResponseDTO> SocialLoginAsync(SocialLoginRequestDTO request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);

        Task LogoutAsync(string? refreshToken, string? accessToken, string? ipAddress, CancellationToken cancellationToken = default);

        Task<AuthTokenResponseDTO> VerifyMfaOtpAsync(VerifyMfaOtpRequestDTO request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);

        Task<ResendMfaOtpResponseDTO> ResendMfaOtpAsync(ResendMfaOtpRequestDTO request, CancellationToken cancellationToken = default);

        Task<AuthTokenResponseDTO> RefreshTokenAsync(string? refreshToken, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);

        Task ForgotPasswordAsync(ForgotPasswordRequestDTO request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);

        Task ResetPasswordAsync(ResetPasswordRequestDTO request, CancellationToken cancellationToken = default);
    }
}
