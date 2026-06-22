using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.TopUp;
using Filters.TopUp;
using Interfaces.TopUp;
using Models;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.FinanceAdmin)]
    public class TopupScheduleManagementController(ITopupScheduleService topupScheduleService) :
        CrudController<CreateTopupScheduleDTO, GetTopupScheduleDTO, UpdateTopupScheduleDTO, TopupScheduleFilterDTO>(topupScheduleService)
    {
        private readonly ITopupScheduleService _topupScheduleService = topupScheduleService;

        protected override string EntityName => nameof(TopupSchedule);

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(BatchUpdateTopupScheduleStatusDTO dto, CancellationToken cancellationToken)
        {
            await _topupScheduleService.UpdateSchedulesStatusAsync(dto, cancellationToken);
            return Result.SuccessAction("Top-up schedules status updated successfully.");
        }
    }
}
