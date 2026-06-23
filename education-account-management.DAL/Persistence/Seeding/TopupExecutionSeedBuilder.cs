using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupExecutionSeedBuilder : ISeedBuilder
{
    public int Priority => 140;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt.AddDays(10);

        modelBuilder.Entity<TopupExecution>().HasData(
            new TopupExecution
            {
                Id = 1,
                ExecutionCode = "TOPUP-SEED-MANUAL-001",
                SourceType = TopupExecutionSourceType.Manual,
                IdempotencyKey = "seed-manual-topup-001",
                ManualAmount = 100m,
                ManualReason = "Seeded manual top-up execution.",
                Status = TopupExecutionStatus.Completed,
                TotalTargetCount = 2,
                SuccessCount = 1,
                FailedCount = 1,
                TotalExecutedAmount = 100m,
                CreatedAt = createdAt,
                CreatedBy = 7
            },
            new TopupExecution
            {
                Id = 2,
                ExecutionCode = "TOPUP-SEED-SYSTEM-001",
                SourceType = TopupExecutionSourceType.System,
                SystemTopupId = 21,
                IdempotencyKey = "seed-system-topup-001",
                Status = TopupExecutionStatus.Completed,
                TotalTargetCount = 1,
                SuccessCount = 1,
                FailedCount = 0,
                TotalExecutedAmount = 200m,
                TopupNameSnapshot = "System Top-up 021",
                TopupAmountSnapshot = 200m,
                ConditionsSnapshot = "{\"logicalOperator\":\"And\",\"conditions\":[]}",
                CreatedAt = createdAt.AddDays(1)
            });

        return modelBuilder;
    }
}