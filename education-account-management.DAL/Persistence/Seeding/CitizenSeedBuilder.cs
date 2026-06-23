using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class CitizenSeedBuilder : ISeedBuilder
{
    public int Priority => 10;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        // Original 1-15
        var citizens = Enumerable.Range(1, 15).Select(id => new Citizen
        {
            Id = id,
            Nric = SingaporeNricUtil.Generate(id),
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
        }).ToList();

        // Original 16
        citizens.Add(new Citizen
        {
            Id = 16,
            Nric = SingaporeNricUtil.Generate(16),
            FullName = "Unlinked Test Citizen",
            Email = "unlinked.citizen@example.com",
            PhoneNumber = "+6590000016",
            ResidentialAddress = "16 Test Avenue, Singapore",
            MailingAddress = "16 Test Avenue, Singapore",
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
            FullName = $"School Student Citizen {id:000}",
            Email = $"student.citizen{id:000}@example.com",
            PhoneNumber = $"+659{id:0000000}",
            ResidentialAddress = $"{id} Student Avenue, Singapore",
            MailingAddress = $"{id} Student Avenue, Singapore",
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
            FullName = $"Free Citizen {id:000}",
            Email = $"free.citizen{id:000}@example.com",
            PhoneNumber = $"+659{id:0000000}",
            ResidentialAddress = $"{id} Free Avenue, Singapore",
            MailingAddress = $"{id} Free Avenue, Singapore",
            DateOfBirth = new DateOnly(2000 + (id % 5), ((id - 1) % 12) + 1, 15),
            CitizenshipStatus = CitizenshipStatus.Active,
            SchoolingStatus = "Not Enrolled",
            CreatedAt = createdAt
        }));

        modelBuilder.Entity<Citizen>().HasData(citizens.ToArray());

        return modelBuilder;
    }
}