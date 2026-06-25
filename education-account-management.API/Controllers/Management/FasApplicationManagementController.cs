using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasApplications;
using Interfaces.FasApplications;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class FasApplicationManagementController(IManagementFasApplicationService service) : BaseController
    {
        private readonly IManagementFasApplicationService _service = service;

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
        {
            await _service.ApproveAsync(id, cancellationToken);
            return Result.SuccessAction("FAS application approved successfully.");
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken)
        {
            await _service.RejectAsync(id, dto, cancellationToken);
            return Result.SuccessAction("FAS application rejected successfully.");
        }
    }
}
