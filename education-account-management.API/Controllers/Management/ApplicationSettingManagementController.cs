using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.ApplicationSettings;
using Interfaces.ApplicationSettings;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SystemAdmin)]
    public class ApplicationSettingManagementController(IApplicationSettingService service) : BaseController
    {
        private readonly IApplicationSettingService _service = service;

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _service.GetAsync(cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateApplicationSettingDTO updateDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(updateDTO, cancellationToken);
            return Result.SuccessData(result, "Application settings updated successfully");
        }
    }
}
