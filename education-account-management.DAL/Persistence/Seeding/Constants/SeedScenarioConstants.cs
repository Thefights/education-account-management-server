namespace Persistence.Seeding.Constants
{
    public readonly record struct SeedEnrollmentLink(
        int EnrollmentId,
        int CourseId,
        int SchoolStudentId);

    public static class SeedScenarioConstants
    {
        public static readonly DateOnly SweepStartDate = new(2026, 6, 30);
        public const int SweepDayCount = 8;
        public const int SweepAccountsPerDay = 100;
        public const int SweepCreateRatio = 5;
        public const int SweepExtendRatio = 2;
        public const int SweepCloseRatio = 3;
        public const int SweepRatioTotal = SweepCreateRatio + SweepExtendRatio + SweepCloseRatio;
        public const int SweepCreateCountPerDay = SweepAccountsPerDay * SweepCreateRatio / SweepRatioTotal;
        public const int SweepExtendCountPerDay = SweepAccountsPerDay * SweepExtendRatio / SweepRatioTotal;
        public const int SweepCloseCountPerDay = SweepAccountsPerDay - SweepCreateCountPerDay - SweepExtendCountPerDay;
        public const int SweepCitizenStartId = 1001;
        public const int ManualCitizenStartId = 9001;
        public const int ManualCitizenCount = 10;

        public static readonly int[] SterlingCourseIds = [1, 3, 4, 5, 6, 8, 9, 11, 13, 14];
        public static readonly int[] SterlingWithdrawnCourseIds = [8, 11, 14];
        public static readonly int[] CourseTestStudentIds =
            Enumerable.Range(1, 33).Where(id => id is not 10 and not 20 and not 30).ToArray();

        public static readonly string[] CitizenNames =
        [
            "Sterling Quach", "Amelia Tan", "Marcus Lim", "Priya Nair", "Ethan Koh",
            "Hannah Lee", "Daniel Wong", "Sofia Chen", "Lucas Nguyen", "Maya Rahman",
            "Noah Teo", "Aisha Fernandez", "Ryan Chua", "Chloe Goh", "Irfan Hassan",
            "Natalie Seah", "Alina Ang", "Benjamin Bala", "Clara Chew", "Darius Das",
            "Elena Eng", "Farhan Foo", "Grace Gan", "Haruto Ho", "Isabelle Ismail",
            "Jasper Jeyaratnam", "Keira Kwek", "Leon Lim", "Mei Lin Mohamed", "Nathan Ng",
            "Olivia Ong", "Pranav Pillai", "Qistina Quek", "Rafael Rao", "Selina Sim",
            "Terence Tan", "Umairah Uddin", "Victor Vasquez", "Wen Jie Wong", "Xavier Xu",
            "Yasmin Yeo", "Zachary Zainal", "Adeline Ang", "Brandon Bala", "Celeste Chew",
            "Damien Das", "Evelyn Eng", "Faris Foo", "Giselle Gan", "Haziq Ho"
        ];

        private static readonly string[] CourseNames =
        [
            "Academic Writing", "Business Numeracy", "Digital Literacy", "Career Readiness",
            "Applied Science", "Financial Literacy", "Project Collaboration", "Data Skills",
            "Workplace Communication", "Software Foundations"
        ];

        public static IReadOnlyList<SeedEnrollmentLink> GetAdditionalCourseTestEnrollments()
        {
            var links = new List<SeedEnrollmentLink>();
            var nextId = 1000;

            foreach (var courseId in SterlingCourseIds)
            {
                foreach (var studentId in CourseTestStudentIds)
                {
                    if (courseId == studentId)
                    {
                        continue;
                    }

                    links.Add(new SeedEnrollmentLink(nextId++, courseId, studentId));
                }
            }

            return links;
        }

        public static int GetEnrollmentId(int courseId, int schoolStudentId)
        {
            if (courseId == schoolStudentId)
            {
                return courseId;
            }

            var link = GetAdditionalCourseTestEnrollments()
                .Single(item => item.CourseId == courseId && item.SchoolStudentId == schoolStudentId);

            return link.EnrollmentId;
        }

        public static CourseStatus GetCourseStatus(int courseId)
        {
            const int courseStatusCycle = 5;
            var statusIndex = courseId % courseStatusCycle;

            return statusIndex switch
            {
                0 => CourseStatus.Closed,
                2 => CourseStatus.Enrolling,
                4 => CourseStatus.InProgress,
                _ => CourseStatus.Upcoming
            };
        }

        public static string GetCourseName(int courseId)
        {
            return $"{CourseNames[(courseId - 1) % CourseNames.Length]} Cohort {courseId:D2}";
        }

        public static decimal GetCourseFee(int courseId) => 125m + (courseId * 5m);

        public static decimal GetMiscFee(int courseId) => 20m + ((courseId % 5) * 3m);

        public static decimal GetGst(int courseId)
        {
            return Math.Round((GetCourseFee(courseId) + GetMiscFee(courseId)) * 0.09m, 2);
        }

        public static decimal GetNetAmount(int courseId)
        {
            var subsidy = courseId % 4 == 0 ? 30m : 0m;
            return GetCourseFee(courseId) + GetMiscFee(courseId) + GetGst(courseId) - subsidy;
        }

        public static DateTime GetCourseStartDate(int courseId)
        {
            var day = ((courseId - 1) % 28) + 1;
            return GetCourseStatus(courseId) switch
            {
                CourseStatus.Closed => new DateTime(2026, 3, day, 0, 0, 0, DateTimeKind.Utc),
                CourseStatus.InProgress => new DateTime(2026, 5, day, 0, 0, 0, DateTimeKind.Utc),
                _ => new DateTime(2026, 8, day, 0, 0, 0, DateTimeKind.Utc)
            };
        }

        public static DateTime GetCourseEndDate(int courseId)
        {
            var day = ((courseId - 1) % 28) + 1;
            return GetCourseStatus(courseId) switch
            {
                CourseStatus.Closed => new DateTime(2026, 5, day, 0, 0, 0, DateTimeKind.Utc),
                CourseStatus.InProgress => new DateTime(2026, 9, day, 0, 0, 0, DateTimeKind.Utc),
                _ => new DateTime(2026, 10, day, 0, 0, 0, DateTimeKind.Utc)
            };
        }

        public static int GetSterlingCourseIndex(int courseId)
        {
            return Array.IndexOf(SterlingCourseIds, courseId);
        }

        public static bool IsSterlingEnrollment(int schoolStudentId) => schoolStudentId == 1;

        public static bool IsSterlingWithdrawnCourse(int courseId) =>
            SterlingWithdrawnCourseIds.Contains(courseId);

        public static int GetSterlingInstallmentId(int chargeId, int installmentNumber)
        {
            var courseIndex = SterlingCourseIds
                .Select((courseId, index) => new
                {
                    ChargeId = GetEnrollmentId(courseId, schoolStudentId: 1),
                    Index = index
                })
                .Single(item => item.ChargeId == chargeId)
                .Index;

            return 2000 + (courseIndex * 10) + installmentNumber;
        }

        public static Guid GetSeedGuid(string scope, int id)
        {
            var hash = System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes($"education-account-management:{scope}:{id}"));

            return new Guid(hash[..16]);
        }
    }
}
