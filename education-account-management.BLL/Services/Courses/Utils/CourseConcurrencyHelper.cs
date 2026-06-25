namespace Services.Courses.Utils
{
    public static class CourseDateTimeHelper
    {
        public static void NormalizeToUtc(Course course)
        {
            course.EnrollmentDeadline = NormalizeToUtc(
                course.EnrollmentDeadline,
                nameof(Course.EnrollmentDeadline));
            course.FasApplicationDueDate = NormalizeToUtc(
                course.FasApplicationDueDate,
                nameof(Course.FasApplicationDueDate));
            course.StartDate = NormalizeToUtc(course.StartDate, nameof(Course.StartDate));
            course.EndDate = NormalizeToUtc(course.EndDate, nameof(Course.EndDate));
        }

        public static DateTime NormalizeToUtc(DateTime value, string propertyName)
        {
            if (value.Kind == DateTimeKind.Unspecified)
            {
                throw new ValidationFailureException(
                    propertyName,
                    $"{propertyName} must include a timezone offset.");
            }

            return value.ToUniversalTime();
        }
    }

    public static class CourseConcurrencyHelper
    {
        public static void Validate(byte[] suppliedRowVersion, byte[] currentRowVersion)
        {
            if (suppliedRowVersion.Length == 0)
            {
                throw new ValidationFailureException(
                    nameof(Course.RowVersion),
                    $"{nameof(Course.RowVersion)} is required.");
            }

            if (!suppliedRowVersion.SequenceEqual(currentRowVersion))
            {
                throw new DbUpdateConcurrencyException(
                    "The course was changed or deleted by another request.");
            }
        }
    }
}
