using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class SchoolStudentSeedBuilder : ISeedBuilder
{
    public int Priority => 110;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        var students = new List<SchoolStudent>
        {
            new() { Id = 1, SchoolId = 1, EducationAccountId = 1, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 2, SchoolId = 2, EducationAccountId = 2, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 3, SchoolId = 3, EducationAccountId = 3, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 4, SchoolId = 4, EducationAccountId = 4, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 5, SchoolId = 5, EducationAccountId = 5, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 6, SchoolId = 6, EducationAccountId = 6, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 7, SchoolId = 7, EducationAccountId = 7, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 8, SchoolId = 8, EducationAccountId = 8, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 9, SchoolId = 9, EducationAccountId = 9, Status = SchoolStudentStatus.Active, CreatedAt = createdAt },
            new() { Id = 10, SchoolId = 10, EducationAccountId = 10, Status = SchoolStudentStatus.Inactive, CreatedAt = createdAt }
        };

        int currentId = 11;
        for (int schoolId = 1; schoolId <= 10; schoolId++)
        {
            for (int i = 0; i < 9; i++)
            {
                students.Add(new SchoolStudent
                {
                    Id = currentId,
                    SchoolId = schoolId,
                    EducationAccountId = currentId,
                    Status = SchoolStudentStatus.Active,
                    CreatedAt = createdAt
                });
                currentId++;
            }
        }

        modelBuilder.Entity<SchoolStudent>().HasData(students.ToArray());

        return modelBuilder;
    }
}