using Authorization;
using Common.HttpResults;
using Controllers.Base;
using Filters.TransactionHistory;
using Interfaces.TransactionHistory;

namespace Controllers
{
    [Authorize]
    public class TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        : BaseController
    {
        private readonly ITransactionHistoryService _transactionHistoryService = transactionHistoryService;

        [Authorize(Roles = RolePolicy.SystemAdmin)]
        [HttpGet("admin/education-accounts/{educationAccountId:int}")]
        public async Task<IActionResult> GetForEducationAccount(
            int educationAccountId,
            [FromQuery] EducationCreditTransactionFilterDTO filterDTO,
            CancellationToken cancellationToken)
        {
            var result = await _transactionHistoryService.GetForEducationAccountAsync(
                educationAccountId,
                filterDTO,
                cancellationToken);

            return Result.SuccessData(result);
        }

        [Authorize(Roles = RolePolicy.AccountHolder)]
        [HttpGet("account-holder/current")]
        public async Task<IActionResult> GetForCurrentAccountHolder(
            [FromQuery] EducationCreditTransactionFilterDTO filterDTO,
            CancellationToken cancellationToken)
        {
            var result = await _transactionHistoryService.GetForCurrentAccountHolderAsync(
                filterDTO,
                cancellationToken);

            return Result.SuccessData(result);
        }
    }
}
