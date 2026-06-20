using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using System.Text.Json;

namespace Services.TopUp
{
    public class TopupService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuditLogWriter auditLogWriter)
        : ITopupService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

        private readonly IGenericRepository<EducationAccount> _educationAccountRepository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<EducationCreditTransaction> _creditTransactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
        private readonly IGenericRepository<TopupExecution> _executionRepository = unitOfWork.Repository<TopupExecution>();
        private readonly IGenericRepository<TopupExecutionTarget> _executionTargetRepository = unitOfWork.Repository<TopupExecutionTarget>();

        #region Execute Manual Top-Up

        public async Task<ExecuteTopupResultDTO> ExecuteManualTopupAsync(
            ManualTopupRequestDTO request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            // Idempotency check
            var existingExecution = await _executionRepository.Query()
                .Include(e => e.Targets).ThenInclude(t => t.EducationAccount).ThenInclude(a => a!.Citizen)
                .FirstOrDefaultAsync(e => e.IdempotencyKey == request.IdempotencyKey, cancellationToken);

            if (existingExecution != null)
                return BuildResultFromExisting(existingExecution);

            // Resolve target accounts
            List<EducationAccount> accounts;
            if (request.AccountIds is { Count: > 0 })
                accounts = await ResolveAccountsByIds(request.AccountIds, cancellationToken);
            else
                accounts = await ResolveAccountsFromCsv(request.File!, cancellationToken);

            return await ExecuteTopupForAccountsAsync(accounts, request, cancellationToken);
        }

        private async Task<List<EducationAccount>> ResolveAccountsByIds(
            List<int> accountIds,
            CancellationToken cancellationToken)
        {
            return await _educationAccountRepository.Query()
                .Include(a => a.Citizen)
                .Where(a => accountIds.Contains(a.Id))
                .ToListAsync(cancellationToken);
        }

        private async Task<List<EducationAccount>> ResolveAccountsFromCsv(
            IFormFile file,
            CancellationToken cancellationToken)
        {
            var fileErrors = CsvImportHelper.ValidateFile(file);
            if (fileErrors.Any())
                throw new UserFacingException(string.Join(" ", fileErrors.Select(e => e.Message)), 400);

            var readResult = CsvImportHelper.ReadRows<ManualTopupImportRowDTO>(file);

            var accountNumbers = readResult.Items
                .Select(i => i.Row.AccountNumber?.Trim())
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return await _educationAccountRepository.Query()
                .Include(a => a.Citizen)
                .Where(a => accountNumbers.Contains(a.AccountNumber))
                .ToListAsync(cancellationToken);
        }

        private async Task<ExecuteTopupResultDTO> ExecuteTopupForAccountsAsync(
            List<EducationAccount> accounts,
            ManualTopupRequestDTO request,
            CancellationToken cancellationToken)
        {
            var execution = new TopupExecution
            {
                ExecutionCode = $"EXEC-MANUAL-{Guid.NewGuid().ToString("N")[..10].ToUpper()}",
                SourceType = TopupExecutionSourceType.Manual,
                IdempotencyKey = request.IdempotencyKey,
                ManualAmount = request.TopUpAmount,
                ManualReason = request.DisbursementReason,
                Status = TopupExecutionStatus.Executing,
                TotalTargetCount = accounts.Count
            };
            execution.TryValidate();
            await _executionRepository.AddAsync(execution, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var result = new ExecuteTopupResultDTO
            {
                BatchId = execution.Id,
                TotalProcessed = accounts.Count
            };

            var processedIds = new HashSet<int>();

            foreach (var account in accounts)
            {
                if (!processedIds.Add(account.Id))
                {
                    await RecordFailedTarget(execution, account, request.TopUpAmount, "Duplicate account.", cancellationToken);
                    result.FailList.Add(BuildFailItem(account, request.TopUpAmount, "Duplicate account."));
                    continue;
                }

                if (account.Status != EducationAccountStatus.Active)
                {
                    var reason = $"Account is not Active (current status: {account.Status}).";
                    await RecordFailedTarget(execution, account, request.TopUpAmount, reason, cancellationToken);
                    result.FailList.Add(BuildFailItem(account, request.TopUpAmount, reason));
                    await LogAuditAsync(execution.Id, "Manual Top-Up", $"Failed (Status: {account.Status})",
                        request.DisbursementReason, account.Id, request.TopUpAmount, account.Citizen.Nric, cancellationToken);
                    continue;
                }

                try
                {
                    var successItem = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
                    {
                        var balanceBefore = account.EducationCreditBalance;
                        var balanceAfter = balanceBefore + request.TopUpAmount;

                        account.EducationCreditBalance = balanceAfter;
                        _educationAccountRepository.Update(account);

                        var txCode = Guid.NewGuid();
                        var creditTx = new EducationCreditTransaction
                        {
                            TransactionCode = txCode,
                            Type = EducationCreditTransactionType.Topup,
                            Direction = EducationCreditTransactionDirection.Credit,
                            Amount = request.TopUpAmount,
                            BalanceBefore = balanceBefore,
                            BalanceAfter = balanceAfter,
                            Description = request.DisbursementReason,
                            EducationAccountId = account.Id
                        };
                        creditTx.TryValidate();
                        await _creditTransactionRepository.AddAsync(creditTx, token);
                        await _unitOfWork.SaveChangeAsync(token);

                        var target = new TopupExecutionTarget
                        {
                            TopupExecutionId = execution.Id,
                            EducationAccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            Amount = request.TopUpAmount,
                            Status = TopupTargetStatus.Success,
                            EducationCreditTransactionId = creditTx.Id
                        };
                        target.TryValidate();
                        await _executionTargetRepository.AddAsync(target, token);
                        await _unitOfWork.SaveChangeAsync(token);

                        return new TopupSuccessItemDTO
                        {
                            AccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            AccountName = account.Citizen.FullName,
                            TopUpTransactionId = txCode,
                            TopUpAmount = request.TopUpAmount
                        };
                    }, cancellationToken);

                    if (successItem != null)
                    {
                        execution.SuccessCount++;
                        execution.TotalExecutedAmount += request.TopUpAmount;
                        result.SuccessList.Add(successItem);
                        result.TotalSuccess++;

                        await LogAuditAsync(execution.Id, "Manual Top-Up", "Success",
                            request.DisbursementReason, account.Id, request.TopUpAmount, account.Citizen.Nric, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    execution.FailedCount++;
                    result.TotalFailed++;
                    var reason = ex.GetBaseException().Message;
                    result.FailList.Add(BuildFailItem(account, request.TopUpAmount, $"Execution failed: {reason}"));

                    await RecordFailedTarget(execution, account, request.TopUpAmount, reason[..Math.Min(500, reason.Length)], cancellationToken);
                    await LogAuditAsync(execution.Id, "Manual Top-Up", $"Failed ({reason})",
                        request.DisbursementReason, account.Id, request.TopUpAmount, account.Citizen.Nric, cancellationToken);
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

        #endregion

        #region Helpers

        private static ExecuteTopupResultDTO BuildResultFromExisting(TopupExecution existing)
        {
            var result = new ExecuteTopupResultDTO
            {
                BatchId = existing.Id,
                TotalProcessed = existing.TotalTargetCount,
                TotalSuccess = existing.SuccessCount,
                TotalFailed = existing.FailedCount,
                TotalAmountCredited = existing.TotalExecutedAmount
            };

            foreach (var t in existing.Targets)
            {
                if (t.Status == TopupTargetStatus.Success)
                {
                    result.SuccessList.Add(new TopupSuccessItemDTO
                    {
                        AccountId = t.EducationAccountId ?? 0,
                        AccountNumber = t.AccountNumber,
                        AccountName = t.EducationAccount?.Citizen?.FullName ?? "Unknown",
                        TopUpAmount = t.Amount
                    });
                }
                else
                {
                    result.FailList.Add(new TopupFailItemDTO
                    {
                        AccountId = t.EducationAccountId ?? 0,
                        AccountNumber = t.AccountNumber,
                        AccountName = t.EducationAccount?.Citizen?.FullName ?? "Unknown",
                        TopUpAmount = t.Amount,
                        Reason = t.FailureReason ?? "Unknown error"
                    });
                }
            }

            return result;
        }

        private async Task RecordFailedTarget(
            TopupExecution execution,
            EducationAccount account,
            decimal amount,
            string reason,
            CancellationToken cancellationToken)
        {
            execution.FailedCount++;
            var target = new TopupExecutionTarget
            {
                TopupExecutionId = execution.Id,
                EducationAccountId = account.Id,
                AccountNumber = account.AccountNumber,
                Amount = amount,
                Status = TopupTargetStatus.Failed,
                FailureReason = reason[..Math.Min(500, reason.Length)]
            };
            target.TryValidate();
            await _executionTargetRepository.AddAsync(target, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private static TopupFailItemDTO BuildFailItem(EducationAccount account, decimal amount, string reason)
        {
            return new TopupFailItemDTO
            {
                AccountId = account.Id,
                AccountNumber = account.AccountNumber,
                AccountName = account.Citizen.FullName,
                TopUpAmount = amount,
                Reason = reason
            };
        }

        private async Task LogAuditAsync(
            int executionId, string logAction, string logStatus,
            string? reason, int accountId, decimal amount,
            string? nric, CancellationToken cancellationToken)
        {
            var payload = new
            {
                TopupExecutionId = executionId,
                TopupAction = logAction,
                TopupStatus = logStatus,
                Name = reason ?? "N/A",
                AccountID = accountId,
                TopUpAmount = amount,
                AdminUser = _currentUserService.UserName ?? "System",
                DateTime = DateTime.UtcNow
            };

            await _auditLogWriter.LogAsync(
                AuditLogCategory.Transaction,
                action: $"{logAction} – {logStatus}",
                payloadJson: JsonSerializer.Serialize(payload),
                targetNric: nric,
                cancellationToken: cancellationToken);
        }
        #endregion
    }
}
