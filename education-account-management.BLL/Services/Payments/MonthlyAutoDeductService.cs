using Interfaces.Email;
using Interfaces.Payments;
using Microsoft.Data.SqlClient;

namespace Services.Payments
{
    public class MonthlyAutoDeductService(
        IUnitOfWork unitOfWork,
        IOutboxWriter outboxWriter,
        AppConfiguration configuration,
        ILogger<MonthlyAutoDeductService> logger) : IMonthlyAutoDeductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutboxWriter _outboxWriter = outboxWriter;
        private readonly AppConfiguration _configuration = configuration;
        private readonly ILogger<MonthlyAutoDeductService> _logger = logger;

        private readonly IGenericRepository<OutstandingDeductionRun> _runRepository = unitOfWork.Repository<OutstandingDeductionRun>();
        private readonly IGenericRepository<OutstandingDeductionTarget> _targetRepository = unitOfWork.Repository<OutstandingDeductionTarget>();
        private readonly IGenericRepository<EducationAccount> _accountRepository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<Charge> _chargeRepository = unitOfWork.Repository<Charge>();
        private readonly IGenericRepository<Payment> _paymentRepository = unitOfWork.Repository<Payment>();
        private readonly IGenericRepository<PaymentAllocation> _paymentAllocationRepository = unitOfWork.Repository<PaymentAllocation>();
        private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();

        public async Task AutoDeductOutstandingFeesAsync(CancellationToken cancellationToken = default)
        {
            TimeZoneInfo sgtZone;
            try
            {
                sgtZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Singapore");
            }
            catch (TimeZoneNotFoundException)
            {
                sgtZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            }
            var nowSgt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sgtZone);
            var runMonth = $"{nowSgt.Year}-{nowSgt.Month:D2}";
            var runDate = DateOnly.FromDateTime(nowSgt);

            var existingRun = await _runRepository.Query()
                .FirstOrDefaultAsync(r => r.RunMonth == runMonth, cancellationToken);

            if (existingRun != null)
            {
                return;
            }

            var run = new OutstandingDeductionRun
            {
                RunMonth = runMonth,
                RunDate = runDate,
                Status = OutstandingDeductionRunStatus.Running,
                StartedAt = nowSgt
            };

            await _runRepository.AddAsync(run, cancellationToken);
            try
            {
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                var baseException = ex.GetBaseException();
                if (baseException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    _logger.LogWarning("Outstanding deduction run for month {RunMonth} already exists or is being processed concurrently.", runMonth);
                    return;
                }
                throw;
            }

            var accountIdsWithOutstandingCharges = await _chargeRepository.Query()
                .Where(c => c.RemainingAmount > 0
                         && c.Status != ChargeStatus.Paid
                         && c.Enrollment.SchoolStudent.EducationAccount.Status != EducationAccountStatus.Closed)
                .Select(c => c.Enrollment.SchoolStudent.EducationAccountId)
                .Distinct()
                .OrderBy(id => id)
                .ToListAsync(cancellationToken);

            var totalScannedCharges = 0;
            var totalDeducted = 0m;
            var successChargesCount = 0;
            var failedChargesCount = 0;

