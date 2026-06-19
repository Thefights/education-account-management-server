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
     Enumerable.Range(1, 15).Select(id => new User
     {
         Id = id,
         Role = id switch
         {
             <= 6 => UserRole.SystemAdmin,

             7 => UserRole.FinanceAdmin,
             8 => UserRole.SchoolAdmin,

             _ => UserRole.AccountHolder
         },
         AuthAccountId = id,
         CitizenId = id,
         CreatedAt = createdAt
     }).ToArray());

        return modelBuilder;
    }

}
