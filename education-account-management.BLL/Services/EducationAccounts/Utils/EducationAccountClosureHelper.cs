namespace Services.EducationAccounts.Utils;

public static class EducationAccountClosureHelper
{
    public static async Task<EducationAccountStatus> CloseOrExtendAsync(
        EducationAccount account,
        IGenericRepository<Charge> chargeRepository,
        IGenericRepository<EducationCreditTransaction> transactionRepository,
        string description,
        CancellationToken cancellationToken = default)
    {
        var hasOutstandingCharge = await chargeRepository.AnyAsync(
            charge => charge.Enrollment.SchoolStudent.EducationAccountId == account.Id
                && charge.RemainingAmount > 0
                && charge.Status != ChargeStatus.Paid
                && charge.Status != ChargeStatus.Cancelled,
            cancellationToken);

        if (hasOutstandingCharge)
        {
            account.Status = EducationAccountStatus.Extended;
            account.ClosedAt = null;
            account.TryValidate();
            return account.Status;
        }

        var balanceBefore = account.EducationCreditBalance;
        if (balanceBefore > 0)
        {
            var transaction = new EducationCreditTransaction
            {
                Type = EducationCreditTransactionType.ExpiredBalance,
                Direction = EducationCreditTransactionDirection.Debit,
                Amount = balanceBefore,
                BalanceBefore = balanceBefore,
                BalanceAfter = 0,
                Description = description,
                EducationAccountId = account.Id
            };
            transaction.TryValidate();
            await transactionRepository.AddAsync(transaction, cancellationToken);
        }

        account.EducationCreditBalance = 0;
        account.Status = EducationAccountStatus.Closed;
        account.ClosedAt ??= DateTime.UtcNow;
        account.TryValidate();
        return account.Status;
    }
}
