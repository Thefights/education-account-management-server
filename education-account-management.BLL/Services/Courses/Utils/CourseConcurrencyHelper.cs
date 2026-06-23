namespace Services.Courses.Utils;

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
