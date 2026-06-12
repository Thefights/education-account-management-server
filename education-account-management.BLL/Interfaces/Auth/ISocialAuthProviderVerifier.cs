using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface ISocialAuthProviderVerifier
    {
        Task<SocialAuthProviderProfileDTO> VerifyAsync(
            SocialLoginProvider provider,
            string providerToken,
            CancellationToken cancellationToken = default);
    }
}
