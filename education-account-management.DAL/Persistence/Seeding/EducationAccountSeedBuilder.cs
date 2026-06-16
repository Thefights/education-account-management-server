using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class EducationAccountSeedBuilder : ISeedBuilder
{
    public int Priority => 80;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<EducationAccount>().HasData(
            Enumerable.Range(1, 10).Select(id => new EducationAccount
            {
                Id = id,
                AccountNumber = $"EA{id:000000000000000000}",
                EducationCreditBalance = 1000m + id * 100m,
                Status = id % 6 == 0
                    ? EducationAccountStatus.Closed
                    : id % 4 == 0 ? EducationAccountStatus.Extended : EducationAccountStatus.Active,
                OpenedAt = createdAt.AddDays(id),
                ClosedAt = id % 6 == 0 ? createdAt.AddDays(100 + id) : null,
                CitizenId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
