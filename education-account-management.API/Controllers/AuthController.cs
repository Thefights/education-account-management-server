using Common.HttpResults;
using Controllers.Base;
using DTOs.Auth;
using Infrastructure.Interface;
using Interfaces.Auth;

namespace Controllers
{
    public class AuthController(
        IAuthService authService,
        IRefreshTokenCookieService refreshTokenCookieService,
        ICurrentTokenService currentTokenService)
        : BaseController
    {
        private readonly IAuthService _authService = authService;
        private readonly IRefreshTokenCookieService _refreshTokenCookieService = refreshTokenCookieService;
        private readonly ICurrentTokenService _currentTokenService = currentTokenService;

        [AllowAnonymous]
        [HttpPost("account-holder/mock-singpass-login")]
        public async Task<IActionResult> LoginWithMockSingpass(CancellationToken cancellationToken)
        {
            var result = await _authService.LoginWithMockSingpassAsync(cancellationToken);
            _refreshTokenCookieService.Set(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Result.SuccessData(result, "Login successful");
        }

        [AllowAnonymous]
        [HttpPost("admin/azure-ad-login")]
        public async Task<IActionResult> LoginWithAzureAd(
            AzureAdLoginRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.LoginWithAzureAdAsync(request, cancellationToken);
            _refreshTokenCookieService.Set(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Result.SuccessData(result, "Login successful");
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(
                _refreshTokenCookieService.Get()!,
                _currentTokenService.AccessToken!,
                cancellationToken);

            _refreshTokenCookieService.Clear();
            return Result.SuccessAction("Logout successfully");
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var result = await _authService.RefreshTokenAsync(
                _refreshTokenCookieService.Get()!,
                cancellationToken);

            _refreshTokenCookieService.Set(result.RefreshToken, result.RefreshTokenExpiresAt);
            return Result.SuccessData(result, "Token refreshed successfully");
        }
    }
}
