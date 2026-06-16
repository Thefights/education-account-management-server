using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class EducationCreditTransactionSeedBuilder : ISeedBuilder
{
    public int Priority => 150;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<EducationCreditTransaction>().HasData(
            Enumerable.Range(1, 20).Select(id => new EducationCreditTransaction
            {
                Id = id,
                TransactionCode = SeedGuid(id),
                Type = id <= 10
                    ? id % 4 == 0 ? EducationCreditTransactionType.Adjustment : EducationCreditTransactionType.Topup
                    : EducationCreditTransactionType.Adjustment,
                Direction = EducationCreditTransactionDirection.Credit,
                Amount = id <= 10 ? 100m + id * 10m : 50m + (id - 10) * 5m,
                BalanceBefore = id <= 10 ? 1000m + id * 100m : 1200m + id * 50m,
                BalanceAfter = id <= 10 ? 1100m + id * 110m : 1250m + id * 55m,
                Description = id <= 10 ? $"Seed transaction {id:000}" : $"Seed adhoc transaction {id:000}",
                EducationAccountId = id <= 10 ? id : id - 10,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

    private static Guid SeedGuid(int id)
    {
        return Guid.Parse($"00000000-0000-0000-0000-{id:000000000000}");
    }

}
