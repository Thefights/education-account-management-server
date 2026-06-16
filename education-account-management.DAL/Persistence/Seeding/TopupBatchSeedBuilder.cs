using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupBatchSeedBuilder : ISeedBuilder
{
    public int Priority => 110;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<TopupBatch>().HasData(
            Enumerable.Range(1, 10).Select(id => new TopupBatch
            {
                Id = id,
                BatchCode = $"REG-BATCH-{id:000}",
                Status = id % 2 == 0 ? TopupBatchStatus.Completed : TopupBatchStatus.Executing,
                TotalTargetCount = 1,
                TotalAmount = 100m + id * 10m,
                ExecutedAt = id % 2 == 0 ? createdAt.AddDays(20 + id) : null,
                TopupRuleId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
