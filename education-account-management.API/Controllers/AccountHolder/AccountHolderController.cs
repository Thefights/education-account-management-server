using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Interfaces.EducationAccounts;

namespace Controllers.AccountHolder
{
    [Authorize(Roles = RolePolicy.AccountHolder)]
    public class AccountHolderController(IEducationAccountService educationAccountService) : BaseController
    {
        private readonly IEducationAccountService _educationAccountService = educationAccountService;

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var result = await _educationAccountService.GetAccountHolderProfileAsync(cancellationToken);
            return Result.SuccessData(result);
        }
    }
}