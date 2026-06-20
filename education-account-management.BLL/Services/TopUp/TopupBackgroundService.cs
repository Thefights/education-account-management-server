using DTOs.TopUp;
using Interfaces.Audit;
using Interfaces.TopUp;
using System.Text.Json;

namespace Services.TopUp
{
    public class TopupBackgroundService(
        IUnitOfWork unitOfWork,
        IAuditLogWriter auditLogWriter)
        : ITopupBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

        private readonly IGenericRepository<TopupSchedule> _topupScheduleRepository = unitOfWork.Repository<TopupSchedule>();
        private readonly IGenericRepository<EducationAccount> _educationAccountRepository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<Enrollment> _enrollmentRepository = unitOfWork.Repository<Enrollment>();
        private readonly IGenericRepository<EducationCreditTransaction> _creditTransactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
        private readonly IGenericRepository<TopupExecution> _executionRepository = unitOfWork.Repository<TopupExecution>();
        private readonly IGenericRepository<TopupExecutionTarget> _executionTargetRepository = unitOfWork.Repository<TopupExecutionTarget>();
        private readonly IGenericRepository<TopupSystemApplication> _systemApplicationRepository = unitOfWork.Repository<TopupSystemApplication>();
        private readonly IGenericRepository<TopupRule> _topupRuleRepository = unitOfWork.Repository<TopupRule>();

        public async Task<List<ExecuteTopupResultDTO>> ExecuteDueTopupsAsync(CancellationToken cancellationToken)
        {
            var nowSgt = DateTime.UtcNow.AddHours(8);
            var results = new List<ExecuteTopupResultDTO>();

            // 1. Process System Rules
            var systemRules = await _topupRuleRepository.Query(tracking: true)
                .Include(r => r.Conditions)
                .Where(r => r.Type == TopupRuleType.System && r.Status == TopupRuleStatus.Active)
                .ToListAsync(cancellationToken);

            foreach (var rule in systemRules)
            {
                var idempotencyKey = $"SYSTEM:Rule-{rule.Id}:{nowSgt:yyyy-MM-dd}";
                await ExecuteRuleAsync(rule, null, TopupExecutionSourceType.System, idempotencyKey, nowSgt, results, cancellationToken);
            }

            // 2. Process due schedules
            var dueSchedules = await _topupScheduleRepository.Query(tracking: true)
                .Include(s => s.TopupRule)
                .ThenInclude(r => r.Conditions)
                .Where(s => s.Status == TopupScheduleStatus.Active && s.NextExecutionAt != null && s.NextExecutionAt <= nowSgt)
                .ToListAsync(cancellationToken);

            foreach (var schedule in dueSchedules)
            {
                if (schedule.TopupRule == null || schedule.TopupRule.Status != TopupRuleStatus.Active)
                    continue;

                var idempotencyKey = $"SCHEDULE:Schedule-{schedule.Id}:{schedule.NextExecutionAt!.Value:yyyy-MM-ddTHH:mm}";
                await ExecuteRuleAsync(schedule.TopupRule, schedule, TopupExecutionSourceType.Schedule, idempotencyKey, nowSgt, results, cancellationToken);

                if (schedule.Frequency == TopupScheduleType.OneTime)
                {
                    schedule.Status = TopupScheduleStatus.Completed;
                    schedule.NextExecutionAt = null;
                }
                else
                {
                    schedule.NextExecutionAt = ComputeNextExecutionDate(schedule, schedule.NextExecutionAt.Value);
                }

                _topupScheduleRepository.Update(schedule);
            }

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return results;
        }

