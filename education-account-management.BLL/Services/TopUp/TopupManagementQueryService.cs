using DTOs.TopUp;
using Filters.TopUp;
using Interfaces.TopUp;
using Results;

namespace Services.TopUp;

public sealed class TopupManagementQueryService(IUnitOfWork unitOfWork) : ITopupManagementQueryService
{
    private readonly IGenericRepository<EducationAccount> _accountRepository =
        unitOfWork.Repository<EducationAccount>();
    private readonly IGenericRepository<TopupExecution> _executionRepository =
        unitOfWork.Repository<TopupExecution>();
    private readonly IGenericRepository<TopupExecutionTarget> _targetRepository =
        unitOfWork.Repository<TopupExecutionTarget>();

    public async Task<PaginationResult<TopupEligibleAccountDTO>> GetEligibleAccountsAsync(
        TopupAccountLookupFilterDTO filter,
        CancellationToken cancellationToken = default)
    {
        var page = Math.Max(filter.Page, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        var query = _accountRepository.Query()
            .Where(account => account.Status == EducationAccountStatus.Active);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(account =>
                account.AccountNumber.Contains(search) ||
                account.Citizen.Nric.Contains(search) ||
                account.Citizen.FullName.Contains(search));
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(account => account.AccountNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(account => new TopupEligibleAccountDTO
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                Nric = account.Citizen.Nric,
                Name = account.Citizen.FullName,
                Balance = account.EducationCreditBalance
            })
            .ToListAsync(cancellationToken);

        return new PaginationResult<TopupEligibleAccountDTO>(total, pageSize, items);
    }

    public async Task<PaginationResult<TopupExecutionDTO>> GetHistoryAsync(
        TopupExecutionFilterDTO filter,
        CancellationToken cancellationToken = default)
    {
        var page = Math.Max(filter.Page, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        var query = ApplyHistoryFilter(_executionRepository.Query(), filter);
        var total = await query.CountAsync(cancellationToken);
        var items = await ApplyHistoryOrder(query, filter.Sort)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToExecutionDTO())
            .ToListAsync(cancellationToken);

        return new PaginationResult<TopupExecutionDTO>(total, pageSize, items);
    }

    public async Task<TopupExecutionDTO> GetHistoryByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _executionRepository.Query()
            .Where(execution => execution.Id == id)
            .Select(ToExecutionDTO())
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new DataNotFoundException(typeof(TopupExecution), id);
    }

