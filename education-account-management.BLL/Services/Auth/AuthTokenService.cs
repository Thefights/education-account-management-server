using AvepointMosPlatform.BLL;
using DTOs.Auth;
using Interfaces.Auth;
using Security;

namespace Services.Auth
{
    public class AuthTokenService(
        IUnitOfWork unitOfWork,
        AppConfiguration configuration,
        ITokenBlacklistService tokenBlacklistService)
        : IAuthTokenService
    {
        private readonly AppConfiguration _configuration = configuration;
        private readonly ITokenBlacklistService _tokenBlacklistService = tokenBlacklistService;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository = unitOfWork.Repository<RefreshToken>();

        public async Task<(AuthTokenResponseDTO Response, RefreshToken RefreshToken)> IssueTokensAsync(
            AuthAccount authAccount,
            bool staySignedIn,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_configuration.JwtConfig.ExpireTimeInMinutes);
            var refreshTokenExpirationDays = staySignedIn
                ? _configuration.RefreshTokenConfig.StaySignedInExpirationDays
                : _configuration.RefreshTokenConfig.ExpirationDays;
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
            var accessToken = TokenUtil.CreateAccessToken(_configuration, authAccount, accessTokenExpiresAt);
            var refreshToken = TokenUtil.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                AuthAccountId = authAccount.Id,
                TokenHash = TokenUtil.HashToken(refreshToken),
                CreatedByIp = ipAddress,
                UserAgent = userAgent,
                ExpiresAt = refreshTokenExpiresAt,
                StaySignedIn = staySignedIn
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

            return (new AuthTokenResponseDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            }, refreshTokenEntity);
        }

        public void RevokeRefreshToken(RefreshToken refreshToken, string? ipAddress, DateTime revokedAt)
        {
            refreshToken.RevokedAt = revokedAt;
            refreshToken.RevokedByIp = ipAddress;
        }

        public async Task BlacklistAccessTokenAsync(string? accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                await _tokenBlacklistService.BlacklistAsync(accessToken);
            }
        }
    }
}
