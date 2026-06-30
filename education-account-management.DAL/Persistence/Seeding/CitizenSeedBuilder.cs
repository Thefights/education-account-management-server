using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class CitizenSeedBuilder : ISeedBuilder
    {
        public int Priority => 10;

        private static readonly string[] Streets =
        [
            "Tampines Street 82", "Bukit Batok West Avenue 8", "Ang Mo Kio Avenue 10",
            "Bedok North Road", "Jurong West Street 65", "Serangoon Central",
            "Choa Chu Kang Avenue 4", "Yishun Ring Road", "Hougang Avenue 8",
            "Pasir Ris Drive 6", "Woodlands Drive 16", "Sengkang East Way",
            "Bishan Street 22", "Toa Payoh Lorong 6", "Clementi Avenue 3"
        ];

        private static readonly string[] ScenarioGivenNames =
        [
            "Aarav", "Abigail", "Adrian", "Alicia", "Ananya", "Arjun", "Avery", "Bryan",
            "Caitlyn", "Damien", "Darren", "Elise", "Felicia", "Gabriel", "Hafiz", "Ishaan",
            "Janelle", "Jonas", "Kavya", "Leanne", "Mikhail", "Nadia", "Owen", "Pavithra",
            "Qi Wei", "Rania", "Sanjay", "Shayna", "Tariq", "Umairah", "Valerie", "Wei Lun",
            "Xin Yi", "Yuvraj", "Zara", "Joel", "Maya", "Naveen", "Siti", "Zhi Hao"
        ];

        private static readonly string[] ScenarioSurnames =
        [
            "Ang", "Bala", "Chua", "Das", "Eng", "Foo", "Gan", "Ho", "Ismail", "Jeyaratnam",
            "Kwek", "Lim", "Mohamed", "Ng", "Ong", "Pillai", "Quek", "Rao", "Sim", "Tan",
            "Uddin", "Vasquez", "Wong", "Xu", "Yeo"
        ];

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var citizens = new List<Citizen>();

            for (int id = 1; id <= SeedScenarioConstants.CitizenNames.Length; id++)
            {
                var name = SeedScenarioConstants.CitizenNames[id - 1];
                citizens.Add(CreateCitizen(
                    id,
                    name,
                    id == 1 ? "phuckhang1088@gmail.com" : CreateEmail(name),
                    GetSchoolingStatus(id),
                    isAutoSweepExcluded: false,
                    createdAt));
            }

            var totalSweepCitizens = SeedScenarioConstants.SweepAccountsPerDay * SeedScenarioConstants.SweepDayCount;
            for (int index = 0; index < totalSweepCitizens; index++)
            {
                var id = SeedScenarioConstants.SweepCitizenStartId + index;
                var name = CreateScenarioName(index);
                citizens.Add(CreateCitizen(
                    id,
                    name,
                    CreateEmail(name),
                    "Not Enrolled",
                    isAutoSweepExcluded: false,
                    createdAt));
            }

            for (int index = 0; index < SeedScenarioConstants.ManualCitizenCount; index++)
            {
                var id = SeedScenarioConstants.ManualCitizenStartId + index;
                var name = CreateScenarioName(totalSweepCitizens + index);
                citizens.Add(CreateCitizen(
                    id,
                    name,
                    CreateEmail(name),
                    "Not Enrolled",
                    isAutoSweepExcluded: true,
                    createdAt));
            }

            modelBuilder.Entity<Citizen>().HasData(citizens);

            return modelBuilder;
        }

        private static Citizen CreateCitizen(
            int id,
            string name,
            string email,
            string schoolingStatus,
            bool isAutoSweepExcluded,
            DateTime createdAt)
        {
            var address = CreateAddress(id);

            return new Citizen
            {
                Id = id,
                Nric = SingaporeNricUtil.Generate(id),
                FullName = name,
                Email = email,
                PhoneNumber = CreatePhoneNumber(id),
                ResidentialAddress = address,
                MailingAddress = address,
                DateOfBirth = CreateDateOfBirth(id),
                SchoolingStatus = schoolingStatus,
                IsSingaporean = true,
                IsAutoSweepExcluded = isAutoSweepExcluded,
                CreatedAt = createdAt
            };
        }

        private static string CreateScenarioName(int index)
        {
            var givenName = ScenarioGivenNames[index % ScenarioGivenNames.Length];
            var surname = ScenarioSurnames[(index / ScenarioGivenNames.Length) % ScenarioSurnames.Length];

            return $"{givenName} {surname}";
        }

        private static string CreateEmail(string name)
        {
            return $"{CreateEmailLocalPart(name)}@studentmail.edu.sg";
        }

        private static string CreatePhoneNumber(int id)
        {
            return $"+65{81000000 + ((id * 7919) % 9000000):D8}";
        }

        private static string CreateAddress(int id)
        {
            var blockNumber = 18 + ((id * 37) % 870);
            var street = Streets[(id - 1) % Streets.Length];

            return $"Block {blockNumber} {street}, Singapore";
        }

        private static DateOnly CreateDateOfBirth(int id)
        {
            return new DateOnly(1994 + (id % 12), ((id * 5) % 12) + 1, ((id * 7) % 28) + 1);
        }

        private static string GetSchoolingStatus(int id)
        {
            string[] statuses = ["Enrolled", "Not Enrolled", "Graduated", "Suspended", "Withdrawn"];

            return statuses[(id - 1) % statuses.Length];
        }

        private static string CreateEmailLocalPart(string value)
        {
            return value.ToLowerInvariant().Replace(" ", ".");
        }
    }
}
