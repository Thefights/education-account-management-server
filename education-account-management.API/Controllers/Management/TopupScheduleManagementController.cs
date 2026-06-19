using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.TopUp.Schedule;
using Filters.TopUp;
using Interfaces.TopUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.FinanceAdmin)]
    public class TopupScheduleManagementController(ITopupScheduleService topupScheduleService) :
        CrudController<CreateTopupScheduleDTO, GetTopupScheduleDTO, UpdateTopupScheduleDTO, TopupScheduleFilterDTO>(topupScheduleService)
    {
        protected override string EntityName => nameof(TopupSchedule);
    }
}
