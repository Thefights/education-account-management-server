using Authorization;
using Common.HttpResults;
using Controllers.Base;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.SchoolAdmin)]
    public class EnrollmentManagementController(IEnrollmentService service)
        : GetController<GetEnrollmentDTO, EnrollmentFilterDTO>(service)
    {
        private readonly IEnrollmentService _service = service;

        [HttpPost]
        public async Task<IActionResult> Assign(
            AssignEnrollmentsDTO assignDTO,
            CancellationToken cancellationToken)
        {
            var result = await _service.AssignAsync(assignDTO, cancellationToken);
            return Result.SuccessData(result, $"{result.Count} student(s) assigned successfully");
        }

        [HttpDelete("selected")]
        public async Task<IActionResult> RemoveSelected(
            RemoveSelectedEnrollmentsDTO removeDTO,
            CancellationToken cancellationToken)
        {
            await _service.RemoveSelectedAsync(removeDTO, cancellationToken);
            return Result.SuccessAction($"{removeDTO.Ids.Count} enrollment(s) removed successfully");
        }

        [HttpPut("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(
            int id,
            CancellationToken cancellationToken)
        {
            await _service.WithdrawAsync(id, cancellationToken);
            return Result.SuccessAction("Student withdrawn successfully");
        }
    }
}
