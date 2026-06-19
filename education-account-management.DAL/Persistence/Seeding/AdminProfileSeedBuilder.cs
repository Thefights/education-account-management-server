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
            Enumerable.Range(1, 8).Select(id => new AdminProfile
            {
                Id = id,
                StaffCode = $"STAFF-{id:000}",
                FullName = id switch
                {
                    <= 6 => $"System Administrator {id:000}",
                    7 => "Finance Administrator",
                    _ => "School Administrator"
                },
                Nric = SingaporeNricUtil.Generate(id),
                Email = $"admin{id:000}@example.com",
                PhoneNumber = $"+6591{id:000000}",
                UserId = id,
                SchoolId = id == 8 ? 1 : null,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
