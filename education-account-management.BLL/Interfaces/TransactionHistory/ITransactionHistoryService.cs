using DTOs.TransactionHistory;
using Filters.TransactionHistory;
using Results;

namespace Interfaces.TransactionHistory
{
    public interface ITransactionHistoryService
    {
        Task<PaginationResult<EducationCreditTransactionDTO>> GetForEducationAccountAsync(
            int educationAccountId,
            EducationCreditTransactionFilterDTO filterDTO,
            CancellationToken cancellationToken = default);

        Task<PaginationResult<EducationCreditTransactionDTO>> GetForCurrentAccountHolderAsync(
            EducationCreditTransactionFilterDTO filterDTO,
            CancellationToken cancellationToken = default);
    }
}
