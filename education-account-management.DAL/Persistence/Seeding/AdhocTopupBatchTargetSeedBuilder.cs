using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class AdhocTopupBatchTargetSeedBuilder : ISeedBuilder
{
    public int Priority => 140;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<AdhocTopupBatchTarget>().HasData(
            Enumerable.Range(1, 10).Select(id => new AdhocTopupBatchTarget
            {
                Id = id,
                Amount = 50m + id * 5m,
                Status = id % 5 == 0
                    ? TopupTargetStatus.Failed
                    : id % 2 == 0 ? TopupTargetStatus.Success : TopupTargetStatus.Pending,
                FailureReason = id % 5 == 0 ? "Manual review rejected" : null,
                AdhocTopupBatchId = id,
                EducationAccountId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
