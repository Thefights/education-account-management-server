using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class SchoolSeedBuilder : ISeedBuilder
{
    public int Priority => 90;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<School>().HasData(
            new School { Id = 1, SchoolName = "Northview Secondary School", Address = "10 Northview Road, Singapore", PhoneNumber = "+6561000001", Email = "contact@northview.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 2, SchoolName = "Eastbridge Secondary School", Address = "20 Eastbridge Avenue, Singapore", PhoneNumber = "+6561000002", Email = "contact@eastbridge.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 3, SchoolName = "Westhaven Secondary School", Address = "30 Westhaven Street, Singapore", PhoneNumber = "+6561000003", Email = "contact@westhaven.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 4, SchoolName = "Southpoint Secondary School", Address = "40 Southpoint Drive, Singapore", PhoneNumber = "+6561000004", Email = "contact@southpoint.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 5, SchoolName = "Central Heights School", Address = "50 Central Heights, Singapore", PhoneNumber = "+6561000005", Email = "contact@centralheights.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 6, SchoolName = "Riverside Learning Institute", Address = "60 Riverside Walk, Singapore", PhoneNumber = "+6561000006", Email = "contact@riverside.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 7, SchoolName = "Lakeside Technical School", Address = "70 Lakeside Crescent, Singapore", PhoneNumber = "+6561000007", Email = "contact@lakeside.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 8, SchoolName = "Greenfield Academy", Address = "80 Greenfield Lane, Singapore", PhoneNumber = "+6561000008", Email = "contact@greenfield.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 9, SchoolName = "Harbourfront School", Address = "90 Harbourfront Road, Singapore", PhoneNumber = "+6561000009", Email = "contact@harbourfront.edu.sg", Status = SchoolStatus.Active, CreatedAt = createdAt },
            new School { Id = 10, SchoolName = "Hillcrest Education Centre", Address = "100 Hillcrest Avenue, Singapore", PhoneNumber = "+6561000010", Email = "contact@hillcrest.edu.sg", Status = SchoolStatus.Inactive, CreatedAt = createdAt });

        return modelBuilder;
    }
}
