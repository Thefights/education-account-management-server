using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.TopUp.Rule;
using Interfaces.TopUp;
using Models;

namespace Controllers.Management
{
    [Authorize(Roles = RolePolicy.FinanceAdmin)]
    public class TopupRuleManagementController(ITopupRuleService topupRuleService)
        : CrudController<CreateTopupRuleDTO, GetTopupRuleDTO, UpdateTopupRuleDTO>(topupRuleService)
    {
        private readonly ITopupRuleService _topupRuleService = topupRuleService;

        protected override string? EntityName => nameof(TopupRule);

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(BatchUpdateTopupRuleStatusDTO dto, CancellationToken cancellationToken)
        {
            await _topupRuleService.UpdateRulesStatusAsync(dto, cancellationToken);
            return Result.SuccessAction("Top-up rules status updated successfully.");
        }
    }
}
