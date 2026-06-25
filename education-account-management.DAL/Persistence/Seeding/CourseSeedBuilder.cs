using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class CourseSeedBuilder : ISeedBuilder
    {
        public int Priority => 100;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var courses = new List<Course>
            {
                new() { Id = 1, SchoolId = 1, Status = CourseStatus.Upcoming, CourseCode = "CRS-2026-A1B2C3D", CourseName = "Quantitative Problem Solving", Description = "Applied numeracy and structured problem-solving for academic pathways.", CourseFeeAmount = 100m, MiscFeeAmount = 10m, GstAmount = 9.90m, EnrollmentDeadline = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 2, SchoolId = 2, Status = CourseStatus.Upcoming, CourseCode = "CRS-2026-B2C3D4E", CourseName = "Software Foundations with C#", Description = "Core programming concepts, debugging, and application structure.", CourseFeeAmount = 115m, MiscFeeAmount = 12m, GstAmount = 11.43m, EnrollmentDeadline = new DateTime(2026, 7, 2, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 7, 2, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 8, 2, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 3, SchoolId = 3, Status = CourseStatus.Upcoming, CourseCode = "CRS-2026-C3D4E5F", CourseName = "Professional Communication Lab", Description = "Practical writing, presentation, and workplace communication skills.", CourseFeeAmount = 130m, MiscFeeAmount = 15m, GstAmount = 13.05m, EnrollmentDeadline = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 10, 2, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 4, SchoolId = 4, Status = CourseStatus.InProgress, CourseCode = "CRS-2026-D4E5F6G", CourseName = "Sustainability Science Workshop", Description = "Environmental systems, resource planning, and sustainability practice.", CourseFeeAmount = 145m, MiscFeeAmount = 17m, GstAmount = 14.58m, EnrollmentDeadline = new DateTime(2026, 2, 4, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 2, 4, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 5, 4, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 5, SchoolId = 5, Status = CourseStatus.InProgress, CourseCode = "CRS-2026-E5F6G7H", CourseName = "Digital Media Production", Description = "Digital storytelling, layout, and media production workflows.", CourseFeeAmount = 160m, MiscFeeAmount = 20m, GstAmount = 16.20m, EnrollmentDeadline = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 6, SchoolId = 6, Status = CourseStatus.InProgress, CourseCode = "CRS-2026-F6G7H8J", CourseName = "Service Operations Practicum", Description = "Customer operations, service standards, and scenario-based practice.", CourseFeeAmount = 175m, MiscFeeAmount = 22m, GstAmount = 17.73m, EnrollmentDeadline = new DateTime(2026, 2, 6, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 2, 6, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 8, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 7, SchoolId = 7, Status = CourseStatus.Closed, CourseCode = "CRS-2026-G7H8J9K", CourseName = "Electrical Systems Fundamentals", Description = "Foundational electrical theory, components, and safety practices.", CourseFeeAmount = 190m, MiscFeeAmount = 25m, GstAmount = 19.35m, EnrollmentDeadline = new DateTime(2026, 2, 7, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 2, 7, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 3, 7, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 8, SchoolId = 8, Status = CourseStatus.Closed, CourseCode = "CRS-2026-H8J9K0L", CourseName = "Creative Writing Studio", Description = "Narrative craft, editing practice, and guided writing critique.", CourseFeeAmount = 205m, MiscFeeAmount = 27m, GstAmount = 20.88m, EnrollmentDeadline = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 3, 8, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 7, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 9, SchoolId = 9, Status = CourseStatus.Closed, CourseCode = "CRS-2026-J9K0L1M", CourseName = "Data Analytics Essentials", Description = "Data preparation, analysis, visualization, and reporting fundamentals.", CourseFeeAmount = 220m, MiscFeeAmount = 30m, GstAmount = 22.50m, EnrollmentDeadline = new DateTime(2026, 2, 9, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 2, 9, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 3, 9, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new() { Id = 10, SchoolId = 10, Status = CourseStatus.Closed, CourseCode = "CRS-2026-K0L1M2N", CourseName = "Office Productivity for Business", Description = "Document, spreadsheet, and presentation workflows for business users.", CourseFeeAmount = 235m, MiscFeeAmount = 32m, GstAmount = 24.03m, EnrollmentDeadline = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), FasApplicationDueDate = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), StartDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt }
            };

            var topics = new[]
            {
                "Academic Writing", "Business Numeracy", "Digital Literacy", "Career Readiness",
                "Applied Science", "Financial Literacy", "Project Collaboration", "Data Skills",
                "Workplace Communication"
            };

            var descriptions = new[]
            {
                "Structured lessons with guided practice and applied assessments.",
                "Practical workshops designed for school-based learning pathways.",
                "Hands-on sessions focused on confidence, fluency, and real-world use.",
                "Applied learning activities with clear progression checkpoints.",
                "Foundation training for students preparing for advanced modules."
            };

            var courseId = 11;
            for (var schoolId = 1; schoolId <= 10; schoolId++)
            {
                for (var index = 0; index < 9; index++)
                {
                    var topic = topics[index];
                    var startDate = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddDays((schoolId - 1) * 3 + index * 7);
                    var enrollmentDeadline = startDate.AddDays(-21);
                    var courseFee = 120m + schoolId * 10m + index * 15m;
                    var miscFee = 15m + index * 2m;
                    courses.Add(new Course
                    {
                        Id = courseId,
                        SchoolId = schoolId,
                        Status = index < 3 ? CourseStatus.Draft : CourseStatus.Enrolling,
                        CourseCode = $"CRS-2026-S{schoolId:00}{index + 1:00}X{(char)('A' + index)}",
                        CourseName = $"{topic} - School {schoolId} Cohort {index + 1}",
                        Description = descriptions[(schoolId + index) % descriptions.Length],
                        CourseFeeAmount = courseFee,
                        MiscFeeAmount = miscFee,
                        GstAmount = decimal.Round((courseFee + miscFee) * 0.09m, 2),
                        EnrollmentDeadline = enrollmentDeadline,
                        FasApplicationDueDate = enrollmentDeadline,
                        StartDate = startDate,
                        EndDate = startDate.AddMonths(2),
                        CreatedAt = createdAt
                    });
                    courseId++;
                }
            }

            modelBuilder.Entity<Course>().HasData(courses);

            return modelBuilder;
        }
    }
}
