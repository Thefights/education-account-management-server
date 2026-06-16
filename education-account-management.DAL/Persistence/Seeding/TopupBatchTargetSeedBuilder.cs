using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupBatchTargetSeedBuilder : ISeedBuilder
{
    public int Priority => 120;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<TopupBatchTarget>().HasData(
            Enumerable.Range(1, 10).Select(id => new TopupBatchTarget
            {
                Id = id,
                Amount = 100m + id * 10m,
                Status = id % 5 == 0
                    ? TopupTargetStatus.Failed
                    : id % 2 == 0 ? TopupTargetStatus.Success : TopupTargetStatus.Pending,
                FailureReason = id % 5 == 0 ? "Eligibility check failed" : null,
                TopupBatchId = id,
                EducationAccountId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
