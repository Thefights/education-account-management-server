using Controllers.Base;
using DTOs.Email;
using Interfaces.Email;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.Admin)]
    public class MfaSettingController(IMfaSettingService mfaSettingService) : BaseController
    {
        private readonly IMfaSettingService _mfaSettingService = mfaSettingService;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mfaSettingService.GetAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateMfaSettingDTO updateDTO, CancellationToken cancellationToken)
        {
            var result = await _mfaSettingService.UpdateAsync(updateDTO, cancellationToken);
            return Result.SuccessData(result, "MFA setting updated successfully");
        }
    }
}
