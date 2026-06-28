using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasApplications;
using Utils;
using Interfaces.FasApplications;
using Filters.FasApplications;


namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class FasApplicationManagementController(
        IFasApplicationManagementService service) : BaseController
    {
        private readonly IFasApplicationManagementService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetApplicationPaginated(
            [FromQuery] FasApplicationFilterDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetApplicationPaginatedAsync(request, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationDetails(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetApplicationDetailsAsync(id, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
        {
            await _service.ApproveAsync(id, cancellationToken);
            return Result.SuccessAction("FAS application approved successfully.");
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken)
        {
            await _service.RejectAsync( id, dto, cancellationToken);
            return Result.SuccessAction("FAS application rejected successfully.");
        }
    }
}
