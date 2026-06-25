using Authorization;
using Common.HttpResults;
using Controllers.Base;
using DTOs.FasApplications;
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
        public async Task<IActionResult> SubmitApplication([FromBody] SubmitFasApplicationDTO dto, CancellationToken cancellationToken)
        {
            var applicationNumber = await _fasApplicationService.SubmitApplicationAsync(dto, cancellationToken);
            return Result.SuccessAction($"FAS application successfully created with ID: {applicationNumber}");
        }
    }
}
