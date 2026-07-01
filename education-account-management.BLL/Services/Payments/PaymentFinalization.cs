using Charge = Models.Charge;
using PaymentIntent = Enums.PaymentIntent;
using PaymentMethod = Enums.PaymentMethod;

namespace Services.Payments;

public partial class StripeService
{
    // ===== Cụm 7: Payment finalization =====
    // Success mutates payment, charge, installment, balance and transaction history in one transaction.
    // Cancel/failed only terminally updates payment rows and leaves financial state unchanged.
    /// <summary>
    /// Hàm lõi xử lý thanh toán (Dùng chung cho cả Wallet và Stripe).
    /// Giao dịch nguyên tử (Atomic): Cập nhật trạng thái Payment, trừ nợ Charge, sinh/trừ kỳ trả góp, và ghi Log.
    /// </summary>
    private async Task ProcessPaymentInternalAsync(
        List<int> paymentIds, int accountId, string? externalReference,
        List<ChargeBillingActionItem> billingActions,
        PaymentStatus targetStatus, CancellationToken cancellationToken)
    {
        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            await LockEducationAccountAsync(accountId, token);

            var relatedPayments = await LoadPendingPaymentsAsync(paymentIds, token);
            if (relatedPayments.Count == 0) return;

            var educationAccount = await GetEducationAccountForProcessingAsync(accountId, token);

            await FinalizePendingPaymentsAsync(
                paymentIds, relatedPayments, educationAccount, externalReference,
                billingActions, targetStatus, token);
        }, cancellationToken);
    }

    private async Task<List<Payment>> LoadPendingPaymentsAsync(List<int> paymentIds, CancellationToken cancellationToken)
    {
        var relatedPayments = await _paymentRepository.Query(tracking: true)
            .Include(p => p.PaymentAllocations).ThenInclude(pa => pa.Charge).ThenInclude(c => c.Installments)
            .Where(p => paymentIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (relatedPayments.Count == 0) throw new InternalAppException($"Invalid {nameof(Payment)}s data.");

        // Idempotency guard: success/cancel/webhook can arrive more than once; already finalized rows are ignored.
        return relatedPayments.Where(p => p.Status == PaymentStatus.Pending).ToList();
    }

    private async Task<EducationAccount> GetEducationAccountForProcessingAsync(int accountId, CancellationToken cancellationToken)
    {
        var educationAccount = await _accountRepository.Query(tracking: true)
            .Include(a => a.Citizen)
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

        return educationAccount ?? throw new InternalAppException($"Account holder not found for process payment!");
    }

    private async Task FinalizePendingPaymentsAsync(
        List<int> paymentIds,
        List<Payment> relatedPayments,
        EducationAccount educationAccount,
        string? externalReference,
        List<ChargeBillingActionItem> billingActions,
        PaymentStatus targetStatus,
        CancellationToken token)
    {
        var isSuccess = targetStatus == PaymentStatus.Succeeded;

        // First persist terminal payment status. Only success is allowed to mutate charge/installment/balance.
        var totals = UpdatePaymentStatus(externalReference, targetStatus, relatedPayments, isSuccess);
        var totalPaid = isSuccess
            ? await ProcessSuccessfulPaymentsAsync(relatedPayments, billingActions, token)
            : 0m;

        if (isSuccess && (totals.CreditBalanceCovered > 0 || totals.OnlinePaymentCovered > 0))
            await CreateEducationCreditTransactionsAsync(
                relatedPayments, educationAccount,
                totals.CreditBalanceCovered, totals.OnlinePaymentCovered, token);

        await LogAuditAsync(
            AuditLogCategory.Billing,
            BuildPaymentFinishedAuditMessage(paymentIds, targetStatus),
            educationAccount.Id,
            educationAccount.Citizen.Nric,
            token);

        await SendPaymentEmailNotificationAsync(
            targetStatus, relatedPayments.First(), educationAccount, isSuccess,
            totalPaid, totals.CreditBalanceCovered, totals.OnlinePaymentCovered);

    }

    // ===== Cụm 8: Ledger transaction writing =====
    // Balance source decreases EducationCreditBalance. Online source records an Unchanged ledger entry
    // so transaction history can still show the online payment without altering balance.
    private async Task CreateEducationCreditTransactionsAsync(
        List<Payment> relatedPayments, EducationAccount educationAccount,
        decimal totalCreditBalanceCovered, decimal remainingPaymentViaStripe, CancellationToken token)
    {
        var balanceBefore = educationAccount.EducationCreditBalance;

        if (totalCreditBalanceCovered > 0 && balanceBefore < totalCreditBalanceCovered)
            throw new ValidationFailureException(new Dictionary<string, string>
                           { { "EducationCreditBalance", "Insufficient credit balance at payment finalization." }});

        var balanceAfter = balanceBefore - totalCreditBalanceCovered;
        educationAccount.EducationCreditBalance = balanceAfter;

        // Ghi nhận log giao dịch vào ví
        // Đối với thanh toán qua ví, direction là Decreased vì số dư bị trừ.
        if (totalCreditBalanceCovered > 0)
        {
            var coursesNames = string.Join(", ", relatedPayments
                .Where(p => p.PaymentMethod == PaymentMethod.EducationBalance)
                .SelectMany(p => p.PaymentAllocations)
                .Select(pa => pa.CourseNameSnapshot).Distinct());

            var description = $"Payment for Courses: {coursesNames}. fee covered via credit balance";
            var balancePayment = relatedPayments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.EducationBalance)
                ?? throw new InternalAppException($"Missing {nameof(PaymentMethod.EducationBalance)} payment for transaction creation.");

            var transactionDebit = new EducationCreditTransaction
            {
                Type = EducationCreditTransactionType.CourseFeePayment,
                Direction = EducationCreditTransactionDirection.Decreased,
                Amount = totalCreditBalanceCovered,
                BalanceBefore = balanceBefore,
                BalanceAfter = balanceAfter,
                Description = description,
                EducationAccountId = educationAccount.Id,
            };
            transactionDebit.TryValidate();
            await _transactionRepository.AddAsync(transactionDebit, token);
            balancePayment.EducationCreditTransaction = transactionDebit;
        }

        // Đối với thanh toán online, direction là Unchanged vì số dư không đổi.
        if (remainingPaymentViaStripe > 0)
        {

            var coursesNames = string.Join(", ", relatedPayments
               .Where(p => p.PaymentMethod == PaymentMethod.OnlinePayment)
               .SelectMany(p => p.PaymentAllocations)
               .Select(pa => pa.CourseNameSnapshot).Distinct());

            var description = $"Payment for Courses: {coursesNames}. fee covered via online card, Credit balance keep remaining";
            var onlinePayment = relatedPayments.FirstOrDefault(p => p.PaymentMethod == PaymentMethod.OnlinePayment)
                ?? throw new InternalAppException($"Missing {nameof(PaymentMethod.OnlinePayment)} payment for transaction creation.");

            var transactionNeutral = new EducationCreditTransaction
            {
                Type = EducationCreditTransactionType.CourseFeePayment,
                Direction = EducationCreditTransactionDirection.Unchanged,
                Amount = remainingPaymentViaStripe,
                BalanceBefore = balanceAfter,
                BalanceAfter = balanceAfter,
                Description = description,
                EducationAccountId = educationAccount.Id,
            };
            transactionNeutral.TryValidate();
            await _transactionRepository.AddAsync(transactionNeutral, token);
            onlinePayment.EducationCreditTransaction = transactionNeutral;
        }
    }

    // ===== Cụm 9: Charge/installment mutation =====
    // These helpers are intentionally small: each action updates charge balance first,
    // then installment state, then derives final charge status from stored statuses.
    private async Task<decimal> ProcessSuccessfulPaymentsAsync(
        List<Payment> relatedPayments,
        List<ChargeBillingActionItem> billingActions,
        CancellationToken token)
    {
        var allAllocations = relatedPayments.SelectMany(p => p.PaymentAllocations).ToList();
        var groupedByCharge = allAllocations.GroupBy(pa => pa.Charge);
        var totalPaid = 0m;

        foreach (var group in groupedByCharge)
        {
            var charge = group.Key;
            var billingAction = billingActions.First(action => action.ChargeId == charge.Id);
            decimal paidForCharge = group.Sum(a => a.Amount);
            totalPaid += paidForCharge;

            switch (billingAction.Intent)
            {
                case PaymentIntent.PayFull:
                    ApplyChargePayment(charge, paidForCharge);
                    break;

                case PaymentIntent.CreateInstallment:
                    await CreateInstallmentPlanAsync(charge, group.ToList(), paidForCharge, billingAction.PaymentPlanMonths!.Value, token);
                    break;

                case PaymentIntent.PayDueInstallments:
                case PaymentIntent.PayRemainingInstallments:
                    ApplyChargePayment(charge, paidForCharge);
                    MarkTargetInstallmentsPaid(charge, group);
                    break;

                default:
                    throw new InternalAppException("Invalid payment action.");
            }

            UpdateChargeStatusFromPersistedPaymentState(charge);
            charge.TryValidate();
        }

        return totalPaid;
    }

    private async Task CreateInstallmentPlanAsync(
        Charge charge,
        List<PaymentAllocation> allocations,
        decimal firstInstallmentAmount,
        int paymentPlanMonths,
        CancellationToken token)
    {
        var originalRemainingAmount = charge.RemainingAmount;
        ApplyChargePayment(charge, firstInstallmentAmount);
        charge.PaymentPlanMonths = paymentPlanMonths;

        var installmentDueDay = await GetInstallmentDueDayAsync(token);
        var firstDueDate = GetFirstInstallmentDueDate(charge.CourseEndDateSnapshot, installmentDueDay);
        var firstInstallment = new ChargeInstallment
        {
            ChargeId = charge.Id,
            InstallmentNumber = 1,
            Status = ChargeInstallmentStatus.Paid,
            DueDate = firstDueDate,
            Amount = firstInstallmentAmount
        };
        firstInstallment.TryValidate();
        await _installmentRepository.AddAsync(firstInstallment, token);

        foreach (var allocation in allocations)
        {
            allocation.ChargeInstallment = firstInstallment;
        }

        var allocatedAmount = firstInstallmentAmount;
        for (var installmentNumber = 2; installmentNumber <= paymentPlanMonths; installmentNumber++)
        {
            var amount = installmentNumber == paymentPlanMonths
                ? originalRemainingAmount - allocatedAmount
                : firstInstallmentAmount;

            allocatedAmount += amount;
            var installment = new ChargeInstallment
            {
                ChargeId = charge.Id,
                InstallmentNumber = installmentNumber,
                Status = ChargeInstallmentStatus.PendingPayment,
                DueDate = firstDueDate.AddMonths(installmentNumber - 1),
                Amount = amount
            };
            installment.TryValidate();
            await _installmentRepository.AddAsync(installment, token);
        }
    }

    private static void ApplyChargePayment(Charge charge, decimal amount)
    {
        charge.PaidAmount += amount;
        charge.RemainingAmount -= amount;
        if (charge.RemainingAmount <= 0)
        {
            charge.RemainingAmount = 0;
        }
    }

    private static void MarkTargetInstallmentsPaid(Charge charge, IEnumerable<PaymentAllocation> allocations)
    {
        var targetInstallmentIds = allocations
            .Where(allocation => allocation.ChargeInstallmentId.HasValue)
            .Select(allocation => allocation.ChargeInstallmentId!.Value)
            .Distinct()
            .ToList();

        foreach (var installment in charge.Installments.Where(installment => targetInstallmentIds.Contains(installment.Id)))
        {
            installment.Status = ChargeInstallmentStatus.Paid;
            installment.TryValidate();
        }
    }

    private async Task<int> GetInstallmentDueDayAsync(CancellationToken token)
    {
        var setting = await _applicationSettingRepository.Query(tracking: false)
            .FirstOrDefaultAsync(token);

        return setting?.InstallmentDueDay ?? new ApplicationSetting().InstallmentDueDay;
    }

    private static DateTime GetFirstInstallmentDueDate(DateTime chargeDueDate, int installmentDueDay)
    {
        var dueDay = Math.Clamp(installmentDueDay, 1, 28);
        var candidate = BuildInstallmentDueDate(chargeDueDate.Year, chargeDueDate.Month, dueDay);

        return candidate.Date < chargeDueDate.Date
            ? BuildInstallmentDueDate(chargeDueDate.AddMonths(1).Year, chargeDueDate.AddMonths(1).Month, dueDay)
            : candidate;
    }

    private static DateTime BuildInstallmentDueDate(int year, int month, int dueDay)
    {
        return new DateTime(year, month, dueDay, 0, 0, 0, DateTimeKind.Utc);
    }

    private static void UpdateChargeStatusFromPersistedPaymentState(Charge charge)
    {
        if (charge.RemainingAmount <= 0)
        {
            charge.Status = ChargeStatus.Paid;
            foreach (var installment in charge.Installments.Where(installment => installment.Status != ChargeInstallmentStatus.Paid))
            {
                installment.Status = ChargeInstallmentStatus.Paid;
                installment.TryValidate();
            }
            return;
        }

        charge.Status = charge.Installments.Any(installment => installment.Status == ChargeInstallmentStatus.Overdue)
            ? ChargeStatus.Overdue
            : ChargeStatus.PendingPayment;
    }

    private static PaymentProcessingTotals UpdatePaymentStatus(
        string? externalReference, PaymentStatus targetStatus, List<Payment> relatedPayments,
        bool isSuccess)
    {
        var paidAt = DateTime.UtcNow;
        var totalCreditBalanceCovered = 0m;
        var onlinePaymentCovered = 0m;

        foreach (var payment in relatedPayments)
        {
            payment.Status = targetStatus;
            if (payment.PaymentMethod == PaymentMethod.OnlinePayment)
            {
                payment.ExternalReference = externalReference ?? payment.ExternalReference;
            }
            if (isSuccess) payment.PaidAt = paidAt;

            if (isSuccess && payment.PaymentMethod == PaymentMethod.EducationBalance)
                totalCreditBalanceCovered += payment.TotalAmount;
            if (isSuccess && payment.PaymentMethod == PaymentMethod.OnlinePayment)
                onlinePaymentCovered += payment.TotalAmount;
        }

        return new PaymentProcessingTotals(totalCreditBalanceCovered, onlinePaymentCovered);
    }

    private static string BuildPaymentFinishedAuditMessage(List<int> paymentIds, PaymentStatus targetStatus)
    {
        var paymentIdText = string.Join(",", paymentIds);
        var actionMessage = $"{nameof(Payment)}s finished with status {targetStatus} - refs: {paymentIdText}";
        return actionMessage.Length > 90 ? actionMessage[..90] + "..." : actionMessage;
    }

}

