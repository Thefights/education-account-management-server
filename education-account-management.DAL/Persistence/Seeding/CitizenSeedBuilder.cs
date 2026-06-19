using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class CitizenSeedBuilder : ISeedBuilder
{
    public int Priority => 10;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<Citizen>().HasData(
            Enumerable.Range(1, 15).Select(id => new Citizen
            {
                Id = id,
                Nric = $"S{id:0000000}A",
                SingpassSubjectId = $"singpass-subject-{id:000}",
                FullName = $"Citizen {id:000}",
                Email = $"citizen{id:000}@example.com",
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
            }).ToArray());

        return modelBuilder;
    }

}
