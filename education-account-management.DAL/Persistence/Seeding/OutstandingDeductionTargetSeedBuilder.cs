using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class OutstandingDeductionTargetSeedBuilder : ISeedBuilder
{
    public int Priority => 180;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutstandingDeductionTarget>().HasData(
            new OutstandingDeductionTarget { Id = 1, OutstandingDeductionRunId = 1, ChargeId = 1, EducationAccountId = 1, BalanceBefore = 0m, RemainingBefore = 0m, DeductedAmount = 0m, BalanceAfter = 0m, RemainingAfter = 0m, Status = OutstandingDeductionTargetStatus.Skipped, FailureReason = "Charge was already paid.", CreatedAt = new DateTime(2025, 9, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 2, OutstandingDeductionRunId = 2, ChargeId = 2, EducationAccountId = 2, BalanceBefore = 100m, RemainingBefore = 70m, DeductedAmount = 0m, BalanceAfter = 100m, RemainingAfter = 70m, Status = OutstandingDeductionTargetStatus.Failed, FailureReason = "Concurrency conflict while updating the balance.", CreatedAt = new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 3, OutstandingDeductionRunId = 7, ChargeId = 6, EducationAccountId = 6, EducationCreditTransactionId = 7, PaymentId = 9, BalanceBefore = 500m, RemainingBefore = 120m, DeductedAmount = 50m, BalanceAfter = 450m, RemainingAfter = 70m, Status = OutstandingDeductionTargetStatus.Success, CreatedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 4, OutstandingDeductionRunId = 8, ChargeId = 8, EducationAccountId = 8, EducationCreditTransactionId = 8, PaymentId = 10, BalanceBefore = 70m, RemainingBefore = 130m, DeductedAmount = 70m, BalanceAfter = 0m, RemainingAfter = 60m, Status = OutstandingDeductionTargetStatus.Success, CreatedAt = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 5, OutstandingDeductionRunId = 3, ChargeId = 3, EducationAccountId = 3, BalanceBefore = 300m, RemainingBefore = 0m, DeductedAmount = 0m, BalanceAfter = 300m, RemainingAfter = 0m, Status = OutstandingDeductionTargetStatus.Skipped, FailureReason = "Charge was already paid.", CreatedAt = new DateTime(2025, 11, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 6, OutstandingDeductionRunId = 4, ChargeId = 4, EducationAccountId = 4, BalanceBefore = 0m, RemainingBefore = 0m, DeductedAmount = 0m, BalanceAfter = 0m, RemainingAfter = 0m, Status = OutstandingDeductionTargetStatus.Skipped, FailureReason = "No remaining amount was available.", CreatedAt = new DateTime(2025, 12, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 7, OutstandingDeductionRunId = 5, ChargeId = 5, EducationAccountId = 5, BalanceBefore = 200m, RemainingBefore = 0m, DeductedAmount = 0m, BalanceAfter = 200m, RemainingAfter = 0m, Status = OutstandingDeductionTargetStatus.Skipped, FailureReason = "Charge was already paid.", CreatedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 8, OutstandingDeductionRunId = 6, ChargeId = 7, EducationAccountId = 7, BalanceBefore = 0m, RemainingBefore = 0m, DeductedAmount = 0m, BalanceAfter = 0m, RemainingAfter = 0m, Status = OutstandingDeductionTargetStatus.Skipped, FailureReason = "Education account balance was zero.", CreatedAt = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 9, OutstandingDeductionRunId = 9, ChargeId = 9, EducationAccountId = 9, BalanceBefore = 250m, RemainingBefore = 250m, DeductedAmount = 0m, BalanceAfter = 250m, RemainingAfter = 250m, Status = OutstandingDeductionTargetStatus.Failed, FailureReason = "Run terminated before the target could be committed.", CreatedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc) },
            new OutstandingDeductionTarget { Id = 10, OutstandingDeductionRunId = 10, ChargeId = 10, EducationAccountId = 10, BalanceBefore = 0m, RemainingBefore = 300m, DeductedAmount = 0m, BalanceAfter = 0m, RemainingAfter = 300m, Status = OutstandingDeductionTargetStatus.Skipped, FailureReason = "Education account balance was zero.", CreatedAt = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc) });

        return modelBuilder;
    }
}
