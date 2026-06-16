using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class AdhocTopupBatchSeedBuilder : ISeedBuilder
{
    public int Priority => 130;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<AdhocTopupBatch>().HasData(
            Enumerable.Range(1, 10).Select(id => new AdhocTopupBatch
            {
                Id = id,
                Reason = $"Adhoc adjustment reason {id:000}",
                Status = id % 2 == 0 ? TopupBatchStatus.Completed : TopupBatchStatus.Executing,
                TotalTargetCount = 1,
                TotalAmount = 50m + id * 5m,
                ExecutedAt = id % 2 == 0 ? createdAt.AddDays(40 + id) : null,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
