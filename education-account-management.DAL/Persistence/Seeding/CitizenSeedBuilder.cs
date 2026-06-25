using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class CitizenSeedBuilder : ISeedBuilder
    {
        public int Priority => 10;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var primaryNames = new[]
            {
                "Sterling Quach",
                "Amelia Tan",
                "Marcus Lim",
                "Priya Nair",
                "Ethan Koh",
                "Hannah Lee",
                "Daniel Wong",
                "Sofia Chen",
                "Lucas Nguyen",
                "Maya Rahman",
                "Noah Teo",
                "Aisha Fernandez",
                "Ryan Chua",
                "Chloe Goh",
                "Irfan Hassan"
            };
            var givenNames = new[]
            {
                "Alina", "Benjamin", "Clara", "Darius", "Elena", "Farhan", "Grace", "Haruto",
                "Isabelle", "Jasper", "Keira", "Leon", "Mei Lin", "Nathan", "Olivia", "Pranav",
                "Qistina", "Rafael", "Selina", "Terence", "Umairah", "Victor", "Wen Jie", "Xavier",
                "Yasmin", "Zachary", "Adeline", "Brandon", "Celeste", "Damien", "Evelyn", "Faris",
                "Giselle", "Haziq", "Irene", "Jonas", "Kavya", "Lydia", "Malcolm", "Nadia"
            };
            var familyNames = new[]
            {
                "Ang", "Bala", "Chew", "Das", "Eng", "Foo", "Gan", "Ho", "Ismail", "Jeyaratnam",
                "Kwek", "Lim", "Mohamed", "Ng", "Ong", "Pillai", "Quek", "Rao", "Sim", "Tan",
                "Uddin", "Vasquez", "Wong", "Xu", "Yeo", "Zainal"
            };
            static string BuildSeedName(int id, string[] givenNames, string[] familyNames)
            {
                var givenName = givenNames[(id - 1) % givenNames.Length];
                var familyName = familyNames[((id - 1) / givenNames.Length + id) % familyNames.Length];
                return $"{givenName} {familyName}";
            }

            // Original 1-15
            var citizens = Enumerable.Range(1, 15).Select(id => new Citizen
            {
                Id = id,
                Nric = SingaporeNricUtil.Generate(id),
                FullName = primaryNames[id - 1],
                Email = $"{primaryNames[id - 1].ToLowerInvariant().Replace(" ", ".")}@example.com",
                PhoneNumber = $"+659{id:0000000}",
                ResidentialAddress = $"Residential block {id}, Singapore",
                MailingAddress = $"Mailing block {id}, Singapore",
                DateOfBirth = new DateOnly(
                    1990 + id,
                    ((id - 1) % 12) + 1,
                    Math.Min(id + 1, 28)),
                CitizenshipStatus = id % 5 == 0
                        ? CitizenshipStatus.Renounced
                        : id % 4 == 0 ? CitizenshipStatus.Revoked : CitizenshipStatus.Active,
                SchoolingStatus = id % 5 == 1 ? "Enrolled" :
                                      id % 5 == 2 ? "Not Enrolled" :
                                      id % 5 == 3 ? "Graduated" :
                                      id % 5 == 4 ? "Suspended" : "Withdrawn",
                CreatedAt = createdAt
            }).ToList();

            // Original 16
            citizens.Add(new Citizen
            {
                Id = 16,
                Nric = SingaporeNricUtil.Generate(16),
                FullName = "Natalie Seah",
                Email = "natalie.seah@example.com",
                PhoneNumber = "+6590000016",
                ResidentialAddress = "16 Orchard Link, Singapore",
                MailingAddress = "16 Orchard Link, Singapore",
                DateOfBirth = new DateOnly(2000, 1, 16),
                CitizenshipStatus = CitizenshipStatus.Active,
                SchoolingStatus = "Not Enrolled",
                CreatedAt = createdAt
            });

            // Add 17 to 100 for enrolled students
            citizens.AddRange(Enumerable.Range(17, 84).Select(id => new Citizen
            {
                Id = id,
                Nric = SingaporeNricUtil.Generate(id),
                FullName = BuildSeedName(id, givenNames, familyNames),
                Email = $"{BuildSeedName(id, givenNames, familyNames).ToLowerInvariant().Replace(" ", ".")}.{id}@example.com",
                PhoneNumber = $"+659{id:0000000}",
                ResidentialAddress = $"{id} Learning Grove, Singapore",
                MailingAddress = $"{id} Learning Grove, Singapore",
                DateOfBirth = new DateOnly(2000 + (id % 5), ((id - 1) % 12) + 1, 15),
                CitizenshipStatus = CitizenshipStatus.Active,
                SchoolingStatus = "Enrolled",
                CreatedAt = createdAt
            }));

            // Add 101 to 130 for unlinked/free citizens
            citizens.AddRange(Enumerable.Range(101, 30).Select(id => new Citizen
            {
                Id = id,
                Nric = SingaporeNricUtil.Generate(id),
                FullName = BuildSeedName(id, givenNames, familyNames),
                Email = $"{BuildSeedName(id, givenNames, familyNames).ToLowerInvariant().Replace(" ", ".")}.{id}@example.com",
                PhoneNumber = $"+659{id:0000000}",
                ResidentialAddress = $"{id} Community Crescent, Singapore",
                MailingAddress = $"{id} Community Crescent, Singapore",
                DateOfBirth = new DateOnly(2000 + (id % 5), ((id - 1) % 12) + 1, 15),
                CitizenshipStatus = CitizenshipStatus.Active,
                SchoolingStatus = "Not Enrolled",
                CreatedAt = createdAt
            }));

            modelBuilder.Entity<Citizen>().HasData(citizens.ToArray());

            return modelBuilder;
        }
    }
}
