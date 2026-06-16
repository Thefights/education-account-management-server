using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupBatchTargetTransactionSeedBuilder : ISeedBuilder
{
    public int Priority => 160;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<TopupBatchTargetTransaction>().HasData(
            Enumerable.Range(1, 10).Select(id => new TopupBatchTargetTransaction
            {
                Id = id,
                TopupBatchTargetId = id,
                EducationCreditTransactionId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
