using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasApplications;
using Interfaces.FasApplications.Management;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class FasApplicationManagementController(
        IManagementFasApplicationService service,
        SchoolScopeResolver schoolScopeResolver) : BaseController
    {
        private readonly IManagementFasApplicationService _service = service;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;

        [HttpGet]
        public async Task<IActionResult> GetApplicationQueue(
            [FromQuery] GetFasApplicationListRequestDTO request,
            CancellationToken cancellationToken)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var result = await _service.GetApplicationQueueAsync(request, adminSchoolId, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationDetails(
            int id,
            CancellationToken cancellationToken)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var result = await _service.GetApplicationDetailsAsync(id, adminSchoolId, cancellationToken);
            return Result.SuccessData(result);
        }
    }
}
