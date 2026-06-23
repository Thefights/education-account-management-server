namespace Interfaces.Courses;

public interface ICourseLifecycleService
{
    Task<int> ProcessDateTransitionsAsync(
        DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task FinalizeEnrollmentAndGenerateChargesAsync(
        int courseId,
        DateTime utcNow,
        CancellationToken cancellationToken = default);
}
