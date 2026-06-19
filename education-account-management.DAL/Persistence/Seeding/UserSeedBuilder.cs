using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class UserSeedBuilder : ISeedBuilder
{
    public int Priority => 60;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<User>().HasData(
            Enumerable.Range(1, 10).Select(id => new User
            {
                Id = id,
                Role = id switch
                {
                    1 => UserRole.SystemAdmin,
                    2 => UserRole.FinanceAdmin,
                    3 => UserRole.SchoolAdmin,
                    _ => UserRole.AccountHolder
                },
                AuthAccountId = id,
                CitizenId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
