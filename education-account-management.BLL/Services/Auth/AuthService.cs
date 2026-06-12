using DTOs.Auth;
using Interfaces.Audit;
using Interfaces.Auth;
using Security;

namespace Services.Auth
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        IAuthEmailService authEmailService,
        IAuthTokenService authTokenService,
        IAuthMfaService authMfaService,
        IAuthRegistrationOtpService authRegistrationOtpService,
        ISocialAuthProviderVerifier socialAuthProviderVerifier,
        IAuditLogWriter auditLogWriter,
        IValidator<RegisterRequestDTO> registerValidator,
        IValidator<LoginRequestDTO> loginValidator,
        IValidator<SocialLoginRequestDTO> socialLoginValidator,
        IValidator<ResetPasswordRequestDTO> resetPasswordValidator)
        : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthEmailService _authEmailService = authEmailService;
        private readonly IAuthTokenService _authTokenService = authTokenService;
        private readonly IAuthMfaService _authMfaService = authMfaService;
        private readonly IAuthRegistrationOtpService _authRegistrationOtpService = authRegistrationOtpService;
        private readonly ISocialAuthProviderVerifier _socialAuthProviderVerifier = socialAuthProviderVerifier;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

        private readonly IValidator<RegisterRequestDTO> _registerValidator = registerValidator;
        private readonly IValidator<LoginRequestDTO> _loginValidator = loginValidator;
        private readonly IValidator<SocialLoginRequestDTO> _socialLoginValidator = socialLoginValidator;
        private readonly IValidator<ResetPasswordRequestDTO> _resetPasswordValidator = resetPasswordValidator;

        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<AuthAccount> _authAccountRepository = unitOfWork.Repository<AuthAccount>();
        private readonly IGenericRepository<Role> _roleRepository = unitOfWork.Repository<Role>();
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository = unitOfWork.Repository<RefreshToken>();
        private readonly IGenericRepository<PasswordResetToken> _passwordResetTokenRepository = unitOfWork.Repository<PasswordResetToken>();
        private readonly IGenericRepository<SocialLogin> _socialLoginRepository = unitOfWork.Repository<SocialLogin>();

        private const int MaxFailedLoginAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan PasswordResetTokenLifetime = TimeSpan.FromHours(1);
        private const string InvalidCredentialMessage = "Invalid User ID or password";

        public async Task RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken = default)
        {
            _registerValidator.Validate(request);

            await _authRegistrationOtpService.EnsureEmailVerifiedAsync(
                request.Email,
                request.EmailVerificationSessionId,
                cancellationToken);

            var user = new User
            {
                FullName = request.FullName!,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender!.Value,
                AuthAccount = new AuthAccount
                {
                    UserIdText = request.UserId!,
                    Email = request.Email!,
                    PasswordHash = request.Password,
                    Status = AuthAccountStatus.Active,
                    FailedLoginCount = 0
                }
            };

            user.TryValidate();
            user.AuthAccount.TryValidate();

            var exists = await _authAccountRepository.AnyAsync(
                authAccount => authAccount.UserIdText == user.AuthAccount.UserIdText,
                cancellationToken);
            if (exists)
            {
                throw new DataConflictException("User ID already exists");
            }

            var tenantUserRole = await _roleRepository
                .Query(tracking: true)
                .FirstOrDefaultAsync(role => role.Name == "TenantUser", cancellationToken)
                ?? throw new InvalidOperationException("TenantUser role seed is missing");

            user.AuthAccount.PasswordHash = PasswordHashUtil.Hash(user.AuthAccount.PasswordHash!);
            user.UserRoles = [new UserRole { RoleId = tenantUserRole.Id }];

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    await _userRepository.AddAsync(user, token);
                    await _unitOfWork.SaveChangeAsync(token);

                    await _auditLogWriter.LogForActorAsync(
                        user.AuthAccount,
                        AuditLogCategory.Authentication,
                        AuditLogAction.Register,
                        $"AuthAccount:{user.AuthAccount.Id}:{user.AuthAccount.UserIdText}",
                        cancellationToken: token);

                    if (!string.IsNullOrWhiteSpace(user.AuthAccount.Email))
                    {
                        await _authEmailService.SendWelcomeEmailAsync(
                            user.AuthAccount.Email,
                            user.AuthAccount.UserIdText,
                            token);
                    }
                },
                cancellationToken);
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _loginValidator.Validate(request);
            }
            catch (InvalidDataException)
            {
                throw new UnauthorizedAccessException(InvalidCredentialMessage);
            }

            var now = DateTime.UtcNow;
            var authAccount = await _authAccountRepository
                .Query(tracking: true, ignoreQueryFilters: true)
                .Include(authAccount => authAccount.User)
                .ThenInclude(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(authAccount => authAccount.UserIdText == request.UserId!.Trim(), cancellationToken);

            if (authAccount == null || authAccount.IsDeleted || authAccount.User.IsDeleted)
            {
                await LogFailedLoginAsync(request.UserId, ipAddress, cancellationToken);
                throw new UnauthorizedAccessException(InvalidCredentialMessage);
            }

            if (authAccount.Status == AuthAccountStatus.Inactive)
            {
                await LogFailedLoginAsync(authAccount, ipAddress, cancellationToken);
                throw new ForbiddenException("Your account has been deactivated. Please contact your administrator.");
            }

            if (authAccount.LockedUntil.HasValue && authAccount.LockedUntil.Value > now)
            {
                await LogFailedLoginAsync(authAccount, ipAddress, cancellationToken);
                throw new ForbiddenException("Your account has been locked due to consecutive failed login attempts. Please try again later or contact your administrator to unlock your account.");
            }

            if (!PasswordHashUtil.Verify(request.Password!, authAccount.PasswordHash!))
            {
                await this.RecordFailedLoginAsync(authAccount, now, ipAddress, cancellationToken);
                throw new UnauthorizedAccessException(InvalidCredentialMessage);
            }

            authAccount.FailedLoginCount = 0;
            authAccount.LockedUntil = null;

            authAccount.LastLoginAt = now;
            var (tokens, _) = await _authTokenService.IssueTokensAsync(
                authAccount,
                request.StaySignedIn,
                ipAddress,
                userAgent,
                cancellationToken);
            await _auditLogWriter.LogForActorAsync(
                authAccount,
                AuditLogCategory.Authentication,
                AuditLogAction.Login,
                $"AuthAccount:{authAccount.Id}:{authAccount.UserIdText}",
                ipAddress,
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return new LoginResponseDTO
            {
                MfaRequired = false,
                Tokens = tokens
            };
        }

        public async Task<AuthTokenResponseDTO> SocialLoginAsync(
            SocialLoginRequestDTO request,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _socialLoginValidator.Validate(request);
            }
            catch (InvalidDataException ex)
            {
                throw new UnauthorizedAccessException("Invalid social login request.", ex);
            }

            var now = DateTime.UtcNow;
            var providerProfile = await _socialAuthProviderVerifier.VerifyAsync(
                request.Provider,
                request.ProviderToken!,
                cancellationToken);

            if (!providerProfile.EmailVerified)
            {
                throw new UnauthorizedAccessException("Provider email must be verified.");
            }

            var socialLogin = await _socialLoginRepository
                .Query(tracking: true, ignoreQueryFilters: true)
                .Include(login => login.AuthAccount)
                .ThenInclude(authAccount => authAccount.User)
                .ThenInclude(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(
                    login => login.Provider == providerProfile.Provider
                        && login.ProviderUserId == providerProfile.ProviderUserId,
                    cancellationToken);

            if (socialLogin != null)
            {
                if (socialLogin.AuthAccount.Status == AuthAccountStatus.Inactive
                    || socialLogin.AuthAccount.IsDeleted
                    || socialLogin.AuthAccount.User.IsDeleted)
                {
                    throw new UnauthorizedAccessException("Invalid social login account.");
                }

                socialLogin.ProviderEmail = providerProfile.Email;
                socialLogin.EmailVerified = true;
                socialLogin.AuthAccount.LastLoginAt = now;
                var (existingResponse, _) = await _authTokenService.IssueTokensAsync(
                    socialLogin.AuthAccount,
                    staySignedIn: false,
                    ipAddress,
                    userAgent,
                    cancellationToken);

                await _auditLogWriter.LogForActorAsync(
                    socialLogin.AuthAccount,
                    AuditLogCategory.Authentication,
                    AuditLogAction.SocialLogin,
                    $"AuthAccount:{socialLogin.AuthAccount.Id}:{socialLogin.AuthAccount.UserIdText}",
                    ipAddress,
                    cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                return existingResponse;
            }

            var tenantUserRole = await _roleRepository
                .Query(tracking: true)
                .FirstOrDefaultAsync(role => role.Name == "TenantUser", cancellationToken)
                ?? throw new InvalidOperationException("TenantUser role seed is missing");

            var user = new User
            {
                FullName = GenerateSocialFullName(providerProfile.Email),
                Gender = UserGender.Unknown,
                AuthAccount = new AuthAccount
                {
                    UserIdText = await CreateSocialUserIdTextAsync(
                        providerProfile.Provider,
                        providerProfile.ProviderUserId,
                        cancellationToken),
                    Email = providerProfile.Email,
                    PasswordHash = null,
                    Status = AuthAccountStatus.Active,
                    FailedLoginCount = 0,
                    LastLoginAt = now,
                    SocialLogins =
                    [
                        new SocialLogin
                        {
                            Provider = providerProfile.Provider,
                            ProviderUserId = providerProfile.ProviderUserId,
                            ProviderEmail = providerProfile.Email,
                            EmailVerified = true,
                            LinkedAt = now
                        }
                    ]
                },
                UserRoles =
                [
                    new UserRole
                    {
                        RoleId = tenantUserRole.Id,
                        Role = tenantUserRole
                    }
                ]
            };

            user.TryValidate();
            user.AuthAccount.TryValidate();

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var (newResponse, _) = await _authTokenService.IssueTokensAsync(
                user.AuthAccount,
                staySignedIn: false,
                ipAddress,
                userAgent,
                cancellationToken);

            await _auditLogWriter.LogForActorAsync(
                user.AuthAccount,
                AuditLogCategory.Authentication,
                AuditLogAction.SocialLogin,
                $"AuthAccount:{user.AuthAccount.Id}:{user.AuthAccount.UserIdText}",
                ipAddress,
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return newResponse;
        }

        public async Task LogoutAsync(string? refreshTokenValue,
            string? accessToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                throw new UnauthorizedAccessException("Refresh token is required");
            }

            var tokenHash = TokenUtil.HashToken(refreshTokenValue);
            var refreshToken = await _refreshTokenRepository
                .Query(tracking: true)
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

            if (refreshToken != null && refreshToken.RevokedAt == null)
            {
                _authTokenService.RevokeRefreshToken(refreshToken, ipAddress, DateTime.UtcNow);
                await _auditLogWriter.LogAsync(
                    AuditLogCategory.Authentication,
                    AuditLogAction.Logout,
                    "AuthSession:RefreshToken",
                    cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }

            await _authTokenService.BlacklistAccessTokenAsync(accessToken);
        }

        public async Task<AuthTokenResponseDTO> VerifyMfaOtpAsync(VerifyMfaOtpRequestDTO request,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var otpVerification = await _authMfaService.VerifyLoginMfaOtpAsync(request, now, cancellationToken);
            otpVerification.AuthAccount!.LastLoginAt = now;

            var (response, _) = await _authTokenService.IssueTokensAsync(
                otpVerification.AuthAccount,
                request.StaySignedIn,
                ipAddress,
                userAgent,
                cancellationToken);

            await _auditLogWriter.LogForActorAsync(
                otpVerification.AuthAccount,
                AuditLogCategory.Authentication,
                AuditLogAction.VerifyMfaOtp,
                $"AuthAccount:{otpVerification.AuthAccount.Id}:{otpVerification.AuthAccount.UserIdText}",
                ipAddress,
                cancellationToken);
            await _auditLogWriter.LogForActorAsync(
                otpVerification.AuthAccount,
                AuditLogCategory.Authentication,
                AuditLogAction.Login,
                $"AuthAccount:{otpVerification.AuthAccount.Id}:{otpVerification.AuthAccount.UserIdText}",
                ipAddress,
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return response;
        }

        public async Task<ResendMfaOtpResponseDTO> ResendMfaOtpAsync(
            ResendMfaOtpRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            var otp = await _authMfaService.ResendLoginMfaOtpAsync(
                request,
                DateTime.UtcNow,
                cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    await _authEmailService.SendOtpEmailAsync(
                        otp.Target,
                        otp.Code,
                        otp.ExpiresAt,
                        token);
                },
                cancellationToken);

            return new ResendMfaOtpResponseDTO
            {
                SessionId = otp.SessionId,
                ExpiresAt = otp.ExpiresAt
            };
        }

        public async Task<AuthTokenResponseDTO> RefreshTokenAsync(string? refreshTokenValue,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var now = DateTime.UtcNow;
            var tokenHash = TokenUtil.HashToken(refreshTokenValue);
            var refreshToken = await _refreshTokenRepository
                .Query(tracking: true)
                .Include(t => t.AuthAccount)
                .ThenInclude(authAccount => authAccount.User)
                .ThenInclude(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

            if (refreshToken == null || refreshToken.RevokedAt != null || refreshToken.ExpiresAt <= now)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            if (refreshToken.AuthAccount.Status == AuthAccountStatus.Inactive
                || refreshToken.AuthAccount.IsDeleted
                || refreshToken.AuthAccount.User.IsDeleted)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            var (response, replacement) = await _authTokenService.IssueTokensAsync(
                refreshToken.AuthAccount,
                refreshToken.StaySignedIn,
                ipAddress,
                userAgent,
                cancellationToken);
            _authTokenService.RevokeRefreshToken(refreshToken, ipAddress, now);
            refreshToken.ReplacedByRefreshToken = replacement;

            await _auditLogWriter.LogForActorAsync(
                refreshToken.AuthAccount,
                AuditLogCategory.Authentication,
                AuditLogAction.RefreshToken,
                $"AuthAccount:{refreshToken.AuthAccount.Id}:{refreshToken.AuthAccount.UserIdText}",
                ipAddress,
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return response;
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequestDTO request,
            string? ipAddress,
            string? userAgent,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                return;
            }

            var authAccount = await _authAccountRepository
                .Query(tracking: false)
                .Include(authAccount => authAccount.User)
                .FirstOrDefaultAsync(authAccount => authAccount.UserIdText == request.UserId.Trim(), cancellationToken);
            if (authAccount == null || authAccount.User.IsDeleted)
            {
                return;
            }

            var token = TokenUtil.GenerateRefreshToken();
            var resetToken = new PasswordResetToken
            {
                AuthAccountId = authAccount.Id,
                TokenHash = TokenUtil.HashToken(token),
                RequestedByIp = ipAddress,
                UserAgent = userAgent,
                ExpiresAt = DateTime.UtcNow.Add(PasswordResetTokenLifetime)
            };

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, transactionToken) =>
                {
                    await _passwordResetTokenRepository.AddAsync(resetToken, transactionToken);
                    await _auditLogWriter.LogForActorAsync(
                        authAccount,
                        AuditLogCategory.Authentication,
                        AuditLogAction.ForgotPassword,
                        $"AuthAccount:{authAccount.Id}:{authAccount.UserIdText}",
                        ipAddress,
                        transactionToken);

                    await _authEmailService.SendPasswordResetEmailAsync(
                        authAccount.Email,
                        token,
                        resetToken.ExpiresAt,
                        transactionToken);
                },
                cancellationToken);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            _resetPasswordValidator.Validate(request);

            var tokenHash = TokenUtil.HashToken(request.Token!);
            var resetToken = await _passwordResetTokenRepository
                .Query(tracking: true)
                .Include(t => t.AuthAccount)
                .ThenInclude(authAccount => authAccount.RefreshTokens)
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

            if (resetToken == null || resetToken.UsedAt != null || resetToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new InvalidDataException("Invalid or expired reset token");
            }

            resetToken.AuthAccount.PasswordHash = PasswordHashUtil.Hash(request.NewPassword!);
            resetToken.AuthAccount.FailedLoginCount = 0;
            resetToken.AuthAccount.LockedUntil = null;
            resetToken.UsedAt = DateTime.UtcNow;

            foreach (var refreshToken in resetToken.AuthAccount.RefreshTokens.Where(t => t.RevokedAt == null))
            {
                _authTokenService.RevokeRefreshToken(refreshToken, null, DateTime.UtcNow);
            }

            await _auditLogWriter.LogForActorAsync(
                resetToken.AuthAccount,
                AuditLogCategory.Authentication,
                AuditLogAction.ResetPassword,
                $"AuthAccount:{resetToken.AuthAccount.Id}:{resetToken.AuthAccount.UserIdText}",
                cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private async Task RecordFailedLoginAsync(AuthAccount authAccount, DateTime now, string? ipAddress, CancellationToken cancellationToken)
        {
            authAccount.FailedLoginCount++;
            if (authAccount.FailedLoginCount >= MaxFailedLoginAttempts)
            {
                authAccount.LockedUntil = now.Add(LockoutDuration);
            }

            await LogFailedLoginAsync(authAccount, ipAddress, cancellationToken, saveChanges: false);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private async Task LogFailedLoginAsync(
            AuthAccount authAccount,
            string? ipAddress,
            CancellationToken cancellationToken,
            bool saveChanges = true)
        {
            await _auditLogWriter.LogForActorAsync(
                authAccount,
                AuditLogCategory.Authentication,
                AuditLogAction.LoginFailed,
                $"AuthAccount:{authAccount.Id}:{authAccount.UserIdText}",
                ipAddress,
                cancellationToken);
            if (saveChanges)
            {
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }
        }

        private async Task LogFailedLoginAsync(
            string? userIdText,
            string? ipAddress,
            CancellationToken cancellationToken)
        {
            var actorUserIdText = string.IsNullOrWhiteSpace(userIdText)
                ? "Anonymous"
                : userIdText.Trim();
            await _auditLogWriter.LogAnonymousAsync(
                AuditLogCategory.Authentication,
                AuditLogAction.LoginFailed,
                $"UserIdText:{actorUserIdText}",
                actorUserIdText,
                actorUserIdText,
                ipAddress,
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private async Task<string> CreateSocialUserIdTextAsync(
            SocialLoginProvider provider,
            string providerUserId,
            CancellationToken cancellationToken)
        {
            var providerName = provider.ToString().ToLowerInvariant();
            var hash = TokenUtil.HashToken($"{provider}:{providerUserId}");
            var userIdText = $"social-{providerName}-{hash[..24]}";
            return !await _authAccountRepository.AnyAsync(
                authAccount => authAccount.UserIdText == userIdText,
                cancellationToken)
                ? userIdText
                : $"social-{providerName}-{Guid.NewGuid():N}";
        }

        private static string GenerateSocialFullName(string email)
        {
            var atIndex = email.IndexOf('@', StringComparison.Ordinal);
            return atIndex > 0
                ? email[..atIndex]
                : "Social User";
        }

    }
}

