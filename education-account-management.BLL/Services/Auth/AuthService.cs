using DTOs.Auth;
using Interfaces.Auth;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.Auth
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        AppConfiguration configuration)
        : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AppConfiguration _configuration = configuration;
        private readonly IGenericRepository<SsoIdentity> _ssoIdentityRepository = unitOfWork.Repository<SsoIdentity>();
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<EducationAccount> _educationAccountRepository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository = unitOfWork.Repository<RefreshToken>();

        public async Task<AuthLoginResponseDTO> LoginWithMockSingpassAsync(
            CancellationToken cancellationToken = default)
        {
            var user = await ResolveUserFromSsoAsync(
                SsoProvider.Singpass,
                cancellationToken,
                "singpass-subject-004");

            if (user.Role != UserRole.AccountHolder)
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            if (!user.CitizenId.HasValue)
            {
                throw new UnauthorizedAccessException("AccountHolder does not have a valid education account");
            }

            var hasValidEducationAccount = await _educationAccountRepository.AnyAsync(
                educationAccount =>
                    educationAccount.CitizenId == user.CitizenId.Value
                    && educationAccount.Status != EducationAccountStatus.Closed,
                cancellationToken);

            if (!hasValidEducationAccount)
            {
                throw new UnauthorizedAccessException("AccountHolder does not have a valid education account");
            }

            return await IssueLoginResultAsync(user, cancellationToken);
        }

        public async Task<AuthLoginResponseDTO> LoginWithAzureAdAsync(
            AzureAdLoginRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.IdToken))
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            var tenantId = string.IsNullOrWhiteSpace(_configuration.Microsoft365Config.TenantId)
                ? "common"
                : _configuration.Microsoft365Config.TenantId.Trim();
            var metadataAddress = $"https://login.microsoftonline.com/{tenantId}/v2.0/.well-known/openid-configuration";
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                metadataAddress,
                new OpenIdConnectConfigurationRetriever());
            var openIdConfig = await configurationManager.GetConfigurationAsync(cancellationToken);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                IssuerValidator = (issuer, _, _) =>
                {
                    if (!Uri.TryCreate(issuer, UriKind.Absolute, out var issuerUri)
                        || issuerUri.Host != "login.microsoftonline.com"
                        || !issuerUri.AbsolutePath.EndsWith("/v2.0", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new SecurityTokenInvalidIssuerException("Invalid Microsoft 365 issuer.");
                    }

                    if (!string.Equals(tenantId, "common", StringComparison.OrdinalIgnoreCase)
                        && !issuerUri.AbsolutePath.Contains($"/{tenantId}/", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new SecurityTokenInvalidIssuerException("Invalid Microsoft 365 tenant.");
                    }

                    return issuer;
                },
                ValidateAudience = true,
                ValidAudience = _configuration.Microsoft365Config.ClientId,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                IssuerSigningKeys = openIdConfig.SigningKeys
            };

            ClaimsPrincipal principal;
            try
            {
                principal = new JwtSecurityTokenHandler
                {
                    MapInboundClaims = false
                }.ValidateToken(request.IdToken, validationParameters, out _);
            }
            catch (Exception ex) when (ex is SecurityTokenException or ArgumentException)
            {
                throw new UnauthorizedAccessException("Invalid login credentials", ex);
            }

            var tokenTenantId = principal.FindFirst("tid")?.Value;
            var objectId = principal.FindFirst("oid")?.Value;
            if (string.IsNullOrWhiteSpace(tokenTenantId) || string.IsNullOrWhiteSpace(objectId))
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            var user = await ResolveUserFromSsoAsync(
                SsoProvider.AzureAD,
                cancellationToken,
                $"{tokenTenantId}:{objectId}",
                objectId);

            if (user.Role is not (UserRole.SystemAdmin
                or UserRole.FinanceAdmin
                or UserRole.CourseAdmin))
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            return await IssueLoginResultAsync(user, cancellationToken);
        }

        private async Task<User> ResolveUserFromSsoAsync(
            SsoProvider provider,
            CancellationToken cancellationToken,
            params string[] providerUserIds)
        {
            var normalizedProviderUserIds = providerUserIds
                .Where(providerUserId => !string.IsNullOrWhiteSpace(providerUserId))
                .Distinct()
                .ToArray();

            if (normalizedProviderUserIds.Length == 0)
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            var ssoIdentity = await _ssoIdentityRepository.Query(tracking: true)
                .Include(identity => identity.AuthAccount)
                .FirstOrDefaultAsync(
                    identity =>
                        identity.Provider == provider
                        && normalizedProviderUserIds.Contains(identity.ProviderUserId),
                    cancellationToken);

            if (ssoIdentity == null)
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            if (ssoIdentity.AuthAccount.Status != AuthAccountStatus.Active)
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            if (ssoIdentity.AuthAccount.LockedUntil.HasValue
                && ssoIdentity.AuthAccount.LockedUntil.Value > DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Account is temporarily locked");
            }

            var user = await _userRepository.Query(tracking: true)
                .Include(user => user.AuthAccount)
                .Include(user => user.Citizen)
                .Include(user => user.AdminProfile)
                .FirstOrDefaultAsync(
                    user => user.AuthAccountId == ssoIdentity.AuthAccountId,
                    cancellationToken);

            return user ?? throw new UnauthorizedAccessException("Invalid login credentials");
        }

        private async Task<AuthLoginResponseDTO> IssueLoginResultAsync(
            User user,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var accessTokenExpiresAt = now.AddMinutes(_configuration.JwtConfig.ExpireTimeInMinutes);
            var refreshToken = TokenUtil.GenerateRefreshToken();
            var refreshTokenExpiresAt = now.AddDays(_configuration.RefreshTokenConfig.ExpirationDays);

            user.AuthAccount.LastLoginAt = now;
            user.AuthAccount.FailedLoginCount = 0;

            await _refreshTokenRepository.AddAsync(
                new RefreshToken
                {
                    AuthAccountId = user.AuthAccountId,
                    TokenHash = TokenUtil.HashToken(refreshToken),
                    ExpiresAt = refreshTokenExpiresAt
                },
                cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return new AuthLoginResponseDTO
            {
                AccessToken = TokenUtil.CreateAccessToken(_configuration, user, accessTokenExpiresAt),
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt,
                AuthAccountId = user.AuthAccountId,
                UserId = user.Id,
                Role = user.Role.ToString(),
            };
        }
    }
}
