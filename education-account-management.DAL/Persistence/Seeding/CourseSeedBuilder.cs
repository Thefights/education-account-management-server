using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class CourseSeedBuilder : ISeedBuilder
{
    public int Priority => 100;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, SchoolId = 1, CourseName = "Applied Mathematics", Description = "Foundation course in applied mathematics.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 2, SchoolId = 2, CourseName = "Computer Science Fundamentals", Description = "Introduction to programming and computing.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 3, SchoolId = 3, CourseName = "Business Communication", Description = "Professional written and verbal communication.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 4, SchoolId = 4, CourseName = "Environmental Science", Description = "Environmental systems and sustainability.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 5, SchoolId = 5, CourseName = "Digital Media Design", Description = "Digital design principles and production.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 6, SchoolId = 6, CourseName = "Hospitality Operations", Description = "Core hospitality service operations.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 7, SchoolId = 7, CourseName = "Electrical Engineering Basics", Description = "Fundamentals of electrical systems.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 8, SchoolId = 8, CourseName = "Creative Writing", Description = "Writing techniques across common genres.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 9, SchoolId = 9, CourseName = "Data Analytics", Description = "Data preparation, analysis and reporting.", Status = CourseStatus.Active, CreatedAt = createdAt },
            new Course { Id = 10, SchoolId = 10, CourseName = "Legacy Office Applications", Description = "Archived office applications programme.", Status = CourseStatus.Inactive, CreatedAt = createdAt });

        return modelBuilder;
    }
}
