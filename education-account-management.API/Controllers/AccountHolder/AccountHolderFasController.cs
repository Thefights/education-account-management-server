using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasApplications;
using Filters.FasApplications;
using Filters.FasSchemes;
using Interfaces.FasApplications;
using Interfaces.FasSchemes;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.AccountHolder
{
    [Authorize(Roles = RolePolicy.AccountHolder)]
    [Route("api/v{version:apiVersion}/Account-Holder")]
    public class AccountHolderFasController(
        IAccountHolderFasSchemeService fasSchemeService,
        IAccountHolderFasApplicationService fasApplicationService) : BaseController
    {
        private readonly IAccountHolderFasSchemeService _fasSchemeService = fasSchemeService;
        private readonly IAccountHolderFasApplicationService _fasApplicationService = fasApplicationService;

        [HttpGet("fas-schemes/available")]
        public async Task<IActionResult> GetAvailableSchemes([FromQuery] FasSchemeFilterDTO filter, CancellationToken cancellationToken)
        {
            var result = await _fasSchemeService.GetAvailableSchemesAsync(filter, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPost("fas-applications")]
        public async Task<IActionResult> SubmitApplication([FromForm] SubmitFasApplicationDTO dto, CancellationToken cancellationToken)
        {
            var applicationNumber = await _fasApplicationService.SubmitApplicationAsync(dto, cancellationToken);
            return Result.SuccessAction($"FAS application successfully created with ID: {applicationNumber}");
        }

        [HttpPost("fas-applications/{id}/reapply-draft")]
        public async Task<IActionResult> CreateReapplyDraft([FromRoute] int id, CancellationToken cancellationToken)
        {
            var draftId = await _fasApplicationService.CreateReapplyDraftAsync(id, cancellationToken);
            return Result.SuccessData(new { id = draftId });
        }

        [HttpPost("fas-applications/{id}/publish")]
        public async Task<IActionResult> PublishDraftApplication([FromRoute] int id, [FromForm] SubmitFasApplicationDTO dto, CancellationToken cancellationToken)
        {
            var applicationNumber = await _fasApplicationService.PublishDraftApplicationAsync(id, dto, cancellationToken);
            return Result.SuccessAction($"FAS application successfully submitted with application number: {applicationNumber}");
        }

        [HttpGet("fas-applications")]
        public async Task<IActionResult> GetMyApplications([FromQuery] FasApplicationFilterDTO filter, CancellationToken cancellationToken)
        {
            var result = await _fasApplicationService.GetMyApplicationsAsync(filter, cancellationToken);
            return Result.SuccessData(result);
        }

        [HttpPost("fas-applications/withdraw/{id}")]
        public async Task<IActionResult> WithdrawApplication([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _fasApplicationService.WithdrawApplicationAsync(id, cancellationToken);
            return Result.SuccessAction("Application successfully withdrawn.");
        }

        [HttpGet("fas-applications/{id}")]
        public async Task<IActionResult> GetApplicationDetail([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _fasApplicationService.GetApplicationDetailAsync(id, cancellationToken);
            return Result.SuccessData(result);
        }
    }
}
