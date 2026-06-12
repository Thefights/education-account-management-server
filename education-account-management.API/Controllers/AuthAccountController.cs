using Controllers.Base;
using DTOs.Auth;
using Interfaces.Auth;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.AdminOrTenantUser)]
    public class AuthAccountController(IAuthAccountService authAccountService) : BaseController
    {
        private readonly IAuthAccountService _authAccountService = authAccountService;

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentAuthAccount(CancellationToken cancellationToken)
        {
            var result = await _authAccountService.GetCurrentAuthAccountAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentAuthAccount(
            UpdateAuthAccountProfileDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _authAccountService.UpdateCurrentAuthAccountAsync(updateDTO, cancellationToken);
            return Result.SuccessData(result, "AuthAccount updated successfully");
        }
    }
}
