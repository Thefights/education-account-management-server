using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class AuthAccountSeedBuilder : ISeedBuilder
{
    public int Priority => 20;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<AuthAccount>().HasData(
            Enumerable.Range(1, 15).Select(id => new AuthAccount
            {
                Id = id,
                Status = id % 5 == 0 ? AuthAccountStatus.Inactive : AuthAccountStatus.Active,
                FailedLoginCount = id % 3,
                LockedUntil = id == 5 ? createdAt.AddHours(1) : null,
                LastLoginAt = id % 2 == 0 ? createdAt.AddDays(id) : null,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
