using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupExecutionTargetSeedBuilder : ISeedBuilder
{
    public int Priority => 151;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt.AddDays(10);

        modelBuilder.Entity<TopupExecutionTarget>().HasData(
            new TopupExecutionTarget
            {
                Id = 1,
                TopupExecutionId = 1,
                EducationAccountId = 1,
                AccountNumber = "EDU-2026-00000000001",
                Amount = 100m,
                Status = TopupTargetStatus.Success,
                EducationCreditTransactionId = 1,
                CreatedAt = createdAt,
                CreatedBy = 7
            },
            new TopupExecutionTarget
            {
                Id = 2,
                TopupExecutionId = 1,
                EducationAccountId = 2,
                AccountNumber = "EDU-2026-00000000002",
                Amount = 100m,
                Status = TopupTargetStatus.Failed,
                FailureReason = "Seeded failure for manual review.",
                CreatedAt = createdAt,
                CreatedBy = 7
            },
            new TopupExecutionTarget
            {
                Id = 3,
                TopupExecutionId = 2,
                EducationAccountId = 3,
                AccountNumber = "EDU-2026-00000000003",
                Amount = 200m,
                Status = TopupTargetStatus.Success,
                EducationCreditTransactionId = 3,
                CreatedAt = createdAt.AddDays(1)
            });

        return modelBuilder;
    }
}
