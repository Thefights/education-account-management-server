using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class EnrollmentSeedBuilder : ISeedBuilder
{
    public int Priority => 120;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment { Id = 1, CourseId = 1, EducationAccountId = 1, EnrolledAt = createdAt.AddDays(10), CreatedAt = createdAt },
            new Enrollment { Id = 2, CourseId = 2, EducationAccountId = 2, EnrolledAt = createdAt.AddDays(11), CreatedAt = createdAt },
            new Enrollment { Id = 3, CourseId = 3, EducationAccountId = 3, EnrolledAt = createdAt.AddDays(12), CompletedAt = createdAt.AddDays(72), CreatedAt = createdAt },
            new Enrollment { Id = 4, CourseId = 4, EducationAccountId = 4, EnrolledAt = createdAt.AddDays(13), CreatedAt = createdAt },
            new Enrollment { Id = 5, CourseId = 5, EducationAccountId = 5, EnrolledAt = createdAt.AddDays(14), WithdrawnAt = createdAt.AddDays(24), CreatedAt = createdAt },
            new Enrollment { Id = 6, CourseId = 6, EducationAccountId = 6, EnrolledAt = createdAt.AddDays(15), CreatedAt = createdAt },
            new Enrollment { Id = 7, CourseId = 7, EducationAccountId = 7, EnrolledAt = createdAt.AddDays(16), CompletedAt = createdAt.AddDays(76), CreatedAt = createdAt },
            new Enrollment { Id = 8, CourseId = 8, EducationAccountId = 8, EnrolledAt = createdAt.AddDays(17), CreatedAt = createdAt },
            new Enrollment { Id = 9, CourseId = 9, EducationAccountId = 9, EnrolledAt = createdAt.AddDays(18), CreatedAt = createdAt },
            new Enrollment { Id = 10, CourseId = 10, EducationAccountId = 10, EnrolledAt = createdAt.AddDays(19), WithdrawnAt = createdAt.AddDays(29), CreatedAt = createdAt });

        return modelBuilder;
    }
}
