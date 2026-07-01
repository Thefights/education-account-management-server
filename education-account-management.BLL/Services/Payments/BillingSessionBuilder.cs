using Stripe.Checkout;

namespace Services.Payments;

public partial class StripeService
{
    // ===== Cụm 3: Billing item calculation and Stripe line items =====
    // BillingItem is the normalized payable target. It can point directly to a charge
    // or to a specific installment depending on the action endpoint.
    private static List<BillingItem> CreateBillingItems(
        List<ChargeBillingActionItem> chargeBillingActionItems,
        List<Charge> charges)
    {
        var billingItems = new List<BillingItem>();
        foreach (var charge in charges)
        {
            var billingAction = chargeBillingActionItems.First(ci => ci.ChargeId == charge.Id);

            switch (billingAction.Intent)
            {
                case PaymentIntent.PayFull:
                    billingItems.Add(new BillingItem(charge, billingAction.Intent, null, null, charge.RemainingAmount));
                    break;
                case PaymentIntent.CreateInstallment:
                    decimal amountToPay = Math.Round(charge.RemainingAmount / billingAction.PaymentPlanMonths!.Value, 2, MidpointRounding.AwayFromZero);
                    billingItems.Add(new BillingItem(charge, billingAction.Intent, billingAction.PaymentPlanMonths, null, amountToPay));
                    break;
                case PaymentIntent.PayCurrentInstallment:
                    var firstPending = charge.Installments.OrderBy(installment => installment.InstallmentNumber).First();
                    billingItems.Add(new BillingItem(charge, billingAction.Intent, null, firstPending.Id, firstPending.Amount));
                    break;
                case PaymentIntent.PayRemainingInstallments:
                    foreach (var inst in charge.Installments.OrderBy(installment => installment.InstallmentNumber))
                    {
                        billingItems.Add(new BillingItem(charge, billingAction.Intent, null, inst.Id, inst.Amount));
                    }
                    break;
                default:
                    throw new InternalAppException("Invalid Payment Intent.");
            }
        }

        return billingItems;
    }

