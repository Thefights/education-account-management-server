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
            new Course { Id = 1, SchoolId = 1, CourseName = "Applied Mathematics", Description = "Foundation course in applied mathematics.", Status = CourseStatus.Active, CourseFeeAmount = 100m, MiscFeeAmount = 10m, GstAmount = 10m, CreatedAt = createdAt },
            new Course { Id = 2, SchoolId = 2, CourseName = "Computer Science Fundamentals", Description = "Introduction to programming and computing.", Status = CourseStatus.Active, CourseFeeAmount = 115m, MiscFeeAmount = 12m, GstAmount = 13m, CreatedAt = createdAt },
            new Course { Id = 3, SchoolId = 3, CourseName = "Business Communication", Description = "Professional written and verbal communication.", Status = CourseStatus.Active, CourseFeeAmount = 130m, MiscFeeAmount = 15m, GstAmount = 15m, CreatedAt = createdAt },
            new Course { Id = 4, SchoolId = 4, CourseName = "Environmental Science", Description = "Environmental systems and sustainability.", Status = CourseStatus.Active, CourseFeeAmount = 145m, MiscFeeAmount = 17m, GstAmount = 18m, CreatedAt = createdAt },
            new Course { Id = 5, SchoolId = 5, CourseName = "Digital Media Design", Description = "Digital design principles and production.", Status = CourseStatus.Active, CourseFeeAmount = 160m, MiscFeeAmount = 20m, GstAmount = 20m, CreatedAt = createdAt },
            new Course { Id = 6, SchoolId = 6, CourseName = "Hospitality Operations", Description = "Core hospitality service operations.", Status = CourseStatus.Active, CourseFeeAmount = 175m, MiscFeeAmount = 22m, GstAmount = 23m, CreatedAt = createdAt },
            new Course { Id = 7, SchoolId = 7, CourseName = "Electrical Engineering Basics", Description = "Fundamentals of electrical systems.", Status = CourseStatus.Active, CourseFeeAmount = 190m, MiscFeeAmount = 25m, GstAmount = 25m, CreatedAt = createdAt },
            new Course { Id = 8, SchoolId = 8, CourseName = "Creative Writing", Description = "Writing techniques across common genres.", Status = CourseStatus.Active, CourseFeeAmount = 205m, MiscFeeAmount = 27m, GstAmount = 28m, CreatedAt = createdAt },
            new Course { Id = 9, SchoolId = 9, CourseName = "Data Analytics", Description = "Data preparation, analysis and reporting.", Status = CourseStatus.Active, CourseFeeAmount = 220m, MiscFeeAmount = 30m, GstAmount = 30m, CreatedAt = createdAt },
            new Course { Id = 10, SchoolId = 10, CourseName = "Legacy Office Applications", Description = "Archived office applications programme.", Status = CourseStatus.Inactive, CourseFeeAmount = 235m, MiscFeeAmount = 32m, GstAmount = 33m, CreatedAt = createdAt });

        return modelBuilder;
    }
}