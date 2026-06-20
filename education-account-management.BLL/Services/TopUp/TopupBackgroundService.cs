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
    private readonly IGenericRepository<TopupSchedule> _scheduleRepository = unitOfWork.Repository<TopupSchedule>();
    private readonly IGenericRepository<EducationAccount> _accountRepository = unitOfWork.Repository<EducationAccount>();
    private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
    private readonly IGenericRepository<TopupExecution> _executionRepository = unitOfWork.Repository<TopupExecution>();
    private readonly IGenericRepository<TopupExecutionTarget> _targetRepository = unitOfWork.Repository<TopupExecutionTarget>();
    private readonly IGenericRepository<TopupSystemApplication> _applicationRepository = unitOfWork.Repository<TopupSystemApplication>();
    private readonly IGenericRepository<TopupRule> _ruleRepository = unitOfWork.Repository<TopupRule>();

    public async Task<List<ExecuteTopupResultDTO>> ExecuteDueTopupsAsync(CancellationToken cancellationToken)
    {
        var nowSgt = DateTime.UtcNow.AddHours(8);
        var results = new List<ExecuteTopupResultDTO>();

        var systemRules = await _ruleRepository.Query(tracking: true)
            .Include(rule => rule.Conditions)
            .Where(rule => rule.Type == TopupRuleType.System && rule.Status == TopupRuleStatus.Active)
            .OrderBy(rule => rule.Id)
            .ToListAsync(cancellationToken);
        foreach (var rule in systemRules)
        {
            var key = $"SYSTEM:Rule-{rule.Id}:{nowSgt:yyyy-MM-dd}";
            await ExecuteRuleAsync(rule, null, TopupExecutionSourceType.System,
                key, nowSgt, results, cancellationToken);
        }

        var schedules = await _scheduleRepository.Query(tracking: true)
            .Include(schedule => schedule.TopupRule)
                .ThenInclude(rule => rule.Conditions)
            .Where(schedule => schedule.Status == TopupScheduleStatus.Active
                && schedule.NextExecutionAt != null
                && schedule.NextExecutionAt <= nowSgt)
            .OrderBy(schedule => schedule.NextExecutionAt)
            .ThenBy(schedule => schedule.TopupRuleId)
            .ToListAsync(cancellationToken);

        foreach (var schedule in schedules)
        {
            if (schedule.TopupRule.Status != TopupRuleStatus.Active) continue;
            var occurrence = schedule.NextExecutionAt!.Value;
            var key = $"SCHEDULE:Schedule-{schedule.Id}:{occurrence:yyyy-MM-ddTHH:mm}";
            var completed = await ExecuteRuleAsync(
                schedule.TopupRule,
                schedule,
                TopupExecutionSourceType.Schedule,
                key,
                nowSgt,
                results,
                cancellationToken);
            if (!completed) continue;

            if (schedule.Frequency == TopupScheduleType.OneTime)
            {
                schedule.Status = TopupScheduleStatus.Completed;
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

    private async Task<bool> ExecuteRuleAsync(
        TopupRule rule,
        TopupSchedule? schedule,
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
            TopupRuleId = rule.Id,
            TopupScheduleId = schedule?.Id,
            IdempotencyKey = idempotencyKey,
            Status = TopupExecutionStatus.Pending,
            RuleNameSnapshot = rule.RuleName,
            RuleTypeSnapshot = rule.Type,
            MatchModeSnapshot = rule.MatchMode,
            TopupAmountSnapshot = rule.TopupAmount,
            RuleConditionsSnapshot = BuildConditionsSnapshot(rule.Conditions)
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
        var appliedAccountIds = sourceType == TopupExecutionSourceType.System
            ? (await _applicationRepository.Query()
                .Where(application => application.TopupRuleId == rule.Id)
                .Select(application => application.EducationAccountId)
                .ToListAsync(cancellationToken)).ToHashSet()
            : [];

        const int pageSize = 2000;
        for (var page = 0; ; page++)
        {
            var accounts = await _accountRepository.Query(tracking: true)
                .Include(account => account.Citizen)
                .Where(account => account.Status == EducationAccountStatus.Active)
                .OrderBy(account => account.Id)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            if (accounts.Count == 0) break;

            foreach (var account in accounts)
            {
                if (sourceType == TopupExecutionSourceType.System && appliedAccountIds.Contains(account.Id))
                    continue;

                var matchedConditions = rule.Conditions
                    .OrderBy(condition => condition.DisplayOrder)
                    .Where(condition => IsConditionSatisfied(account, condition, nowSgt))
                    .ToList();
                var isSatisfied = rule.MatchMode == TopupMatchMode.And
                    ? matchedConditions.Count == rule.Conditions.Count
                    : matchedConditions.Count > 0;
                if (!isSatisfied) continue;

                var amount = rule.MatchMode == TopupMatchMode.And
                    ? rule.TopupAmount ?? 0
                    : matchedConditions.Sum(condition => condition.ConditionAmount ?? 0);
                if (amount <= 0) continue;

                execution.TotalTargetCount++;
                try
                {
                    var success = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
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
                            Description = $"{sourceType} top-up - {rule.RuleName}",
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
                            MatchedConditionsSnapshot = BuildConditionsSnapshot(matchedConditions),
                            EducationCreditTransaction = transaction
                        };
                        target.TryValidate();
                        await _targetRepository.AddAsync(target, token);

                        if (sourceType == TopupExecutionSourceType.System)
                        {
                            var application = new TopupSystemApplication
                            {
                                TopupRuleId = rule.Id,
                                EducationAccountId = account.Id,
                                TopupExecutionTarget = target
                            };
                            application.TryValidate();
                            await _applicationRepository.AddAsync(application, token);
                        }

                        return new TopupSuccessItemDTO
                        {
                            AccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            AccountName = account.Citizen.FullName,
                            TopUpAmount = amount,
                            TopUpTransactionId = transaction.TransactionCode
                        };
                    }, cancellationToken);

                    execution.SuccessCount++;
                    execution.TotalExecutedAmount += amount;
                    appliedAccountIds.Add(account.Id);
                    result.SuccessList.Add(success);
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
                        MatchedConditionsSnapshot = BuildConditionsSnapshot(matchedConditions)
                    };
                    target.TryValidate();
                    await _targetRepository.AddAsync(target, cancellationToken);
                    await _unitOfWork.SaveChangeAsync(cancellationToken);
                }
            }
        }

        execution.Status = TopupExecutionStatus.Completed;
        _executionRepository.Update(execution);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

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

    private static string BuildConditionsSnapshot(IEnumerable<TopupRuleCondition> conditions)
    {
        return JsonSerializer.Serialize(conditions
            .OrderBy(condition => condition.DisplayOrder)
            .Select(condition => new
            {
                Field = condition.Field,
                Operator = condition.Operator,
                ValueText = condition.ValueText,
                ValueNumber = condition.ValueNumber,
                ConditionAmount = condition.ConditionAmount,
                DisplayOrder = condition.DisplayOrder
            })
            .ToList());
    }

    private static bool IsConditionSatisfied(
        EducationAccount account,
        TopupRuleCondition condition,
        DateTime nowSgt)
    {
        if (condition.Field == TopupRuleConditionField.Age)
        {
            if (!condition.ValueNumber.HasValue) return false;
            var today = DateOnly.FromDateTime(nowSgt);
            var age = today.Year - account.Citizen.DateOfBirth.Year;
            if (today < account.Citizen.DateOfBirth.AddYears(age)) age--;
            return Compare(age, condition.ValueNumber.Value, condition.Operator);
        }

        if (condition.Field == TopupRuleConditionField.Balance)
            return condition.ValueNumber.HasValue
                && Compare(account.EducationCreditBalance, condition.ValueNumber.Value, condition.Operator);

        if (condition.Field == TopupRuleConditionField.SchoolingStatus)
        {
            var current = account.Citizen.SchoolingStatus?.Trim();
            var expected = condition.ValueText?.Trim();
            return condition.Operator switch
            {
                TopupRuleConditionOperator.Equals => string.Equals(current, expected, StringComparison.OrdinalIgnoreCase),
                TopupRuleConditionOperator.NotEquals => !string.Equals(current, expected, StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }

        return false;
    }

    private static bool Compare(decimal current, decimal expected, TopupRuleConditionOperator operation)
    {
        return operation switch
        {
            TopupRuleConditionOperator.GreaterThan => current > expected,
            TopupRuleConditionOperator.GreaterThanOrEqual => current >= expected,
            TopupRuleConditionOperator.LessThan => current < expected,
            TopupRuleConditionOperator.LessThanOrEqual => current <= expected,
            TopupRuleConditionOperator.Equals => current == expected,
            TopupRuleConditionOperator.NotEquals => current != expected,
            _ => false
        };
    }

    private static DateTime ComputeNextExecutionDate(TopupSchedule schedule, DateTime nowSgt)
    {
        if (schedule.Frequency == TopupScheduleType.Monthly)
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
