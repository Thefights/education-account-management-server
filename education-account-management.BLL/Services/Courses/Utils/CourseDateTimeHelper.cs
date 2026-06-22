namespace Services.Courses.Utils;

public static class CourseDateTimeHelper
{
    public static void NormalizeToUtc(Course course)
    {
        course.EnrollmentDueDate = NormalizeToUtc(
            course.EnrollmentDueDate,
            nameof(Course.EnrollmentDueDate));
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
