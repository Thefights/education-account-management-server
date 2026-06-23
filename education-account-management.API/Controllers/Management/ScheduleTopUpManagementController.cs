using Authorization;
using Common.HttpResults;
using Controllers.Base;

namespace Controllers.Management;

[Authorize(Roles = RolePolicy.FinanceAdmin)]
public class ScheduleTopUpManagementController(IScheduleTopUpService scheduleTopUpService)
    : CrudController<CreateScheduleTopUpDTO, GetScheduleTopUpDTO, UpdateScheduleTopUpDTO, ScheduleTopUpFilterDTO>(scheduleTopUpService)
{
    private readonly IScheduleTopUpService _scheduleTopUpService = scheduleTopUpService;

    protected override string EntityName => nameof(ScheduleTopUp);

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(BatchUpdateScheduleTopUpStatusDTO dto, CancellationToken cancellationToken)
    {
        await _scheduleTopUpService.UpdateStatusesAsync(dto, cancellationToken);
        return Result.SuccessAction("Scheduled top-up statuses updated successfully.");
    }
}