    public async Task<PaginationResult<TopupExecutionTargetDTO>> GetTargetsAsync(
        int executionId,
        TopupExecutionTargetFilterDTO filter,
        CancellationToken cancellationToken = default)
    {
        if (!await _executionRepository.AnyAsync(
                execution => execution.Id == executionId,
                cancellationToken))
        {
            throw new DataNotFoundException(typeof(TopupExecution), executionId);
        }

        var page = Math.Max(filter.Page, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        var query = _targetRepository.Query()
            .Where(target => target.TopupExecutionId == executionId);

        if (filter.Statuses is { Count: > 0 })
        {
            query = query.Where(target => filter.Statuses.Contains(target.Status));
        }
        if (!string.IsNullOrWhiteSpace(filter.AccountNumber))
        {
            var accountNumber = filter.AccountNumber.Trim();
            query = query.Where(target => target.AccountNumber.Contains(accountNumber));
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await ApplyTargetOrder(query, filter.Sort)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(target => new TopupExecutionTargetDTO
            {
                Id = target.Id,
                EducationAccountId = target.EducationAccountId,
                AccountNumber = target.AccountNumber,
                AccountName = target.EducationAccount != null
                    ? target.EducationAccount.Citizen.FullName
                    : string.Empty,
                Amount = target.Amount,
                Status = target.Status.ToString(),
                FailureReason = target.FailureReason,
                TransactionCode = target.EducationCreditTransaction != null
                    ? target.EducationCreditTransaction.TransactionCode
                    : null,
                CreatedAt = target.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new PaginationResult<TopupExecutionTargetDTO>(total, pageSize, items);
    }

    private static IQueryable<TopupExecution> ApplyHistoryFilter(
        IQueryable<TopupExecution> query,
        TopupExecutionFilterDTO filter)
    {
        if (filter.SourceTypes is { Count: > 0 })
            query = query.Where(execution => filter.SourceTypes.Contains(execution.SourceType));
        if (filter.Statuses is { Count: > 0 })
            query = query.Where(execution => filter.Statuses.Contains(execution.Status));
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(execution =>
                execution.ExecutionCode.Contains(search) ||
                (execution.TopupNameSnapshot != null && execution.TopupNameSnapshot.Contains(search)));
        }
        if (!string.IsNullOrWhiteSpace(filter.AccountNumber))
        {
            var accountNumber = filter.AccountNumber.Trim();
            query = query.Where(execution =>
                execution.Targets.Any(target => target.AccountNumber.Contains(accountNumber)));
        }
        if (filter.CreatedFrom.HasValue)
        {
            var createdFrom = filter.CreatedFrom.Value.Date;
            query = query.Where(execution => execution.CreatedAt >= createdFrom);
        }
        if (filter.CreatedTo.HasValue)
        {
            var createdTo = filter.CreatedTo.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(execution => execution.CreatedAt <= createdTo);
        }
        return query;
    }

    private static IOrderedQueryable<TopupExecution> ApplyHistoryOrder(
        IQueryable<TopupExecution> query,
        string? sort)
    {
        var descending = sort?.EndsWith(" desc", StringComparison.OrdinalIgnoreCase) != false;
        var field = sort?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.ToLowerInvariant();
        return field switch
        {
            "executioncode" => descending ? query.OrderByDescending(x => x.ExecutionCode) : query.OrderBy(x => x.ExecutionCode),
            "sourcetype" => descending ? query.OrderByDescending(x => x.SourceType) : query.OrderBy(x => x.SourceType),
            "status" => descending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
            "totalexecutedamount" => descending ? query.OrderByDescending(x => x.TotalExecutedAmount) : query.OrderBy(x => x.TotalExecutedAmount),
            _ => descending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt)
        };
    }

    private static IOrderedQueryable<TopupExecutionTarget> ApplyTargetOrder(
        IQueryable<TopupExecutionTarget> query,
        string? sort)
    {
        var descending = sort?.EndsWith(" desc", StringComparison.OrdinalIgnoreCase) == true;
        var field = sort?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.ToLowerInvariant();
        return field switch
        {
            "accountnumber" => descending ? query.OrderByDescending(x => x.AccountNumber) : query.OrderBy(x => x.AccountNumber),
            "amount" => descending ? query.OrderByDescending(x => x.Amount) : query.OrderBy(x => x.Amount),
            "status" => descending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
            "createdat" => descending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.Id)
        };
    }

    private static System.Linq.Expressions.Expression<Func<TopupExecution, TopupExecutionDTO>> ToExecutionDTO()
    {
        return execution => new TopupExecutionDTO
        {
            Id = execution.Id,
            ExecutionCode = execution.ExecutionCode,
            SourceType = execution.SourceType.ToString(),
            Status = execution.Status.ToString(),
            SystemTopupId = execution.SystemTopupId,
            ScheduleTopUpId = execution.ScheduleTopUpId,
            ManualAmount = execution.ManualAmount,
            ManualReason = execution.ManualReason,
            TotalTargetCount = execution.TotalTargetCount,
            SuccessCount = execution.SuccessCount,
            FailedCount = execution.FailedCount,
            TotalExecutedAmount = execution.TotalExecutedAmount,
            TopupNameSnapshot = execution.TopupNameSnapshot,
            TopupAmountSnapshot = execution.TopupAmountSnapshot,
            ConditionsSnapshot = execution.ConditionsSnapshot,
            CreatedAt = execution.CreatedAt,
            UpdatedAt = execution.UpdatedAt
        };
    }
}