    /// <summary>
    /// Chuẩn bị danh sách LineItem (Sản phẩm/Khóa học) để hiển thị trên giao diện thanh toán của Stripe.
    /// Phân bổ số dư ví (Credit Balance) vào từng khóa học để tính ra số tiền cuối cùng Stripe cần thu.
    /// </summary>
    private static void CreateItemsCheckOut(List<BillingItem> billingItems, decimal accountCreditBalance, List<SessionLineItemOptions> lineItems)
    {
        var chargeGroups = billingItems.GroupBy(b => b.Charge);
        foreach (var group in chargeGroups)
        {
            var charge = group.Key;
            var enrollment = charge.Enrollment;
            decimal chargeAmountToPay = group.Sum(b => b.AmountToPay);

            decimal coveredByCreditBalance = Math.Min(chargeAmountToPay, accountCreditBalance);
            decimal amountToPayViaStripe = chargeAmountToPay - coveredByCreditBalance;

            accountCreditBalance -= coveredByCreditBalance;
            if (amountToPayViaStripe <= 0)
            {
                continue;
            }

            lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "sgd",
                    UnitAmount = (long)(amountToPayViaStripe * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = enrollment.CourseNameSnapshot,
                        Description = coveredByCreditBalance > 0
                            ? $"• Remaining Owed: {charge.RemainingAmount:C} SGD | \n" +
                              $" • Current Payment Portion: {chargeAmountToPay:C} SGD | \n " +
                              $"• Credit Balance Applied: -{coveredByCreditBalance:C} SGD"
                            : $"• Remaining Owed: {charge.RemainingAmount:C} SGD | \n" +
                              $" • Payment Portion: {chargeAmountToPay:C} SGD"
                    }
                },
                Quantity = 1
            });
        }
    }

    /// <summary>
    /// Lưu nháp (Pending) các bản ghi Payment và PaymentAllocation vào Database trước khi gọi Stripe API.
    /// Sử dụng logic Waterfall: Ưu tiên rút cạn số dư Ví (Wallet) trước, phần dư mới đẩy sang Stripe.
    /// </summary>
    private async Task<List<Payment>> CreatePrePaymentAndPaymentAllocationsAsync(
        List<Charge> charges, List<BillingItem> billingItems, EducationAccount educationAccount,
        decimal totalOwed, decimal balanceToApply,
        decimal remainingToPayViaStripe, CancellationToken cancellationToken)
    {
        var payments = new List<Payment>();

        var walletPayment = await AddPendingPaymentIfNeededAsync(
            PaymentMethod.EducationBalance, balanceToApply, educationAccount, charges, payments, cancellationToken);

        var stripePayment = await AddPendingPaymentIfNeededAsync(
            PaymentMethod.OnlinePayment, remainingToPayViaStripe, educationAccount, charges, payments, cancellationToken);

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        decimal currentWalletBalance = balanceToApply;

        // Allocate every billing target by waterfall: consume balance first, then online payment.
        foreach (var item in billingItems)
        {
            var charge = item.Charge;
            decimal amountToPay = item.AmountToPay;
            decimal coveredByWallet = Math.Min(amountToPay, currentWalletBalance);
            decimal coveredByStripe = amountToPay - coveredByWallet;

            currentWalletBalance -= coveredByWallet;

            if (coveredByWallet > 0 && walletPayment != null)
            {
                await AddPaymentAllocationAsync(walletPayment.Id, item, coveredByWallet, cancellationToken);
            }

            if (coveredByStripe > 0 && stripePayment != null)
            {
                await AddPaymentAllocationAsync(stripePayment.Id, item, coveredByStripe, cancellationToken);
            }
        }

        foreach (var p in payments)
        {
            var actionMsg = $"{nameof(Payment)} {p.Id} Init for paying {nameof(Course)}s {nameof(Charge)} - Total Owed: {totalOwed}, Credit Applied: {balanceToApply}";
            if (actionMsg.Length > 90) actionMsg = actionMsg.Substring(0, 90) + "...";
            await LogAuditAsync(AuditLogCategory.Billing, actionMsg, educationAccount.Id, educationAccount.Citizen.Nric, cancellationToken);
        }

        return payments;
    }

    // ===== Cụm 4: Payment reservation helpers =====
    // Pending Payment rows are split by source: EducationBalance and OnlinePayment.
    // PaymentAllocation is the source of truth for which charge/installment each source covers.
    private async Task<Payment?> AddPendingPaymentIfNeededAsync(
        PaymentMethod paymentMethod,
        decimal amount,
        EducationAccount educationAccount,
        List<Charge> charges,
        List<Payment> payments,
        CancellationToken cancellationToken)
    {
        if (amount <= 0) return null;

        var payment = new Payment
        {
            Status = PaymentStatus.Pending,
            PaymentMethod = paymentMethod,
            TotalAmount = amount,
            AccountNumberSnapshot = educationAccount.AccountNumber,
            CitizenNricSnapshot = charges.First().Enrollment.CitizenNricSnapshot,
            CitizenFullNameSnapshot = charges.First().Enrollment.CitizenFullNameSnapshot
        };
        payment.TryValidate();
        await _paymentRepository.AddAsync(payment, cancellationToken);
        payments.Add(payment);

        return payment;
    }

    private async Task AddPaymentAllocationAsync(int paymentId, BillingItem item, decimal amount, CancellationToken cancellationToken)
    {
        var charge = item.Charge;
        var allocation = new PaymentAllocation
        {
            PaymentId = paymentId,
            ChargeId = charge.Id,
            ChargeInstallmentId = item.TargetInstallmentId,
            CourseNameSnapshot = charge.Enrollment.CourseNameSnapshot,
            SchoolNameSnapshot = charge.Enrollment.SchoolNameSnapshot,
            ChargeGrossAmountSnapshot = charge.GrossAmount,
            ChargeNetAmountSnapshot = charge.NetAmount,
            ChargeRemainingAmountSnapshot = charge.RemainingAmount,
            Amount = amount
        };
        allocation.TryValidate();
        await _paymentAllocationRepository.AddAsync(allocation, cancellationToken);
    }

}

