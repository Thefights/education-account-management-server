using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SchoolSeedBuilder : ISeedBuilder
    {
        public int Priority => 50;

        private static readonly string[] SchoolNames =
        [
            "Northview Secondary School", "Eastbridge Secondary School", "Westhaven Secondary School",
            "Southpoint Secondary School", "Central Heights School", "Riverside Learning Institute",
            "Lakeside Technical School", "Greenfield Academy", "Harbourfront School",
            "Hillcrest Education Centre", "Cedar Valley School", "Maple Ridge Academy",
            "Oceanview Institute", "Brighton Learning Centre", "Pioneer Technical College",
            "Summit Arts School", "Meridian Business School", "Silverstream Polytechnic",
            "Redwood Community College", "Bluewater Skills Institute", "Golden Grove Academy",
            "Sunrise Training Centre", "Crescent School of Technology", "Orchid City College",
            "Evergreen Education Hub", "Vista Applied Learning", "Compass Point School",
            "Newbridge Institute", "Heritage Skills Academy", "Frontier Science School",
            "Meadowbrook College", "Peakview Education Centre", "Bayfront Technical School",
            "Queensway Learning Academy", "Unity Continuing Education", "Elmwood Professional School",
            "Innovation Training Campus", "Riverbend School", "Stonefield Institute",
            "Seaside Skills Centre", "Northstar Academy", "Eastgate Technical College",
            "Westlake Learning Hub", "Southridge School", "Central Park Institute",
            "Hillview Polytechnic", "Greenridge Academy", "Harbour Bay College",
            "Lighthouse Skills School", "Civic Learning Centre"
        ];

        private static readonly string[] Streets =
        [
            "Marine Parade Road", "Bukit Timah Road", "Tampines Avenue 9", "Yio Chu Kang Road",
            "Clementi Road", "Woodlands Avenue 5", "Serangoon Avenue 3", "Jurong East Street 21",
            "Pasir Ris Drive 1", "Punggol Field"
        ];

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var schools = new List<School>();

            for (int id = 1; id <= SchoolNames.Length; id++)
            {
                var name = SchoolNames[id - 1];
                schools.Add(new School
                {
                    Id = id,
                    SchoolName = name,
                    Address = CreateAddress(id),
                    PhoneNumber = CreatePhoneNumber(id),
                    Email = CreateEmail(name),
                    Status = id % 10 == 0 ? SchoolStatus.Inactive : SchoolStatus.Active,
                    CreatedAt = createdAt
                });
            }

            modelBuilder.Entity<School>().HasData(schools);

            return modelBuilder;
        }

        private static string CreateEmail(string schoolName)
        {
            return $"{CreateEmailLocalPart(schoolName)}.admin@schools.gov.sg";
        }

        private static string CreatePhoneNumber(int id)
        {
            return $"+65{63000000 + ((id * 6151) % 900000):D8}";
        }

        private static string CreateAddress(int id)
        {
            var blockNumber = 21 + ((id * 43) % 280);
            var street = Streets[(id - 1) % Streets.Length];

            return $"{blockNumber} {street}, Singapore";
        }

        private static string CreateEmailLocalPart(string value)
        {
            return value
                .ToLowerInvariant()
                .Replace(" secondary school", string.Empty)
                .Replace(" school", string.Empty)
                .Replace(" ", ".");
        }
    }
}
