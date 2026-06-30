using DTOs.Admin;
using DTOs.Auth;
using Interfaces.Audit;
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
        ICurrentUserService currentUserService,
        ITokenBlacklistService tokenBlacklistService,
        AppConfiguration configuration,
        IAuditLogWriter auditLogWriter)
        : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AppConfiguration _configuration = configuration;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ITokenBlacklistService _tokenBlacklistService = tokenBlacklistService;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IGenericRepository<AdminProfile> _adminRepository = unitOfWork.Repository<AdminProfile>();
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository = unitOfWork.Repository<RefreshToken>();
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<UserStatusHistory> _userStatusHistoryRepository = unitOfWork.Repository<UserStatusHistory>();
        private readonly IGenericRepository<SsoIdentity> _ssoIdentityRepository = unitOfWork.Repository<SsoIdentity>();
        private readonly IGenericRepository<Models.EducationAccount> _educationAccountRepository = unitOfWork.Repository<Models.EducationAccount>();

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

        public async Task LogoutAsync(
            string refreshToken,
            string accessToken,
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var tokenHash = TokenUtil.HashToken(refreshToken);
                var refreshTokenEntity = await _refreshTokenRepository.Query(tracking: true)
                    .FirstOrDefaultAsync(
                        token => token.TokenHash == tokenHash,
                        cancellationToken);

                if (refreshTokenEntity != null && refreshTokenEntity.RevokedAt == null)
                {
                    refreshTokenEntity.RevokedAt = DateTime.UtcNow;
                    await _unitOfWork.SaveChangeAsync(cancellationToken);
                }
            }

            await _tokenBlacklistService.BlacklistAsync(accessToken);
        }

        public async Task<AuthLoginResponseDTO> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var now = DateTime.UtcNow;
            var tokenHash = TokenUtil.HashToken(refreshToken);
            var refreshTokenEntity = await _refreshTokenRepository.Query(tracking: true)
                .Include(token => token.User)
                .FirstOrDefaultAsync(
                    token => token.TokenHash == tokenHash,
                    cancellationToken);

            if (refreshTokenEntity == null
                || refreshTokenEntity.RevokedAt != null
                || refreshTokenEntity.ExpiresAt <= now)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            if (refreshTokenEntity.User.Status != UserStatus.Active
                || refreshTokenEntity.User.LockedUntil > now)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var user = await _userRepository.Query(tracking: true)
                .Include(user => user.Citizen)
                .Include(user => user.AdminProfile)
                .FirstOrDefaultAsync(
                    user => user.Id == refreshTokenEntity.UserId,
                    cancellationToken)
                ?? throw new UnauthorizedAccessException("Invalid refresh token");

            refreshTokenEntity.RevokedAt = now;
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

            var metadataAddress =
                $"https://login.microsoftonline.com/{tenantId}/v2.0/.well-known/openid-configuration";

            OpenIdConnectConfiguration openIdConfig;
            try
            {
                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    metadataAddress,
                    new OpenIdConnectConfigurationRetriever());

                openIdConfig = await configurationManager.GetConfigurationAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }

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
                }.ValidateToken(request.IdToken, validationParameters, out var validatedToken);
            }
            catch (Exception ex) when (ex is SecurityTokenException or ArgumentException)
            {
                throw new UnauthorizedAccessException("Invalid login credentials", ex);
            }

            var tokenTenantId = principal.FindFirst("tid")?.Value;
            var objectId = principal.FindFirst("oid")?.Value;
            var audience = principal.FindFirst("aud")?.Value;
            var issuer = principal.FindFirst("iss")?.Value;
            var username = principal.FindFirst("preferred_username")?.Value;

            if (string.IsNullOrWhiteSpace(tokenTenantId) || string.IsNullOrWhiteSpace(objectId))
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            var compositeProviderUserId = $"{tokenTenantId}:{objectId}";

            var user = await ResolveUserFromSsoAsync(
                SsoProvider.AzureAD,
                cancellationToken,
                compositeProviderUserId,
                objectId);

            if (user.Role is not (UserRole.SystemAdmin
                or UserRole.FinanceAdmin
                or UserRole.SchoolAdmin))
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
                .Include(identity => identity.User)
                    .ThenInclude(user => user.Citizen)
                .Include(identity => identity.User)
                    .ThenInclude(user => user.AdminProfile)
                .FirstOrDefaultAsync(
                    identity =>
                        identity.Provider == provider
                        && normalizedProviderUserIds.Contains(identity.ProviderUserId),
                    cancellationToken);

            if (ssoIdentity == null)
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            if (ssoIdentity.User.Status != UserStatus.Active)
            {
                throw new UnauthorizedAccessException("Invalid login credentials");
            }

            if (ssoIdentity.User.LockedUntil.HasValue
                && ssoIdentity.User.LockedUntil.Value > DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Account is temporarily locked");
            }

            return ssoIdentity.User;
        }

        private async Task<AuthLoginResponseDTO> IssueLoginResultAsync(
            User user,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var accessTokenExpiresAt = now.AddMinutes(_configuration.JwtConfig.ExpireTimeInMinutes);
            var refreshToken = TokenUtil.GenerateRefreshToken();
            var refreshTokenExpiresAt = now.AddDays(_configuration.RefreshTokenConfig.ExpirationDays);

            user.LastLoginAt = now;
            user.FailedLoginCount = 0;

            await _refreshTokenRepository.AddAsync(
                new RefreshToken
                {
                    UserId = user.Id,
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
                UserId = user.Id,
                Role = user.Role.ToString(),
            };
        }

        public async Task UpdateStatusAsync(
            UpdateAdminStatusDTO dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // Execute the operations inside a database transaction to ensure consistency
            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                // Fetch the admin profiles with their users.
                var adminProfiles = await _adminRepository
                    .Query(tracking: true)
                    .Include(a => a.User)
                    .Where(a => dto.AdminIds.Contains(a.Id))
                    .ToListAsync(token);

                // Throw an exception if some of the requested admin profiles are not found
                if (adminProfiles.Count != dto.AdminIds.Count)
                {
                    var foundIds = adminProfiles.Select(a => a.Id).ToHashSet();
                    var firstMissingId = dto.AdminIds.First(id => !foundIds.Contains(id));
                    throw new DataNotFoundException(typeof(AdminProfile), firstMissingId);
                }

                // Prevent the currently logged-in user from changing their own status
                var currentUserId = _currentUserService.UserId;
                var selfEntry = adminProfiles.FirstOrDefault(a => a.UserId == currentUserId);
                if (selfEntry != null)
                {
                    throw new ValidationFailureException("You cannot update your own status.");
                }

                var revokeTime = DateTime.UtcNow;
                var auditAction = dto.Status == UserStatus.Inactive
                    ? "DeactivateAdmin"
                    : "ReactivateAdmin";

                var userIds = adminProfiles.Select(a => a.UserId).ToList();

                // If deactivating, retrieve all currently active refresh tokens to revoke them
                var activeRefreshTokens = dto.Status == UserStatus.Inactive
                    ? await _refreshTokenRepository
                        .Query(tracking: true)
                        .Where(t => userIds.Contains(t.UserId)
                                    && t.RevokedAt == null
                                    && t.ExpiresAt > revokeTime)
                        .ToListAsync(token)
                    : [];

                // Update the status, handle sessions in Redis, and create audit logs for each admin
                foreach (var adminProfile in adminProfiles)
                {
                    var user = adminProfile.User;
                    var previousStatus = user.Status;
                    user.Status = dto.Status;

                    if (dto.Status == UserStatus.Inactive)
                    {
                        // Revoke active refresh tokens
                        foreach (var refreshToken in activeRefreshTokens.Where(t => t.UserId == user.Id))
                        {
                            refreshToken.RevokedAt = revokeTime;
                        }

                        // Blacklist the account in Redis and register a rollback hook to revert if the SQL commit fails
                        await _tokenBlacklistService.BlacklistUserAsync(user.Id);
                        transaction.OnRollback(() => _tokenBlacklistService.UnblacklistUserAsync(user.Id));
                    }
                    else if (dto.Status == UserStatus.Active)
                    {
                        // Remove the blacklist in Redis and register a rollback hook to revert if the SQL commit fails
                        await _tokenBlacklistService.UnblacklistUserAsync(user.Id);
                        transaction.OnRollback(() => _tokenBlacklistService.BlacklistUserAsync(user.Id));
                    }

                    if (previousStatus != user.Status)
                    {
                        var history = new UserStatusHistory
                        {
                            UserId = user.Id,
                            PreviousStatus = previousStatus,
                            NewStatus = user.Status,
                            Reason = dto.Reason,
                            ChangedAt = revokeTime,
                            ChangedByUserId = currentUserId
                        };
                        history.TryValidate();
                        await _userStatusHistoryRepository.AddAsync(history, token);
                    }

                    // Log the activity
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.StatusChange,
                        auditAction,
                        adminProfile.Nric,
                        cancellationToken: token);
                }
            }, cancellationToken);
        }
    }
}
