using DTOs.TransactionHistory;
using Filters.TransactionHistory;
using Interfaces.TransactionHistory;
using Results;
using Services.Base;

namespace Services.TransactionHistory
{
    public class TransactionHistoryService(
        IUnitOfWork unitOfWork,
        TransactionHistoryMapper mapper,
        ICurrentUserService currentUserService)
        : BaseGetService<EducationCreditTransaction, EducationCreditTransactionDTO>(unitOfWork, mapper),
          ITransactionHistoryService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<PaginationResult<EducationCreditTransactionDTO>> GetForEducationAccountAsync(
            int educationAccountId,
            EducationCreditTransactionFilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            if (!await _unitOfWork.Repository<EducationAccount>().AnyAsync(
                    account => account.Id == educationAccountId,
                    cancellationToken))
            {
                throw new DataNotFoundException(typeof(EducationAccount), educationAccountId);
            }

            return await GetAllPaginatedAsync(
                filterDTO,
                transaction => transaction.EducationAccountId == educationAccountId,
                cancellationToken);
        }

        public async Task<PaginationResult<EducationCreditTransactionDTO>> GetForCurrentAccountHolderAsync(
            EducationCreditTransactionFilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            var currentAccountHolderId = _currentUserService.UserId;
            var educationAccountId = await _unitOfWork.Repository<EducationAccount>()
                .Query()
                .Where(account => account.Citizen.User != null
                    && account.Citizen.User.Id == currentAccountHolderId)
                .Select(account => account.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (educationAccountId == 0)
            {
                throw new DataNotFoundException("Education account for the current account holder was not found.");
            }

            return await GetAllPaginatedAsync(
                filterDTO,
                transaction => transaction.EducationAccountId == educationAccountId,
                cancellationToken);
        }
    }
}
