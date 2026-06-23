using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using System.Text.Json;

namespace Services.TopUp;

public class TopupBackgroundService(
    IUnitOfWork unitOfWork,
    IAuditLogWriter auditLogWriter)
    : ITopupBackgroundService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
    private readonly IGenericRepository<SystemTopup> _systemTopupRepository = unitOfWork.Repository<SystemTopup>();
    private readonly IGenericRepository<ScheduleTopUp> _scheduleRepository = unitOfWork.Repository<ScheduleTopUp>();
    private readonly IGenericRepository<EducationAccount> _accountRepository = unitOfWork.Repository<EducationAccount>();
    private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
    private readonly IGenericRepository<TopupExecution> _executionRepository = unitOfWork.Repository<TopupExecution>();
    private readonly IGenericRepository<TopupExecutionTarget> _targetRepository = unitOfWork.Repository<TopupExecutionTarget>();
    private readonly IGenericRepository<TopupSystemApplication> _applicationRepository = unitOfWork.Repository<TopupSystemApplication>();

    public async Task<List<ExecuteTopupResultDTO>> ExecuteDueTopupsAsync(CancellationToken cancellationToken)
    {
        var nowSgt = DateTime.UtcNow.AddHours(8);
        var results = new List<ExecuteTopupResultDTO>();

        var systemTopups = await _systemTopupRepository.Query()
            .Include(topup => topup.ConditionGroups)
                .ThenInclude(group => group.Conditions)
            .Where(topup => topup.Status == SystemTopupStatus.Active && topup.TopupAmount > 0)
            .OrderBy(topup => topup.Id)
            .ToListAsync(cancellationToken);
        foreach (var systemTopup in systemTopups)
        {
            var tree = TopupConditionEvaluator.BuildSystemTree(systemTopup.ConditionGroups);
            var key = $"SYSTEM:SystemTopup-{systemTopup.Id}:{nowSgt:yyyy-MM-dd}";
            await ExecuteAsync(systemTopup.Id, null, systemTopup.Name, systemTopup.TopupAmount!.Value,
                tree, TopupExecutionSourceType.System, key, nowSgt, results, cancellationToken);
        }

        var schedules = await _scheduleRepository.Query(tracking: true)
            .Include(schedule => schedule.ConditionGroups)
                .ThenInclude(group => group.Conditions)
            .Where(schedule => schedule.Status == ScheduleTopUpStatus.Active &&
                               schedule.TopupAmount > 0 &&
                               schedule.NextExecutionAt != null &&
                               schedule.NextExecutionAt <= nowSgt)
            .OrderBy(schedule => schedule.NextExecutionAt)
            .ThenBy(schedule => schedule.Id)
            .ToListAsync(cancellationToken);

        foreach (var schedule in schedules)
        {
            var occurrence = schedule.NextExecutionAt!.Value;
            var tree = TopupConditionEvaluator.BuildScheduleTree(schedule.ConditionGroups);
            var key = $"SCHEDULE:ScheduleTopUp-{schedule.Id}:{occurrence:yyyy-MM-ddTHH:mm}";
            var completed = await ExecuteAsync(null, schedule.Id, schedule.Name, schedule.TopupAmount!.Value,
                tree, TopupExecutionSourceType.Schedule, key, nowSgt, results, cancellationToken);
            if (!completed) continue;

            if (schedule.Frequency == ScheduleTopUpFrequency.OneTime)
            {
                schedule.Status = ScheduleTopUpStatus.Completed;
                schedule.NextExecutionAt = null;
            }
            else
            {
                schedule.NextExecutionAt = ComputeNextExecutionDate(schedule, nowSgt);
            }
            _scheduleRepository.Update(schedule);
        }

        await _unitOfWork.SaveChangeAsync(cancellationToken);
        return results;
    }

    private async Task<bool> ExecuteAsync(
        int? systemTopupId,
        int? scheduleTopUpId,
        string name,
        decimal amount,
        TopupConditionEvaluator.GroupNode tree,
        TopupExecutionSourceType sourceType,
        string idempotencyKey,
        DateTime nowSgt,
        List<ExecuteTopupResultDTO> results,
        CancellationToken cancellationToken)
    {
        var existingStatus = await _executionRepository.Query()
            .Where(execution => execution.IdempotencyKey == idempotencyKey)
            .Select(execution => (TopupExecutionStatus?)execution.Status)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingStatus.HasValue)
            return existingStatus == TopupExecutionStatus.Completed;

        var execution = new TopupExecution
        {
            ExecutionCode = $"EXEC-{sourceType.ToString().ToUpperInvariant()}-{Guid.NewGuid():N}"[..22],
            SourceType = sourceType,
            SystemTopupId = systemTopupId,
            ScheduleTopUpId = scheduleTopUpId,
            IdempotencyKey = idempotencyKey,
            Status = TopupExecutionStatus.Pending,
            TopupNameSnapshot = name,
            TopupAmountSnapshot = amount,
            ConditionsSnapshot = JsonSerializer.Serialize(tree)
        };
        execution.TryValidate();
        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            await _executionRepository.AddAsync(execution, token);
            await _unitOfWork.SaveChangeAsync(token);
            execution.Status = TopupExecutionStatus.Executing;
            _executionRepository.Update(execution);
        }, cancellationToken);

        var result = new ExecuteTopupResultDTO { BatchId = execution.Id };
        var appliedAccountIds = systemTopupId.HasValue
            ? (await _applicationRepository.Query()
                .Where(application => application.SystemTopupId == systemTopupId.Value)
                .Select(application => application.EducationAccountId)
                .ToListAsync(cancellationToken)).ToHashSet()
            : [];

        const int pageSize = 2000;
        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            for (var page = 0; ; page++)
            {
                var accounts = await _accountRepository.Query(tracking: true)
                    .Include(account => account.Citizen)
                    .Where(account => account.Status == EducationAccountStatus.Active)
                    .OrderBy(account => account.Id)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync(token);
                if (accounts.Count == 0) break;

                foreach (var account in accounts)
                {
                    if (systemTopupId.HasValue && appliedAccountIds.Contains(account.Id)) continue;
                    var evaluation = TopupConditionEvaluator.Evaluate(account, tree, nowSgt);
                    if (!evaluation.IsSatisfied) continue;

                    execution.TotalTargetCount++;
                    try
                    {
                        var balanceBefore = account.EducationCreditBalance;
                        var balanceAfter = balanceBefore + amount;
                        account.EducationCreditBalance = balanceAfter;
                        _accountRepository.Update(account);

                        var transaction = new EducationCreditTransaction
                        {
                            Type = EducationCreditTransactionType.Topup,
                            Direction = EducationCreditTransactionDirection.Credit,
                            Amount = amount,
                            BalanceBefore = balanceBefore,
                            BalanceAfter = balanceAfter,
                            Description = $"{sourceType} top-up - {name}",
                            EducationAccountId = account.Id
                        };
                        transaction.TryValidate();
                        await _transactionRepository.AddAsync(transaction, token);

                        var target = new TopupExecutionTarget
                        {
                            TopupExecutionId = execution.Id,
                            EducationAccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            Amount = amount,
                            Status = TopupTargetStatus.Success,
                            MatchedConditionsSnapshot = JsonSerializer.Serialize(evaluation.MatchedConditions),
                            EducationCreditTransaction = transaction
                        };
                        target.TryValidate();
                        await _targetRepository.AddAsync(target, token);

                        if (systemTopupId.HasValue)
                        {
                            var application = new TopupSystemApplication
                            {
                                SystemTopupId = systemTopupId.Value,
                                EducationAccountId = account.Id,
                                TopupExecutionTarget = target
                            };
                            await _applicationRepository.AddAsync(application, token);
                        }

                        execution.SuccessCount++;
                        execution.TotalExecutedAmount += amount;
                        appliedAccountIds.Add(account.Id);
                        result.SuccessList.Add(new TopupSuccessItemDTO
                        {
                            AccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            AccountName = account.Citizen.FullName,
                            TopUpAmount = amount,
                            TopUpTransactionId = transaction.TransactionCode
                        });
                    }
                    catch (Exception exception) when (exception is not OperationCanceledException)
                    {
                        execution.FailedCount++;
                        var reason = exception.GetBaseException().Message;
                        result.FailList.Add(new TopupFailItemDTO
                        {
                            AccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            AccountName = account.Citizen.FullName,
                            TopUpAmount = amount,
                            Reason = $"Execution failed: {reason}"
                        });
                        var target = new TopupExecutionTarget
                        {
                            TopupExecutionId = execution.Id,
                            EducationAccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            Amount = amount,
                            Status = TopupTargetStatus.Failed,
                            FailureReason = reason[..Math.Min(500, reason.Length)],
                            MatchedConditionsSnapshot = JsonSerializer.Serialize(evaluation.MatchedConditions)
                        };
                        target.TryValidate();
                        await _targetRepository.AddAsync(target, token);
                    }
                }
            }

            execution.Status = TopupExecutionStatus.Completed;
            _executionRepository.Update(execution);
        }, cancellationToken);

        result.TotalProcessed = execution.TotalTargetCount;
        result.TotalSuccess = execution.SuccessCount;
        result.TotalFailed = execution.FailedCount;
        result.TotalAmountCredited = execution.TotalExecutedAmount;
        results.Add(result);

        await _auditLogWriter.LogAsync(
            AuditLogCategory.Transaction,
            $"{sourceType} Top-Up Completed",
            cancellationToken: cancellationToken);
        return true;
    }

    private static DateTime ComputeNextExecutionDate(ScheduleTopUp schedule, DateTime nowSgt)
    {
        if (schedule.Frequency == ScheduleTopUpFrequency.Monthly)
        {
            var occurrence = CreateOccurrence(nowSgt.Year, nowSgt.Month,
                schedule.ExecuteAtDay!.Value, schedule.ExecutionTime);
            if (occurrence <= nowSgt)
            {
                var nextMonth = new DateTime(nowSgt.Year, nowSgt.Month, 1).AddMonths(1);
                occurrence = CreateOccurrence(nextMonth.Year, nextMonth.Month,
                    schedule.ExecuteAtDay.Value, schedule.ExecutionTime);
            }
            return occurrence;
        }

        var yearly = CreateOccurrence(nowSgt.Year, schedule.ExecuteAtMonth!.Value,
            schedule.ExecuteAtDay!.Value, schedule.ExecutionTime);
        return yearly <= nowSgt
            ? CreateOccurrence(nowSgt.Year + 1, schedule.ExecuteAtMonth.Value,
                schedule.ExecuteAtDay.Value, schedule.ExecutionTime)
            : yearly;
    }

    private static DateTime CreateOccurrence(int year, int month, int requestedDay, TimeOnly time)
    {
        var day = Math.Min(requestedDay, DateTime.DaysInMonth(year, month));
        return new DateTime(year, month, day, time.Hour, time.Minute, time.Second, DateTimeKind.Unspecified);
    }
}