using Controllers.Base;
using DTOs.Auth;
using Interfaces.Auth;

namespace Controllers
{
    public class AuthController(
        IAuthService authService,
        ICurrentTokenService currentTokenService,
        IAuthRegistrationOtpService authRegistrationOtpService,
        IRefreshTokenCookieService refreshTokenCookieService)
        : BaseController
    {
        private readonly IAuthService _authService = authService;
        private readonly ICurrentTokenService _currentTokenService = currentTokenService;
        private readonly IAuthRegistrationOtpService _authRegistrationOtpService = authRegistrationOtpService;
        private readonly IRefreshTokenCookieService _refreshTokenCookieService = refreshTokenCookieService;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request, CancellationToken cancellationToken)
        {
            await _authService.RegisterAsync(request, cancellationToken);
            return Result.SuccessAction("User registered successfully");
        }

        [AllowAnonymous]
        [HttpPost("register/email-otp/send")]
        public async Task<IActionResult> SendRegisterEmailOtp(
            SendRegisterEmailOtpRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _authRegistrationOtpService.SendEmailOtpAsync(request, cancellationToken);
            return Result.SuccessData(result, "Registration email OTP sent successfully");
        }

        [AllowAnonymous]
        [HttpPost("register/email-otp/verify")]
        public async Task<IActionResult> VerifyRegisterEmailOtp(
            VerifyRegisterEmailOtpRequestDTO request,
            CancellationToken cancellationToken)
        {
            await _authRegistrationOtpService.VerifyEmailOtpAsync(request, cancellationToken);
            return Result.SuccessAction("Registration email verified successfully");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(
                request,
                this.HttpContext.Connection.RemoteIpAddress?.ToString(),
                this.HttpContext.Request.Headers.UserAgent.ToString(),
                cancellationToken);

            if (!result.MfaRequired && result.Tokens != null)
            {
                _refreshTokenCookieService.Set(result.Tokens);
            }

            return Result.SuccessData(result, "Login successfully");
        }

        [AllowAnonymous]
        [HttpPost("social-login")]
        public async Task<IActionResult> SocialLogin(SocialLoginRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.SocialLoginAsync(
                request,
                this.HttpContext.Connection.RemoteIpAddress?.ToString(),
                this.HttpContext.Request.Headers.UserAgent.ToString(),
                cancellationToken);

            _refreshTokenCookieService.Set(result);
            return Result.SuccessData(result, "Social login successfully");
        }

        [Authorize(Roles = RolePolicy.AdminOrTenantUser)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(
                _refreshTokenCookieService.RefreshToken,
                _currentTokenService.AccessToken,
                this.HttpContext.Connection.RemoteIpAddress?.ToString(),
                cancellationToken);

            _refreshTokenCookieService.Clear();
            return Result.SuccessAction("Logout successfully");
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var result = await _authService.RefreshTokenAsync(
                _refreshTokenCookieService.RefreshToken,
                this.HttpContext.Connection.RemoteIpAddress?.ToString(),
                this.HttpContext.Request.Headers.UserAgent.ToString(),
                cancellationToken);

            _refreshTokenCookieService.Set(result);
            return Result.SuccessData(result, "Token refreshed successfully");
        }

        [AllowAnonymous]
        [HttpPost("mfa/verify")]
        public async Task<IActionResult> VerifyMfaOtp(VerifyMfaOtpRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.VerifyMfaOtpAsync(
                request,
                this.HttpContext.Connection.RemoteIpAddress?.ToString(),
                this.HttpContext.Request.Headers.UserAgent.ToString(),
                cancellationToken);

            _refreshTokenCookieService.Set(result);
            return Result.SuccessData(result, "MFA verified successfully");
        }

        [AllowAnonymous]
        [HttpPost("mfa/resend")]
        public async Task<IActionResult> ResendMfaOtp(ResendMfaOtpRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.ResendMfaOtpAsync(request, cancellationToken);
            return Result.SuccessData(result, "MFA OTP resent successfully");
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO request, CancellationToken cancellationToken)
        {
            await _authService.ForgotPasswordAsync(
                request,
                this.HttpContext.Connection.RemoteIpAddress?.ToString(),
                this.HttpContext.Request.Headers.UserAgent.ToString(),
                cancellationToken);

            return Result.SuccessAction("If this email is registered, a reset link has been sent.");
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO request, CancellationToken cancellationToken)
        {
            await _authService.ResetPasswordAsync(request, cancellationToken);
            return Result.SuccessAction("Password reset successfully");
        }

    }
}
