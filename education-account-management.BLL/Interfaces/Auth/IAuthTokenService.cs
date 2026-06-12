using DTOs.Auth;

namespace Interfaces.Auth
{
    public interface IAuthTokenService
    {
        Task<(AuthTokenResponseDTO Response, RefreshToken RefreshToken)> IssueTokensAsync(
            AuthAccount authAccount,
            bool staySignedIn,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default);

        void RevokeRefreshToken(RefreshToken refreshToken, string? ipAddress, DateTime revokedAt);

        Task BlacklistAccessTokenAsync(string? accessToken);
    }
}
