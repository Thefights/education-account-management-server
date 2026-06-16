using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class AdhocTopupBatchTargetTransactionSeedBuilder : ISeedBuilder
{
    public int Priority => 170;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<AdhocTopupBatchTargetTransaction>().HasData(
            Enumerable.Range(1, 10).Select(id => new AdhocTopupBatchTargetTransaction
            {
                Id = id,
                AdhocTopupBatchTargetId = id,
                EducationCreditTransactionId = id + 10,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
