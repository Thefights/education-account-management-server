using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthMfaService
    {
        Task<LoginMfaOtpDTO> CreateLoginMfaOtpAsync(
            AuthAccount authAccount,
            DateTime now,
            CancellationToken cancellationToken = default);

        Task<LoginMfaOtpDTO> ResendLoginMfaOtpAsync(
            ResendMfaOtpRequestDTO request,
            DateTime now,
            CancellationToken cancellationToken = default);

        Task<OtpVerification> VerifyLoginMfaOtpAsync(
            VerifyMfaOtpRequestDTO request,
            DateTime now,
            CancellationToken cancellationToken = default);
    }
}
