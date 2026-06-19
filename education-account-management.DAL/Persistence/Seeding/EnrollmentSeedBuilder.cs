using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class EnrollmentSeedBuilder : ISeedBuilder
{
    public int Priority => 120;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;
        var courseNames = new[]
        {
            "Applied Mathematics",
            "Computer Science Fundamentals",
            "Business Communication",
            "Environmental Science",
            "Digital Media Design",
            "Hospitality Operations",
            "Electrical Engineering Basics",
            "Creative Writing",
            "Data Analytics",
            "Legacy Office Applications"
        };
        var courseDescriptions = new[]
        {
            "Foundation course in applied mathematics.",
            "Introduction to programming and computing.",
            "Professional written and verbal communication.",
            "Environmental systems and sustainability.",
            "Digital design principles and production.",
            "Core hospitality service operations.",
            "Fundamentals of electrical systems.",
            "Writing techniques across common genres.",
            "Data preparation, analysis and reporting.",
            "Archived office applications programme."
        };
        var schoolNames = new[]
        {
            "Northview Secondary School",
            "Eastbridge Secondary School",
            "Westhaven Secondary School",
            "Southpoint Secondary School",
            "Central Heights School",
            "Riverside Learning Institute",
            "Lakeside Technical School",
            "Greenfield Academy",
            "Harbourfront School",
            "Hillcrest Education Centre"
        };

        static Enrollment CreateEnrollment(
            int id,
            DateTime enrolledAt,
            DateTime createdAt,
            string[] courseNames,
            string[] courseDescriptions,
            string[] schoolNames,
            DateTime? completedAt = null,
            DateTime? withdrawnAt = null)
        {
            return new Enrollment
            {
                Id = id,
                CourseId = id,
                EducationAccountId = id,
                SchoolNameSnapshot = schoolNames[id - 1],
                CourseNameSnapshot = courseNames[id - 1],
                CourseDescriptionSnapshot = courseDescriptions[id - 1],
                CitizenNricSnapshot = $"S{id:0000000}A",
                CitizenFullNameSnapshot = $"Citizen {id:000}",
                CitizenEmailSnapshot = $"citizen{id:000}@example.com",
                CitizenPhoneNumberSnapshot = $"+659{id:0000000}",
                AccountNumberSnapshot = $"EA{id:000000000000000000}",
                EnrolledAt = enrolledAt,
                CompletedAt = completedAt,
                WithdrawnAt = withdrawnAt,
                CreatedAt = createdAt
            };
        }

        modelBuilder.Entity<Enrollment>().HasData(
            CreateEnrollment(1, createdAt.AddDays(10), createdAt, courseNames, courseDescriptions, schoolNames),
            CreateEnrollment(2, createdAt.AddDays(11), createdAt, courseNames, courseDescriptions, schoolNames),
            CreateEnrollment(3, createdAt.AddDays(12), createdAt, courseNames, courseDescriptions, schoolNames, completedAt: createdAt.AddDays(72)),
            CreateEnrollment(4, createdAt.AddDays(13), createdAt, courseNames, courseDescriptions, schoolNames),
            CreateEnrollment(5, createdAt.AddDays(14), createdAt, courseNames, courseDescriptions, schoolNames, withdrawnAt: createdAt.AddDays(24)),
            CreateEnrollment(6, createdAt.AddDays(15), createdAt, courseNames, courseDescriptions, schoolNames),
            CreateEnrollment(7, createdAt.AddDays(16), createdAt, courseNames, courseDescriptions, schoolNames, completedAt: createdAt.AddDays(76)),
            CreateEnrollment(8, createdAt.AddDays(17), createdAt, courseNames, courseDescriptions, schoolNames),
            CreateEnrollment(9, createdAt.AddDays(18), createdAt, courseNames, courseDescriptions, schoolNames),
            CreateEnrollment(10, createdAt.AddDays(19), createdAt, courseNames, courseDescriptions, schoolNames, withdrawnAt: createdAt.AddDays(29)));

        return modelBuilder;
    }
}
