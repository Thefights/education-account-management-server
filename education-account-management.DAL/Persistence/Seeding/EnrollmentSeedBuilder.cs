using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EnrollmentSeedBuilder : ISeedBuilder
    {
        public int Priority => 90;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var enrollments = new List<Enrollment>();

            for (int i = 1; i <= 50; i++)
            {
                enrollments.Add(CreateEnrollment(
                    id: i,
                    courseId: i,
                    schoolStudentId: i,
                    status: SeedScenarioConstants.SterlingCourseIds.Contains(i) || i % 8 != 0
                        ? EnrollmentStatus.Active
                        : EnrollmentStatus.Withdrawn,
                    createdAt));
            }

            foreach (var link in SeedScenarioConstants.GetAdditionalCourseTestEnrollments())
            {
                enrollments.Add(CreateEnrollment(
                    id: link.EnrollmentId,
                    courseId: link.CourseId,
                    schoolStudentId: link.SchoolStudentId,
                    status: EnrollmentStatus.Active,
                    createdAt));
            }

            modelBuilder.Entity<Enrollment>().HasData(enrollments);

            return modelBuilder;
        }

        private static Enrollment CreateEnrollment(
            int id,
            int courseId,
            int schoolStudentId,
            EnrollmentStatus status,
            DateTime createdAt)
        {
            var name = SeedScenarioConstants.CitizenNames[schoolStudentId - 1];
            var emailName = name.ToLowerInvariant().Replace(" ", ".");

            return new Enrollment
            {
                Id = id,
                CourseId = courseId,
                SchoolStudentId = schoolStudentId,
                Status = status,
                SchoolNameSnapshot = "Northview Secondary School",
                CourseNameSnapshot = SeedScenarioConstants.GetCourseName(courseId),
                CourseDescriptionSnapshot = "Course enrollment created for school-admin review.",
                CitizenNricSnapshot = SingaporeNricUtil.Generate(schoolStudentId),
                CitizenFullNameSnapshot = name,
                CitizenEmailSnapshot = schoolStudentId == 1
                    ? "phuckhang1088@gmail.com"
                    : $"{emailName}@example.com",
                CitizenPhoneNumberSnapshot = $"+659000{schoolStudentId:D4}",
                AccountNumberSnapshot = SeedBusinessCodeUtil.Generate(
                    BusinessCodeGenerator.EducationAccountPrefix,
                    schoolStudentId),
                CreatedAt = createdAt
            };
        }
    }
}
