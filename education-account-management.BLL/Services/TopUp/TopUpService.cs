using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;

namespace Services.TopUp;

public class TopupService(
    IUnitOfWork unitOfWork,
    IAuditLogWriter auditLogWriter)
    : ITopupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
    private readonly IGenericRepository<EducationAccount> _accountRepository = unitOfWork.Repository<EducationAccount>();
    private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
    private readonly IGenericRepository<TopupExecution> _executionRepository = unitOfWork.Repository<TopupExecution>();
    private readonly IGenericRepository<TopupExecutionTarget> _targetRepository = unitOfWork.Repository<TopupExecutionTarget>();

    public async Task<ExecuteTopupResultDTO> ExecuteManualTopupAsync(
        ManualTopupRequestDTO request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateRequest(request);

        var existingExecution = await _executionRepository.Query()
            .Include(execution => execution.Targets)
                .ThenInclude(target => target.EducationAccount)
                .ThenInclude(account => account!.Citizen)
            .Include(execution => execution.Targets)
                .ThenInclude(target => target.EducationCreditTransaction)
            .FirstOrDefaultAsync(
                execution => execution.IdempotencyKey == request.IdempotencyKey,
                cancellationToken);
        if (existingExecution != null)
            return BuildResult(existingExecution);

        var inputs = request.AccountIds is { Count: > 0 }
            ? await ResolveSelectedInputsAsync(request.AccountIds, cancellationToken)
            : await ResolveCsvInputsAsync(request.File!, cancellationToken);

        var execution = new TopupExecution
        {
            ExecutionCode = $"EXEC-MANUAL-{Guid.NewGuid():N}"[..22].ToUpperInvariant(),
            SourceType = TopupExecutionSourceType.Manual,
            IdempotencyKey = request.IdempotencyKey,
            ManualAmount = request.TopUpAmount,
            ManualReason = request.DisbursementReason,
            Status = TopupExecutionStatus.Pending,
            TotalTargetCount = inputs.Count
        };
        execution.TryValidate();
        await _executionRepository.AddAsync(execution, cancellationToken);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

        execution.Status = TopupExecutionStatus.Executing;
        _executionRepository.Update(execution);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

        var result = new ExecuteTopupResultDTO
        {
            BatchId = execution.Id,
            TotalProcessed = inputs.Count
        };

        foreach (var input in inputs)
        {
            if (input.Error != null || input.Account == null)
            {
                await AddFailureAsync(execution, input, request.TopUpAmount,
                    input.Error ?? "Account not found.", result, cancellationToken);
                continue;
            }

            var account = input.Account;
            if (account.Status != EducationAccountStatus.Active)
            {
                await AddFailureAsync(execution, input, request.TopUpAmount,
                    $"Account is not Active (current status: {account.Status}).", result, cancellationToken);
                continue;
            }

            try
            {
                var success = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
                {
                    var balanceBefore = account.EducationCreditBalance;
                    var balanceAfter = balanceBefore + request.TopUpAmount;
                    account.EducationCreditBalance = balanceAfter;
                    _accountRepository.Update(account);

                    var transaction = new EducationCreditTransaction
                    {
                        Type = EducationCreditTransactionType.Topup,
                        Direction = EducationCreditTransactionDirection.Credit,
                        Amount = request.TopUpAmount,
                        BalanceBefore = balanceBefore,
                        BalanceAfter = balanceAfter,
                        Description = request.DisbursementReason,
                        EducationAccountId = account.Id
                    };
                    transaction.TryValidate();
                    await _transactionRepository.AddAsync(transaction, token);

                    var target = new TopupExecutionTarget
                    {
                        TopupExecutionId = execution.Id,
                        EducationAccountId = account.Id,
                        AccountNumber = account.AccountNumber,
                        Amount = request.TopUpAmount,
                        Status = TopupTargetStatus.Success,
                        EducationCreditTransaction = transaction
                    };
                    target.TryValidate();
                    await _targetRepository.AddAsync(target, token);

                    return new TopupSuccessItemDTO
                    {
                        AccountId = account.Id,
                        AccountNumber = account.AccountNumber,
                        AccountName = account.Citizen.FullName,
                        TopUpAmount = request.TopUpAmount,
                        TopUpTransactionId = transaction.TransactionCode
                    };
                }, cancellationToken);

                execution.SuccessCount++;
                execution.TotalExecutedAmount += request.TopUpAmount;
                result.SuccessList.Add(success);
                await LogAuditAsync("Success", account, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                await AddFailureAsync(execution, input, request.TopUpAmount,
                    $"Execution failed: {exception.GetBaseException().Message}", result, cancellationToken);
            }
        }

        execution.Status = TopupExecutionStatus.Completed;
        _executionRepository.Update(execution);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

        result.TotalSuccess = execution.SuccessCount;
        result.TotalFailed = execution.FailedCount;
        result.TotalAmountCredited = execution.TotalExecutedAmount;
        return result;
    }

    private async Task<List<ManualInput>> ResolveSelectedInputsAsync(
        List<int> accountIds,
        CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.Query(tracking: true)
            .Include(account => account.Citizen)
            .Where(account => accountIds.Contains(account.Id))
            .ToListAsync(cancellationToken);
        var byId = accounts.ToDictionary(account => account.Id);
        var seen = new HashSet<int>();

        return accountIds.Select(id =>
        {
            byId.TryGetValue(id, out var account);
            var error = !seen.Add(id)
                ? "Duplicate account in the selected list."
                : account == null ? "Account not found." : null;
            return new ManualInput(account, account?.AccountNumber ?? $"ID:{id}", error, account != null && error == null);
        }).ToList();
    }

    private async Task<List<ManualInput>> ResolveCsvInputsAsync(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var fileErrors = CsvImportHelper.ValidateFile(file);
        if (fileErrors.Count != 0)
            throw new UserFacingException(string.Join(" ", fileErrors.Select(error => error.Message)), 400);

        var rows = CsvImportHelper.ReadRows<ManualTopupImportRowDTO>(file);
        var accountNumbers = rows.Items
            .Select(item => item.Row.AccountNumber?.Trim())
            .Where(accountNumber => !string.IsNullOrWhiteSpace(accountNumber))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var accounts = await _accountRepository.Query(tracking: true)
            .Include(account => account.Citizen)
            .Where(account => accountNumbers.Contains(account.AccountNumber))
            .ToListAsync(cancellationToken);
        var byNumber = accounts.ToDictionary(account => account.AccountNumber, StringComparer.OrdinalIgnoreCase);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var inputs = new List<ManualInput>();

        foreach (var item in rows.Items)
        {
            var accountNumber = item.Row.AccountNumber?.Trim() ?? string.Empty;
            byNumber.TryGetValue(accountNumber, out var account);
            var error = string.IsNullOrWhiteSpace(accountNumber)
                ? "Account Number is required."
                : !seen.Add(accountNumber)
                    ? "Duplicate account in the CSV file."
                    : account == null ? "Account not found." : null;
            inputs.Add(new ManualInput(account, accountNumber, error,
                !string.IsNullOrWhiteSpace(accountNumber) && error != "Duplicate account in the CSV file."));
        }

        inputs.AddRange(rows.Errors.Select(error =>
            new ManualInput(null, string.Empty, $"CSV row {error.RowNumber}: {error.Message}", false)));
        return inputs;
    }

    private async Task AddFailureAsync(
        TopupExecution execution,
        ManualInput input,
        decimal amount,
        string reason,
        ExecuteTopupResultDTO result,
        CancellationToken cancellationToken)
    {
        execution.FailedCount++;
        var truncatedReason = reason[..Math.Min(500, reason.Length)];
        if (input.PersistFailure && !string.IsNullOrWhiteSpace(input.AccountNumber))
        {
            var target = new TopupExecutionTarget
            {
                TopupExecutionId = execution.Id,
                EducationAccountId = input.Account?.Id,
                AccountNumber = input.AccountNumber,
                Amount = amount,
                Status = TopupTargetStatus.Failed,
                FailureReason = truncatedReason
            };
            target.TryValidate();
            await _targetRepository.AddAsync(target, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        result.FailList.Add(new TopupFailItemDTO
        {
            AccountId = input.Account?.Id ?? 0,
            AccountNumber = input.AccountNumber,
            AccountName = input.Account?.Citizen.FullName ?? "Unknown",
            TopUpAmount = amount,
            Reason = reason
        });
    }

    private static void ValidateRequest(ManualTopupRequestDTO request)
    {
        var errors = new Dictionary<string, string>();
        if (request.TopUpAmount <= 0) errors[nameof(request.TopUpAmount)] = "Top-up amount must be positive.";
        if (string.IsNullOrWhiteSpace(request.DisbursementReason))
            errors[nameof(request.DisbursementReason)] = "Disbursement reason is required.";
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
            errors[nameof(request.IdempotencyKey)] = "Idempotency key is required.";

        var hasIds = request.AccountIds is { Count: > 0 };
        var hasFile = request.File is { Length: > 0 };
        if (hasIds == hasFile)
            errors[nameof(request.AccountIds)] = "Provide either AccountIds or a CSV file, but not both.";
        if (hasIds && request.AccountIds!.Any(id => id <= 0))
            errors[nameof(request.AccountIds)] = "All account IDs must be positive.";
        if (errors.Count != 0) throw new ValidationFailureException(errors);
    }

    private static ExecuteTopupResultDTO BuildResult(TopupExecution execution)
    {
        var result = new ExecuteTopupResultDTO
        {
            BatchId = execution.Id,
            TotalProcessed = execution.TotalTargetCount,
            TotalSuccess = execution.SuccessCount,
            TotalFailed = execution.FailedCount,
            TotalAmountCredited = execution.TotalExecutedAmount
        };
        foreach (var target in execution.Targets)
        {
            if (target.Status == TopupTargetStatus.Success)
            {
                result.SuccessList.Add(new TopupSuccessItemDTO
                {
                    AccountId = target.EducationAccountId ?? 0,
                    AccountNumber = target.AccountNumber,
                    AccountName = target.EducationAccount?.Citizen.FullName ?? "Unknown",
                    TopUpAmount = target.Amount,
                    TopUpTransactionId = target.EducationCreditTransaction?.TransactionCode ?? Guid.Empty
                });
            }
            else
            {
                result.FailList.Add(new TopupFailItemDTO
                {
                    AccountId = target.EducationAccountId ?? 0,
                    AccountNumber = target.AccountNumber,
                    AccountName = target.EducationAccount?.Citizen.FullName ?? "Unknown",
                    TopUpAmount = target.Amount,
                    Reason = target.FailureReason ?? "Unknown error"
                });
            }
        }

        return result;
    }

    private async Task LogAuditAsync(
        string status,
        EducationAccount account,
        CancellationToken cancellationToken)
    {
        await _auditLogWriter.LogAsync(
            AuditLogCategory.Transaction,
            $"Manual Top-Up - {status}",
            account.Citizen.Nric,
            cancellationToken);
    }

    private sealed record ManualInput(
        EducationAccount? Account,
        string AccountNumber,
        string? Error,
        bool PersistFailure);
}
