using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasApplications;
using Utils;
using Interfaces.FasApplications;


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

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            await _service.ApproveAsync(adminSchoolId, id, cancellationToken);
            return Result.SuccessAction("FAS application approved successfully.");
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            await _service.RejectAsync(adminSchoolId, id, dto, cancellationToken);
            return Result.SuccessAction("FAS application rejected successfully.");
        }
    }
}
