using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class AdminProfileSeedBuilder : ISeedBuilder
    {
        public int Priority => 40;

        private static readonly string[] Names =
        [
            "Aaron Lim", "Belinda Tan", "Cheryl Ng", "Daniel Koh", "Elaine Chua",
            "Farhan Rahman", "Grace Lee", "Hannah Wong", "Isaac Teo", "Jasmine Goh",
            "Kenneth Low", "Liyana Hassan", "Marcus Chen", "Nadia Ismail", "Oliver Tan",
            "Priya Nair", "Qiao En Lim", "Rachel Ong", "Samuel Neo", "Theresa Yeo"
        ];

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var adminProfiles = new List<AdminProfile>();

            for (int i = 1; i <= Names.Length; i++)
            {
                var mappedUserId = i == 4
                    ? 21
                    : i;

                int? schoolId = null;
                int[] schoolAdminIds = [3, 7, 10, 13, 16, 17, 18, 19, 20, 21];
                var schoolAdminIndex = Array.IndexOf(schoolAdminIds, mappedUserId);
                if (schoolAdminIndex >= 0)
                {
                    schoolId = schoolAdminIndex + 1;
                }

                adminProfiles.Add(new AdminProfile
                {
                    Id = i,
                    StaffCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.StaffPrefix, i),
                    FullName = Names[i - 1],
                    Nric = SingaporeNricUtil.Generate(100 + i),
                    Email = CreateEmail(Names[i - 1]),
                    PhoneNumber = CreatePhoneNumber(i),
                    UserId = mappedUserId,
                    SchoolId = schoolId,
                    CreatedAt = createdAt
                });
            }

            modelBuilder.Entity<AdminProfile>().HasData(adminProfiles);

            return modelBuilder;
        }

        private static string CreateEmail(string name)
        {
            return $"{CreateEmailLocalPart(name)}@moe.gov.sg";
        }

        private static string CreatePhoneNumber(int id)
        {
            return $"+65{82000000 + ((id * 4739) % 17000000):D8}";
        }

        private static string CreateEmailLocalPart(string value)
        {
            return value.ToLowerInvariant().Replace(" ", ".");
        }
    }
}
