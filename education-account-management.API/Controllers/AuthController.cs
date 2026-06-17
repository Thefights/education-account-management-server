using Common.HttpResults;
using Controllers.Base;
using DTOs.Auth;
using Infrastructure.Interface;
using Interfaces.Auth;

namespace Controllers
{
    [AllowAnonymous]
    public class AuthController(
        IAuthService authService,
        IRefreshTokenCookieService refreshTokenCookieService)
        : BaseController
    {
        private readonly IAuthService _authService = authService;
        private readonly IRefreshTokenCookieService _refreshTokenCookieService = refreshTokenCookieService;

        [HttpPost("account-holder/mock-singpass-login")]
        public async Task<IActionResult> LoginWithMockSingpass(CancellationToken cancellationToken)
        {
            var result = await _authService.LoginWithMockSingpassAsync(cancellationToken);
            _refreshTokenCookieService.Set(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Result.SuccessData(result, "Login successful");
        }

        [HttpPost("admin/azure-ad-login")]
        public async Task<IActionResult> LoginWithAzureAd(
            AzureAdLoginRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.LoginWithAzureAdAsync(request, cancellationToken);
            _refreshTokenCookieService.Set(result.RefreshToken, result.RefreshTokenExpiresAt);

            return Result.SuccessData(result, "Login successful");
        }
    }
}
