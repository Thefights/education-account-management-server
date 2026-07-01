using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasApplications;
using Filters.FasApplications;
using Interfaces.FasApplications;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class FasApplicationManagementController(
        IFasApplicationManagementService service) : GetController<GetFasApplicationSchoolAdminDTO, FasApplicationFilterDTO>(service)
    {
        private readonly IFasApplicationManagementService _service = service;

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _service.GetApplicationDetailsAsync(id, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(
            int id,
            ApproveFasApplicationDTO dto,
            CancellationToken cancellationToken)
        {
            await _service.ApproveAsync(id, dto, cancellationToken);
            return Result.SuccessAction("FAS application approved successfully.");
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken)
        {
            await _service.RejectAsync(id, dto, cancellationToken);
            return Result.SuccessAction("FAS application rejected successfully.");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public override Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return base.GetAll(cancellationToken);
        }
    }
}
