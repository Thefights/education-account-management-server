using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class AdminProfileSeedBuilder : ISeedBuilder
{
    public int Priority => 70;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<AdminProfile>().HasData(
            Enumerable.Range(1, 10).Select(id => new AdminProfile
            {
                Id = id,
                StaffCode = $"STAFF-{id:000}",
                FullName = $"Admin Profile {id:000}",
                Email = $"admin{id:000}@example.com",
                UserId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
