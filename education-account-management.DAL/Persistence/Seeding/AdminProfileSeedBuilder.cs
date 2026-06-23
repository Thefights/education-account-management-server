using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class AdminProfileSeedBuilder : ISeedBuilder
    {
        public int Priority => 70;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            string[] staffCodes =
            [
                "STAFF-K7M2Q",
                "STAFF-R4T9X",
                "STAFF-C8N5W",
                "STAFF-P3H7V",
                "STAFF-Y6D2K",
                "STAFF-M9Q4A",
                "STAFF-T5X8C",
                "STAFF-H2W6R"
            ];

            modelBuilder.Entity<AdminProfile>().HasData(
                Enumerable.Range(1, 8).Select(id => new AdminProfile
                {
                    Id = id,
                    StaffCode = staffCodes[id - 1],
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
}
