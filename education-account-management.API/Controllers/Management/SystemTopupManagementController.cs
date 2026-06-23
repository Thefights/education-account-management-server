using Authorization;
using Common.HttpResults;
using Controllers.Base;

namespace Controllers.Management;

[Authorize(Roles = RolePolicy.FinanceAdmin)]
public class SystemTopupManagementController(ISystemTopupService systemTopupService)
    : CrudController<CreateSystemTopupDTO, GetSystemTopupDTO, UpdateSystemTopupDTO, SystemTopupFilterDTO>(systemTopupService)
{
    private readonly ISystemTopupService _systemTopupService = systemTopupService;

    protected override string EntityName => nameof(SystemTopup);

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(BatchUpdateSystemTopupStatusDTO dto, CancellationToken cancellationToken)
    {
        await _systemTopupService.UpdateStatusesAsync(dto, cancellationToken);
        return Result.SuccessAction("System top-up statuses updated successfully.");
    }
}