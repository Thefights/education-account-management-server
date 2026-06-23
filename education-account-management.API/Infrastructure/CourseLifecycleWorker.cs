namespace Infrastructure;

public class CourseLifecycleWorker(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<CourseLifecycleWorker> logger,
    TimeProvider timeProvider)
    : BaseBackgroundJob(serviceScopeFactory, logger)
{
    private readonly TimeProvider _timeProvider = timeProvider;

    protected override string JobName => nameof(CourseLifecycleWorker);

    protected override TimeSpan Interval => TimeSpan.FromMinutes(5);

    protected override async Task ExecuteJobAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var lifecycleService = serviceProvider.GetRequiredService<ICourseLifecycleService>();
        await lifecycleService.ProcessDateTransitionsAsync(
           _timeProvider.GetUtcNow().UtcDateTime,
           cancellationToken);
    }
}