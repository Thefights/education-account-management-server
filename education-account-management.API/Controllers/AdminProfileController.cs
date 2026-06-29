using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Interfaces.Admin;

namespace Controllers
{
    [Authorize(Roles = RolePolicy.FullAdmin)]
    public class AdminProfileController(IAdminService adminService) : BaseController
    {
        private readonly IAdminService _adminService = adminService;

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
        {
            return Result.SuccessData(await _adminService.GetCurrentProfileAsync(cancellationToken));
        }
    }
}