        private async Task ExecuteRuleAsync(
            TopupRule rule,
            TopupSchedule? schedule,
            TopupExecutionSourceType sourceType,
            string idempotencyKey,
            DateTime nowSgt,
            List<ExecuteTopupResultDTO> results,
            CancellationToken cancellationToken)
        {
            // Idempotency check
            var exists = await _executionRepository.Query().AnyAsync(e => e.IdempotencyKey == idempotencyKey, cancellationToken);
            if (exists) return;

            var execution = new TopupExecution
            {
                ExecutionCode = $"EXEC-{sourceType.ToString().ToUpper()}-{Guid.NewGuid().ToString("N")[..10].ToUpper()}",
                SourceType = sourceType,
                TopupRuleId = rule.Id,
                TopupScheduleId = schedule?.Id,
                IdempotencyKey = idempotencyKey,
                Status = TopupExecutionStatus.Executing
            };
            execution.TryValidate();
            await _executionRepository.AddAsync(execution, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var result = new ExecuteTopupResultDTO { BatchId = execution.Id };

            const int pageSize = 2000;
            var page = 0;

            while (true)
            {
                var accountPage = await _educationAccountRepository
                    .Query(tracking: true)
                    .Include(a => a.Citizen)
                    .Where(a => a.Status == EducationAccountStatus.Active)
                    .OrderBy(a => a.Id)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                if (accountPage.Count == 0) break;
                page++;

                var accountIds = accountPage.Select(a => a.Id).ToList();
                var enrolledAccountIds = (await _enrollmentRepository.GetProjectedAsync(
                    query => query.Select(e => e.EducationAccountId),
                    filter: e => accountIds.Contains(e.EducationAccountId)
                        && e.CompletedAt == null
                        && e.WithdrawnAt == null,
                    cancellationToken: cancellationToken))
                    .ToHashSet();

                foreach (var account in accountPage)
                {
                    if (sourceType == TopupExecutionSourceType.System)
                    {
                        var hasApplied = await _systemApplicationRepository.Query()
                            .AnyAsync(sa => sa.TopupRuleId == rule.Id && sa.EducationAccountId == account.Id, cancellationToken);
                        if (hasApplied) continue;
                    }

                    var matchedConditions = GetMatchedConditions(account, rule.Conditions.ToList(), enrolledAccountIds.Contains(account.Id));

                    bool isSatisfied;
                    decimal topupAmount;

                    if (rule.MatchMode == TopupMatchMode.And)
                    {
                        isSatisfied = matchedConditions.Count == rule.Conditions.Count;
                        topupAmount = rule.TopupAmount ?? 0m;
                    }
                    else
                    {
                        isSatisfied = matchedConditions.Count > 0;
                        topupAmount = matchedConditions.Sum(c => c.ConditionAmount ?? 0m);
                    }

                    if (!isSatisfied) continue;

                    execution.TotalTargetCount++;

                    try
                    {
                        var matchedIdsJson = JsonSerializer.Serialize(matchedConditions.Select(c => c.Id).ToList());

                        var successItem = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
                        {
                            var balanceBefore = account.EducationCreditBalance;
                            var balanceAfter = balanceBefore + topupAmount;

                            account.EducationCreditBalance = balanceAfter;
                            _educationAccountRepository.Update(account);

                            var txCode = Guid.NewGuid();
                            var creditTx = new EducationCreditTransaction
                            {
                                TransactionCode = txCode,
                                Type = EducationCreditTransactionType.Topup,
                                Direction = EducationCreditTransactionDirection.Credit,
                                Amount = topupAmount,
                                BalanceBefore = balanceBefore,
                                BalanceAfter = balanceAfter,
                                Description = sourceType == TopupExecutionSourceType.System
                                    ? $"System top-up - {rule.RuleName}"
                                    : "Scheduled top-up",
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
                                Amount = topupAmount,
                                Status = TopupTargetStatus.Success,
                                MatchedConditionIds = matchedIdsJson,
                                EducationCreditTransactionId = creditTx.Id
                            };
                            target.TryValidate();
                            await _executionTargetRepository.AddAsync(target, token);
                            await _unitOfWork.SaveChangeAsync(token);

                            if (sourceType == TopupExecutionSourceType.System)
                            {
                                var appLog = new TopupSystemApplication
                                {
                                    TopupRuleId = rule.Id,
                                    EducationAccountId = account.Id,
                                    TopupExecutionTargetId = target.Id
                                };
                                appLog.TryValidate();
                                await _systemApplicationRepository.AddAsync(appLog, token);
                                await _unitOfWork.SaveChangeAsync(token);
                            }

                            return new TopupSuccessItemDTO
                            {
                                AccountId = account.Id,
                                AccountNumber = account.AccountNumber,
                                AccountName = account.Citizen.FullName,
                                TopUpTransactionId = txCode,
                                TopUpAmount = topupAmount
                            };
                        }, cancellationToken);

                        if (successItem != null)
                        {
                            execution.SuccessCount++;
                            execution.TotalExecutedAmount += topupAmount;
                            result.SuccessList.Add(successItem);
                            result.TotalSuccess++;

                            await LogAuditAsync(execution.Id, $"{sourceType} Top-Up", "Success",
                                rule.RuleName, account.Id, topupAmount, account.Citizen.Nric, cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        execution.FailedCount++;
                        result.TotalFailed++;
                        result.FailList.Add(new TopupFailItemDTO
                        {
                            AccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            AccountName = account.Citizen.FullName,
                            TopUpAmount = topupAmount,
                            Reason = $"Execution failed: {ex.GetBaseException().Message}"
                        });

                        var failedTarget = new TopupExecutionTarget
                        {
                            TopupExecutionId = execution.Id,
                            EducationAccountId = account.Id,
                            AccountNumber = account.AccountNumber,
                            Amount = topupAmount,
                            Status = TopupTargetStatus.Failed,
                            FailureReason = ex.GetBaseException().Message[..Math.Min(500, ex.GetBaseException().Message.Length)],
                            MatchedConditionIds = JsonSerializer.Serialize(matchedConditions.Select(c => c.Id).ToList())
                        };
                        failedTarget.TryValidate();
                        await _executionTargetRepository.AddAsync(failedTarget, cancellationToken);
                        await _unitOfWork.SaveChangeAsync(cancellationToken);

                        await LogAuditAsync(execution.Id, $"{sourceType} Top-Up",
                            $"Failed ({ex.GetBaseException().Message})",
                            rule.RuleName, account.Id, topupAmount, account.Citizen.Nric, cancellationToken);
                    }
                }
            }

            execution.Status = TopupExecutionStatus.Completed;
            _executionRepository.Update(execution);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            result.TotalProcessed = execution.TotalTargetCount;
            result.TotalAmountCredited = execution.TotalExecutedAmount;
            results.Add(result);
        }

        private static DateTime ComputeNextExecutionDate(TopupSchedule schedule, DateTime lastRun)
        {
            return schedule.Frequency switch
            {
                TopupScheduleType.Monthly => lastRun.AddMonths(1),
                TopupScheduleType.Yearly => lastRun.AddYears(1),
                _ => lastRun
            };
        }

        private List<TopupRuleCondition> GetMatchedConditions(
            EducationAccount account,
            List<TopupRuleCondition> conditions,
            bool isEnrolled)
        {
            return conditions.Where(c => IsConditionSatisfied(account, c, isEnrolled)).ToList();
        }

        private static bool IsConditionSatisfied(EducationAccount account, TopupRuleCondition cond, bool isEnrolled)
        {
            if (cond.Field == TopupRuleConditionField.Age)
            {
                if (cond.ValueNumber == null) return false;
                var age = TimeUtil.TryCalculateAge(account.Citizen.DateOfBirth);
                var val = (int)cond.ValueNumber.Value;
                return cond.Operator switch
                {
                    TopupRuleConditionOperator.GreaterThan => age > val,
                    TopupRuleConditionOperator.GreaterThanOrEqual => age >= val,
                    TopupRuleConditionOperator.LessThan => age < val,
                    TopupRuleConditionOperator.LessThanOrEqual => age <= val,
                    TopupRuleConditionOperator.Equals => age == val,
                    TopupRuleConditionOperator.NotEquals => age != val,
                    _ => false
                };
            }

            if (cond.Field == TopupRuleConditionField.Balance)
            {
                if (cond.ValueNumber == null) return false;
                var balance = account.EducationCreditBalance;
                var val = cond.ValueNumber.Value;
                return cond.Operator switch
                {
                    TopupRuleConditionOperator.GreaterThan => balance > val,
                    TopupRuleConditionOperator.GreaterThanOrEqual => balance >= val,
                    TopupRuleConditionOperator.LessThan => balance < val,
                    TopupRuleConditionOperator.LessThanOrEqual => balance <= val,
                    TopupRuleConditionOperator.Equals => balance == val,
                    TopupRuleConditionOperator.NotEquals => balance != val,
                    _ => false
                };
            }

            if (cond.Field == TopupRuleConditionField.SchoolingStatus)
            {
                if (string.IsNullOrEmpty(cond.ValueText)) return false;
                var expectedEnrolled = string.Equals(cond.ValueText.Trim(), "Enrolled", StringComparison.OrdinalIgnoreCase);
                return cond.Operator switch
                {
                    TopupRuleConditionOperator.Equals => isEnrolled == expectedEnrolled,
                    TopupRuleConditionOperator.NotEquals => isEnrolled != expectedEnrolled,
                    _ => false
                };
            }

            return false;
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
                DateTime = DateTime.UtcNow
            };

            await _auditLogWriter.LogAsync(
                AuditLogCategory.Transaction,
                action: $"{logAction} – {logStatus}",
                payloadJson: JsonSerializer.Serialize(payload),
                targetNric: nric,
                cancellationToken: cancellationToken);
        }
    }
}
