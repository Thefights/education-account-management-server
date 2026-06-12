using Controllers.Base;
using DTOs.Email;
using Interfaces.Email;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class EmailWhitelistSettingController(
        IEmailWhitelistSettingService emailWhitelistSettingService)
        : BaseController
    {
        private readonly IEmailWhitelistSettingService _emailWhitelistSettingService = emailWhitelistSettingService;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _emailWhitelistSettingService.GetAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateEmailWhitelistSettingDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _emailWhitelistSettingService.UpdateAsync(updateDTO, cancellationToken);
            return Result.SuccessData(result, "Email whitelist setting updated successfully");
        }
    }
}
