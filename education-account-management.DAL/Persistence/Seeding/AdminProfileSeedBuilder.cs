using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class AdminProfileSeedBuilder : ISeedBuilder
    {
        public int Priority => 40;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            var adminProfiles = new List<AdminProfile>();

            string[] names = new[] { "Aaron Lim", "Belinda Tan", "Cheryl Ng", "Daniel Koh", "Elaine Chua", "Farhan Rahman", "Grace Lee", "Hannah Wong", "Isaac Teo", "Jasmine Goh", "Kenneth Low", "Liyana Hassan", "Marcus Chen", "Nadia Ismail", "Oliver Tan", "Priya Nair", "Qiao En Lim", "Rachel Ong", "Samuel Neo", "Theresa Yeo" };
            
            for (int i = 1; i <= 20; i++)
            {
                int mappedUserId = i;
                if (i == 4) mappedUserId = 21; // Since UserId=4 is an AccountHolder, we map this admin profile to the 21st user which is a SchoolAdmin.

                                int? schoolId = null;
                int[] schoolAdminIds = { 3, 7, 10, 13, 16, 17, 18, 19, 20, 21 };
                if (System.Array.IndexOf(schoolAdminIds, mappedUserId) >= 0)
                {
                    schoolId = System.Array.IndexOf(schoolAdminIds, mappedUserId) + 1;
                }

                adminProfiles.Add(new AdminProfile
                {
                    Id = i,
                    StaffCode = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.StaffPrefix, i),
                    FullName = names[i - 1],
                    Nric = SingaporeNricUtil.Generate(100 + i),
                    Email = $"admin{i:D3}@example.com",
                    PhoneNumber = $"+6580000{i:D3}",
                    UserId = mappedUserId,
                    SchoolId = schoolId,
                    CreatedAt = createdAt
                });
            }

            modelBuilder.Entity<AdminProfile>().HasData(adminProfiles);

            return modelBuilder;
        }
    }
}