            const int batchSize = 100;
            for (int i = 0; i < accountIdsWithOutstandingCharges.Count; i += batchSize)
            {
                var batchAccountIds = accountIdsWithOutstandingCharges.Skip(i).Take(batchSize).ToList();

                var accountsMap = await _accountRepository.Query(tracking: true)
                    .Include(a => a.Citizen)
                    .Where(a => batchAccountIds.Contains(a.Id))
                    .ToDictionaryAsync(a => a.Id, cancellationToken);

                var charges = await _chargeRepository.Query(tracking: true)
                    .Include(c => c.Enrollment)
                        .ThenInclude(e => e.SchoolStudent)
                    .Include(c => c.Enrollment)
                        .ThenInclude(e => e.Course)
                    .Where(c => batchAccountIds.Contains(c.Enrollment.SchoolStudent.EducationAccountId)
                             && c.RemainingAmount > 0
                             && c.Status != ChargeStatus.Paid)
                    .OrderBy(c => c.Enrollment.SchoolStudent.EducationAccountId)
                    .ThenBy(c => c.Enrollment.Course.FasApplicationDueDate)
                    .ToListAsync(cancellationToken);

                var chargesGroupedByAccount = charges.GroupBy(c => c.Enrollment.SchoolStudent.EducationAccountId);

                foreach (var accountGroup in chargesGroupedByAccount)
                {
                    var accountId = accountGroup.Key;
                    var accountCharges = accountGroup.OrderBy(c => c.Enrollment.Course.FasApplicationDueDate).ToList();

                    if (!accountsMap.TryGetValue(accountId, out var account))
                    {
                        continue;
                    }

                    totalScannedCharges += accountCharges.Count;

                    var localSuccessCount = 0;
                    var localDeducted = 0m;

                    try
                    {
                        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
                        {
                            var balanceBefore = account.EducationCreditBalance;
                            var totalOutstanding = accountCharges.Sum(c => c.RemainingAmount);
                            var runningBalance = balanceBefore;

                            if (balanceBefore > 0)
                            {
                                var deductedAmount = Math.Min(balanceBefore, totalOutstanding);
                                var balanceAfter = balanceBefore - deductedAmount;

                                account.EducationCreditBalance = balanceAfter;
                                _accountRepository.Update(account);

                                var transaction = new EducationCreditTransaction
                                {
                                    Type = EducationCreditTransactionType.OutstandingAutoDeduction,
                                    Direction = EducationCreditTransactionDirection.Debit,
                                    Amount = deductedAmount,
                                    BalanceBefore = balanceBefore,
                                    BalanceAfter = balanceAfter,
                                    Description = $"Monthly auto-deduct - {runMonth}",
                                    EducationAccountId = account.Id
                                };
                                transaction.TryValidate();
                                await _transactionRepository.AddAsync(transaction, token);

                                var payment = new Payment
                                {
                                    EducationCreditTransaction = transaction,
                                    PaymentMethod = PaymentMethod.EducationBalance,
                                    Status = PaymentStatus.Succeeded,
                                    AccountNumberSnapshot = account.AccountNumber,
                                    CitizenNricSnapshot = account.Citizen.Nric,
                                    CitizenFullNameSnapshot = account.Citizen.FullName,
                                    TotalAmount = deductedAmount,
                                    PaidAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sgtZone)
                                };
                                payment.TryValidate();
                                await _paymentRepository.AddAsync(payment, token);

                                var remainingDeduction = deductedAmount;
                                foreach (var charge in accountCharges)
                                {
                                    var chargeRemainingBefore = charge.RemainingAmount;

                                    if (remainingDeduction > 0)
                                    {
                                        var allocationAmount = Math.Min(chargeRemainingBefore, remainingDeduction);
                                        var chargeRemainingAfter = chargeRemainingBefore - allocationAmount;
                                        var chargePaidAfter = charge.PaidAmount + allocationAmount;

                                        charge.PaidAmount = chargePaidAfter;
                                        charge.RemainingAmount = chargeRemainingAfter;
                                        charge.Status = chargeRemainingAfter == 0 ? ChargeStatus.Paid : charge.Status;
                                        charge.TryValidate();
                                        _chargeRepository.Update(charge);

                                        var allocation = new PaymentAllocation
                                        {
                                            Payment = payment,
                                            PaymentId = -1, // Temporary non-default value to pass TryValidate() before EF Core SaveChanges
                                            ChargeId = charge.Id,
                                            CourseNameSnapshot = charge.Enrollment.CourseNameSnapshot,
                                            SchoolNameSnapshot = charge.Enrollment.SchoolNameSnapshot,
                                            ChargeGrossAmountSnapshot = charge.GrossAmount,
                                            ChargeNetAmountSnapshot = charge.NetAmount,
                                            ChargeRemainingAmountSnapshot = chargeRemainingBefore,
                                            Amount = allocationAmount
                                        };
                                        allocation.TryValidate();
                                        await _paymentAllocationRepository.AddAsync(allocation, token);

                                        var runningBalanceBefore = runningBalance;
                                        runningBalance -= allocationAmount;
                                        var runningBalanceAfter = runningBalance;

                                        var target = new OutstandingDeductionTarget
                                        {
                                            OutstandingDeductionRunId = run.Id,
                                            ChargeId = charge.Id,
                                            EducationAccountId = account.Id,
                                            EducationCreditTransaction = transaction,
                                            Payment = payment,
                                            BalanceBefore = runningBalanceBefore,
                                            RemainingBefore = chargeRemainingBefore,
                                            DeductedAmount = allocationAmount,
                                            BalanceAfter = runningBalanceAfter,
                                            RemainingAfter = chargeRemainingAfter,
                                            Status = OutstandingDeductionTargetStatus.Success
                                        };
                                        target.TryValidate();
                                        await _targetRepository.AddAsync(target, token);

                                        remainingDeduction -= allocationAmount;
                                        localSuccessCount++;
                                    }
                                    else
                                    {
                                        var target = new OutstandingDeductionTarget
                                        {
                                            OutstandingDeductionRunId = run.Id,
                                            ChargeId = charge.Id,
                                            EducationAccountId = account.Id,
                                            BalanceBefore = runningBalance,
                                            RemainingBefore = chargeRemainingBefore,
                                            DeductedAmount = 0m,
                                            BalanceAfter = runningBalance,
                                            RemainingAfter = chargeRemainingBefore,
                                            Status = OutstandingDeductionTargetStatus.Skipped,
                                            FailureReason = "Insufficient balance for this charge."
                                        };
                                        target.TryValidate();
                                        await _targetRepository.AddAsync(target, token);
                                    }
                                }

                                localDeducted = deductedAmount;

                                if (!string.IsNullOrEmpty(account.Citizen.Email))
                                {
                                    var template = new EmailTemplateBuilder().BuildAutoDeductSuccessEmail(
                                        _configuration.AppInfo.Name,
                                        account.Citizen.FullName,
                                        deductedAmount,
                                        totalOutstanding - deductedAmount
                                    );
                                    await _outboxWriter.EnqueueEmailAsync(account.Citizen.Email, template, token);
                                }
                            }
                            else
                            {
                                foreach (var charge in accountCharges)
                                {
                                    var target = new OutstandingDeductionTarget
                                    {
                                        OutstandingDeductionRunId = run.Id,
                                        ChargeId = charge.Id,
                                        EducationAccountId = account.Id,
                                        BalanceBefore = 0m,
                                        RemainingBefore = charge.RemainingAmount,
                                        DeductedAmount = 0m,
                                        BalanceAfter = 0m,
                                        RemainingAfter = charge.RemainingAmount,
                                        Status = OutstandingDeductionTargetStatus.Skipped,
                                        FailureReason = "No balance available for deduction."
                                    };
                                    target.TryValidate();
                                    await _targetRepository.AddAsync(target, token);
                                }

                                if (!string.IsNullOrEmpty(account.Citizen.Email))
                                {
                                    var template = new EmailTemplateBuilder().BuildPaymentReminderEmail(
                                        _configuration.AppInfo.Name,
                                        account.Citizen.FullName,
                                        totalOutstanding
                                    );
                                    await _outboxWriter.EnqueueEmailAsync(account.Citizen.Email, template, token);
                                }
                            }
                        }, cancellationToken);

                        totalDeducted += localDeducted;
                        successChargesCount += localSuccessCount;
                    }
                    catch (Exception ex)
                    {
                        failedChargesCount += accountCharges.Count;
                        _logger.LogError(ex, "Error processing outstanding auto deduction for account ID {AccountId}", accountId);

                        try
                        {
                            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
                            {
                                foreach (var charge in accountCharges)
                                {
                                    var target = new OutstandingDeductionTarget
                                    {
                                        OutstandingDeductionRunId = run.Id,
                                        ChargeId = charge.Id,
                                        EducationAccountId = account.Id,
                                        BalanceBefore = account.EducationCreditBalance,
                                        RemainingBefore = charge.RemainingAmount,
                                        DeductedAmount = 0m,
                                        BalanceAfter = account.EducationCreditBalance,
                                        RemainingAfter = charge.RemainingAmount,
                                        Status = OutstandingDeductionTargetStatus.Failed,
                                        FailureReason = ex.GetBaseException().Message
                                    };
                                    if (target.FailureReason.Length > 1000)
                                    {
                                        target.FailureReason = target.FailureReason[..1000];
                                    }
                                    target.TryValidate();
                                    await _targetRepository.AddAsync(target, token);
                                }
                            }, cancellationToken);
                        }
                        catch (Exception logEx)
                        {
                            _logger.LogError(logEx, "Failed to write audit target for failed deduction on account ID {AccountId}", accountId);
                        }
                    }
                }
            }

            run.Status = OutstandingDeductionRunStatus.Completed;
            run.TotalScannedCharges = totalScannedCharges;
            run.TotalDeductedAmount = totalDeducted;
            run.SuccessCount = successChargesCount;
            run.FailedCount = failedChargesCount;
            run.CompletedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, sgtZone);

            _runRepository.Update(run);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
