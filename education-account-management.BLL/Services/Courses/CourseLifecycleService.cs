using Interfaces.Audit;
using Interfaces.Courses;
using Services.Courses.Utils;

namespace Services.Courses;

public class CourseLifecycleService(
    IUnitOfWork unitOfWork,
    IAuditLogWriter auditLogWriter)
    : ICourseLifecycleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IGenericRepository<Course> _courseRepository = unitOfWork.Repository<Course>();
    private readonly IGenericRepository<Charge> _chargeRepository = unitOfWork.Repository<Charge>();
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

    public async Task<int> ProcessDateTransitionsAsync(
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        utcNow = CourseDateTimeHelper.NormalizeToUtc(utcNow, nameof(utcNow));
        var courseIds = await _courseRepository.Query()
            .Where(course =>
                (course.Status == CourseStatus.Enrolling
                    && course.FasApplicationDueDate <= utcNow)
                || (course.Status == CourseStatus.Upcoming
                    && course.StartDate <= utcNow)
                || (course.Status == CourseStatus.InProgress
                    && course.EndDate <= utcNow))
            .Select(course => course.Id)
            .ToListAsync(cancellationToken);

        var transitionCount = 0;
        var failures = new List<Exception>();
        foreach (var courseId in courseIds)
        {
            try
            {
                if (await ProcessCourseAsync(
                        courseId,
                        utcNow,
                        requireEnrollmentFinalization: false,
                        cancellationToken))
                {
                    transitionCount++;
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                failures.Add(new InvalidOperationException(
                    $"Course {courseId} lifecycle transition failed.",
                    ex));
            }
        }

        if (failures.Count > 0)
        {
            throw new AggregateException(
                "One or more course lifecycle transitions failed and will be retried.",
                failures);
        }

        return transitionCount;
    }

    public async Task FinalizeEnrollmentAndGenerateChargesAsync(
        int courseId,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        utcNow = CourseDateTimeHelper.NormalizeToUtc(utcNow, nameof(utcNow));
        await ProcessCourseAsync(
            courseId,
            utcNow,
            requireEnrollmentFinalization: true,
            cancellationToken);
    }

    private async Task<bool> ProcessCourseAsync(
        int courseId,
        DateTime utcNow,
        bool requireEnrollmentFinalization,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteInTransactionAsync(
            async (_, token) =>
            {
                var course = await _courseRepository.Query(tracking: true)
                    .Include(item => item.School)
                    .Include(item => item.Enrollments)
                        .ThenInclude(enrollment => enrollment.Charge)
                    .Include(item => item.Enrollments)
                        .ThenInclude(enrollment => enrollment.SchoolStudent)
                        .ThenInclude(student => student.EducationAccount)
                    .FirstOrDefaultAsync(item => item.Id == courseId, token)
                    ?? throw new DataNotFoundException(typeof(Course), courseId);

                if (requireEnrollmentFinalization
                    && (course.Status is CourseStatus.Upcoming
                        or CourseStatus.InProgress
                        or CourseStatus.Closed))
                {
                    return false;
                }

                if (requireEnrollmentFinalization && course.Status != CourseStatus.Enrolling)
                {
                    throw new DataConflictException(
                        "Only an enrolling course can generate charges.");
                }

                if (course.Status == CourseStatus.Enrolling
                    && course.FasApplicationDueDate > utcNow)
                {
                    if (requireEnrollmentFinalization)
                    {
                        throw new DataConflictException(
                            "The FAS application deadline has not been reached.");
                    }

                    return false;
                }

                var generatedChargeCount = 0;
                if (course.Status == CourseStatus.Enrolling && course.EndDate > utcNow)
                {
                    generatedChargeCount = await GenerateMissingChargesAsync(course, token);
                }

                var previousStatus = course.Status;
                var targetStatus = DetermineNextStatus(course, utcNow);
                if (targetStatus == previousStatus)
                {
                    return false;
                }

                var outstandingCharges = targetStatus == CourseStatus.Closed
                    ? course.Enrollments
                        .Select(enrollment => enrollment.Charge)
                        .Where(charge => charge != null
                            && charge.RemainingAmount > 0
                            && charge.Status is ChargeStatus.Unpaid or ChargeStatus.PartiallyPaid)
                        .Cast<Charge>()
                        .ToList()
                    : [];

                foreach (var charge in outstandingCharges)
                {
                    charge.Status = ChargeStatus.Outstanding;
                    charge.BecameOutstandingAt ??= utcNow;
                    charge.TryValidate();
                }

                if (outstandingCharges.Count > 0)
                {
                    _chargeRepository.UpdateRange(outstandingCharges);
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.Billing,
                        $"Marked {outstandingCharges.Count} charge(s) outstanding for course {course.CourseCode}.",
                        cancellationToken: token);
                }

                course.Status = targetStatus;
                _courseRepository.Update(course);

                if (generatedChargeCount > 0)
                {
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.Billing,
                        $"Generated {generatedChargeCount} charge(s) for course {course.CourseCode}.",
                        cancellationToken: token);
                }

                await _auditLogWriter.LogAsync(
                    AuditLogCategory.StatusChange,
                    $"Course {course.CourseCode} automatically transitioned from {previousStatus} to {targetStatus}.",
                    cancellationToken: token);
                await _unitOfWork.SaveChangeAsync(token);
                return true;
            },
            cancellationToken);
    }

    private async Task<int> GenerateMissingChargesAsync(
        Course course,
        CancellationToken cancellationToken)
    {
        var generatedCount = 0;
        foreach (var enrollment in course.Enrollments)
        {
            if (enrollment.Charge != null
                || enrollment.SchoolStudent.Status != SchoolStudentStatus.Active
                || enrollment.SchoolStudent.EducationAccount.Status == EducationAccountStatus.Closed)
            {
                continue;
            }

            var grossAmount = course.CourseFeeAmount
                + course.MiscFeeAmount
                + course.GstAmount;
            var charge = new Charge
            {
                EnrollmentId = enrollment.Id,
                Status = grossAmount == 0m ? ChargeStatus.Paid : ChargeStatus.Unpaid,
                SchoolNameSnapshot = course.School.SchoolName,
                CourseCodeSnapshot = course.CourseCode,
                CourseNameSnapshot = course.CourseName,
                CourseDescriptionSnapshot = course.Description,
                CourseStartDateSnapshot = course.StartDate,
                CourseEndDateSnapshot = course.EndDate,
                CourseFeeAmountSnapshot = course.CourseFeeAmount,
                MiscFeeAmountSnapshot = course.MiscFeeAmount,
                GstAmountSnapshot = course.GstAmount,
                GrossAmount = grossAmount,
                SubsidyAmount = 0m,
                NetAmount = grossAmount,
                PaidAmount = 0m,
                RemainingAmount = grossAmount
            };

            charge.TryValidate();
            await _chargeRepository.AddAsync(charge, cancellationToken);
            enrollment.Charge = charge;
            generatedCount++;
        }

        return generatedCount;
    }

    private static CourseStatus DetermineNextStatus(Course course, DateTime utcNow)
    {
        return course.Status switch
        {
            CourseStatus.Enrolling when course.EndDate <= utcNow
                => CourseStatus.Closed,

            CourseStatus.Enrolling when course.StartDate <= utcNow
                => CourseStatus.InProgress,

            CourseStatus.Enrolling
                => CourseStatus.Upcoming,

            CourseStatus.Upcoming when course.EndDate <= utcNow
                => CourseStatus.Closed,

            CourseStatus.Upcoming when course.StartDate <= utcNow
                => CourseStatus.InProgress,

            CourseStatus.InProgress when course.EndDate <= utcNow
                => CourseStatus.Closed,

            _ => course.Status
        };
    }
}