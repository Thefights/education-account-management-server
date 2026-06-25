using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
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
                    IdempotencyKey = "manual-emergency-education-credit-2026-01",
                    ManualAmount = 100m,
                    ManualReason = "Emergency education credit approved by finance team.",
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
                    IdempotencyKey = "post-secondary-study-support-2026-01",
                    Status = TopupExecutionStatus.Completed,
                    TotalTargetCount = 1,
                    SuccessCount = 1,
                    FailedCount = 0,
                    TotalExecutedAmount = 200m,
                    TopupNameSnapshot = "Post-Secondary Study Support",
                    TopupAmountSnapshot = 200m,
                    ConditionsSnapshot = "{\"logicalOperator\":\"And\",\"conditions\":[{\"field\":\"SchoolingStatus\",\"operator\":\"Equals\",\"valueText\":\"Enrolled\"},{\"field\":\"Age\",\"operator\":\"Between\",\"valueNumber\":16,\"valueNumberTo\":25}]}",
                    CreatedAt = createdAt.AddDays(1)
                });

            return modelBuilder;
        }
    }
}
