namespace Services.Courses.Utils
{
    public static class CourseCodeGenerator
    {
        public static async Task<string> GenerateUniqueAsync(
            IGenericRepository<Course> courseRepository,
            int schoolId,
            DateTime utcNow,
            ISet<string>? reservedCodes = null,
            CancellationToken cancellationToken = default)
        {
            return await BusinessCodeGenerator.GenerateUniqueAsync(
                BusinessCodeGenerator.CoursePrefix,
                (courseCode, token) => courseRepository.AnyAsync(
                    course => course.SchoolId == schoolId
                        && course.CourseCode == courseCode,
                    token),
                utcNow,
                reservedCodes,
                "Unable to generate a unique course code.",
                cancellationToken);
        }
    }
}
