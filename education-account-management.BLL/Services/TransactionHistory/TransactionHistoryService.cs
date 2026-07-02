using DTOs.TransactionHistory;
using Filters.TransactionHistory;
using Interfaces.TransactionHistory;
using Results;
using Services.Base;
using System.Linq.Expressions;


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

            return await GetTransactionHistoryPaginatedAsync(
                filterDTO,
                BuildAccountSearchPredicate(educationAccountId, filterDTO.Search),
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

            return await GetTransactionHistoryPaginatedAsync(
                filterDTO,
                BuildAccountSearchPredicate(educationAccountId, filterDTO.Search),
                cancellationToken);
        }

        private async Task<PaginationResult<EducationCreditTransactionDTO>> GetTransactionHistoryPaginatedAsync(
            EducationCreditTransactionFilterDTO filterDTO,
            Expression<Func<EducationCreditTransaction, bool>> predicate,
            CancellationToken cancellationToken)
        {
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, items) = await _repository.GetProjectedPaginatedAsync(
                projection: query => query.Select(transaction => new EducationCreditTransactionDTO
                {
                    TransactionCode = transaction.TransactionCode,
                    Type = transaction.Type.ToString(),
                    Direction = transaction.Direction.ToString(),
                    PaymentMethod = transaction.Payment != null ? transaction.Payment.PaymentMethod : null,
                    Amount = transaction.Amount,
                    BalanceBefore = transaction.BalanceBefore,
                    BalanceAfter = transaction.BalanceAfter,
                    Description = transaction.Description,
                    CreatedAt = transaction.CreatedAt,
                    Receipt = transaction.Payment == null
                        ? null
                        : new EducationCreditTransactionReceiptDTO
                        {
                            PaymentMethod = transaction.Payment.PaymentMethod.ToString(),
                            AccountNumber = transaction.Payment.AccountNumberSnapshot,
                            CitizenNric = transaction.Payment.CitizenNricSnapshot,
                            CitizenFullName = transaction.Payment.CitizenFullNameSnapshot,
                            TotalAmount = transaction.Payment.TotalAmount,
                            PaidAt = transaction.Payment.PaidAt,
                            ExternalReference = transaction.Payment.ExternalReference,
                            Items = transaction.Payment.PaymentAllocations
                                .Select(allocation => new EducationCreditTransactionReceiptItemDTO
                                {
                                    CourseName = allocation.CourseNameSnapshot,
                                    SchoolName = allocation.SchoolNameSnapshot,
                                    InstallmentNumber = allocation.ChargeInstallment != null
                                        ? allocation.ChargeInstallment.InstallmentNumber
                                        : null,
                                    Amount = allocation.Amount
                                })
                                .ToList()
                        }
                }),
                filterExpr: predicate,
                filterStr: filterDTO.Filter,
                search: filterDTO.Search,
                searchFields: filterDTO.SearchFields,
                order: filterDTO.SortExpression,
                page: filterDTO.Page,
                pageSize: pageSize,
                includes: null,
                cancellationToken: cancellationToken);

            return new PaginationResult<EducationCreditTransactionDTO>(total, pageSize, items);
        }

        private static Expression<Func<EducationCreditTransaction, bool>> BuildAccountSearchPredicate(
            int educationAccountId,
            string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return transaction => transaction.EducationAccountId == educationAccountId;
            }

            var normalizedSearch = search.Trim().ToLowerInvariant();

            return transaction => transaction.EducationAccountId == educationAccountId
                && (
                    transaction.TransactionCode.ToString().ToLower().Contains(normalizedSearch)
                    || (transaction.Description != null
                        && transaction.Description.ToLower().Contains(normalizedSearch))
                );
        }
    }
}
