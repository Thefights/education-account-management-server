using Interfaces.Audit;
using Interfaces.Courses;
using Services.Courses.Utils;
using Interfaces.Notifications;

namespace Services.Courses
{
    public class CourseLifecycleService(
        IUnitOfWork unitOfWork,
        IAuditLogWriter auditLogWriter,
        INotificationWriter notificationWriter)
        : ICourseLifecycleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<Course> _courseRepository = unitOfWork.Repository<Course>();
        private readonly IGenericRepository<Charge> _chargeRepository = unitOfWork.Repository<Charge>();
        private readonly IGenericRepository<ChargeInstallment> _installmentRepository =
            unitOfWork.Repository<ChargeInstallment>();
        private readonly IGenericRepository<ApplicationSetting> _settingRepository =
            unitOfWork.Repository<ApplicationSetting>();
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly INotificationWriter _notificationWriter = notificationWriter;

        public async Task<int> ProcessDateTransitionsAsync(
            DateTime utcNow,
            CancellationToken cancellationToken = default)
        {
            utcNow = CourseDateTimeHelper.NormalizeToUtc(utcNow, nameof(utcNow));
            var overdueInstallmentCount = await MarkOverdueInstallmentsAsync(utcNow, cancellationToken);
            var courseIds = await _courseRepository.Query()
                .Where(course =>
                    (course.Status == CourseStatus.Enrolling
                        && course.EnrollmentDeadline <= utcNow)
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

            return transitionCount + overdueInstallmentCount;
        }

        private async Task<int> MarkOverdueInstallmentsAsync(
            DateTime utcNow,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var installments = await _installmentRepository.Query(tracking: true)
                        .Include(installment => installment.Charge)
                            .ThenInclude(charge => charge.Enrollment)
                                .ThenInclude(enrollment => enrollment.SchoolStudent)
                                    .ThenInclude(student => student.EducationAccount)
                                        .ThenInclude(account => account.Citizen)
                                            .ThenInclude(citizen => citizen.User)
                        .Where(installment => installment.Status == ChargeInstallmentStatus.PendingPayment
                            && installment.DueDate < utcNow)
                        .ToListAsync(token);

                    if (installments.Count == 0)
                    {
                        return 0;
                    }

                    var chargesToUpdate = new Dictionary<int, Charge>();
                    foreach (var installment in installments)
                    {
                        installment.Status = ChargeInstallmentStatus.Overdue;
                        installment.TryValidate();

                        if (installment.Charge.Status != ChargeStatus.Paid)
                        {
                            installment.Charge.Status = ChargeStatus.Overdue;
                            chargesToUpdate[installment.ChargeId] = installment.Charge;
                        }

                        var user = installment.Charge.Enrollment.SchoolStudent.EducationAccount.Citizen.User;
                        if (user != null)
                        {
                            await _notificationWriter.CreateAsync(
                                user.Id,
                                NotificationType.TuitionChargeOverdue,
                                NotificationSeverity.Warning,
                                "Payment overdue",
                                $"Your tuition fee for {installment.Charge.CourseNameSnapshot} is overdue.",
                                nameof(Charge),
                                installment.ChargeId,
                                new
                                {
                                    installment.Id,
                                    installment.DueDate,
                                    installment.Amount,
                                    chargeId = installment.ChargeId
                                },
                                token);
                        }
                    }

                    _installmentRepository.UpdateRange(installments);
                    if (chargesToUpdate.Count > 0)
                    {
                        _chargeRepository.UpdateRange(chargesToUpdate.Values.ToList());
                    }

                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.Billing,
                        $"Marked {installments.Count} installment(s) overdue.",
                        cancellationToken: token);
                    await _unitOfWork.SaveChangeAsync(token);
                    return installments.Count;
                },
                cancellationToken);
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
                            .ThenInclude(account => account.Citizen)
                            .ThenInclude(citizen => citizen.User)
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
                        && course.EnrollmentDeadline > utcNow)
                    {
                        if (requireEnrollmentFinalization)
                        {
                            throw new DataConflictException(
                                "The enrollment deadline has not been reached.");
                        }

                        return false;
                    }

                    var previousStatus = course.Status;
                    var targetStatus = DetermineNextStatus(course, utcNow);
                    if (targetStatus == previousStatus)
                    {
                        return false;
                    }

                    course.Status = targetStatus;
                    _courseRepository.Update(course);

                    var generatedChargeCount = 0;
                    if (previousStatus == CourseStatus.Enrolling)
                    {
                        generatedChargeCount = await GenerateMissingChargesAsync(course, token);
                    }

                    var overdueCharges = targetStatus == CourseStatus.Closed
                        ? course.Enrollments
                            .Select(enrollment => enrollment.Charge)
                            .Where(charge => charge != null
                                && charge.RemainingAmount > 0
                                && charge.Status == ChargeStatus.PendingPayment)
                            .Cast<Charge>()
                            .ToList()
                        : [];

                    foreach (var charge in overdueCharges)
                    {
                        charge.Status = ChargeStatus.Overdue;
                        charge.TryValidate();
                    }

                    if (overdueCharges.Count > 0)
                    {
                        _chargeRepository.UpdateRange(overdueCharges);
                        await _auditLogWriter.LogAsync(
                            AuditLogCategory.Billing,
                            $"Marked {overdueCharges.Count} charge(s) overdue for course {course.CourseCode}.",
                            cancellationToken: token);
                    }

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
            if (course.Status is CourseStatus.Draft or CourseStatus.Enrolling)
            {
                throw new DataConflictException(
                    "Charges can only be generated after enrollment has been finalized.");
            }

            var generatedCount = 0;
            foreach (var enrollment in course.Enrollments)
            {
                if (enrollment.Charge != null
                    || enrollment.SchoolStudent.Status != SchoolStudentStatus.Active
                    || enrollment.SchoolStudent.EducationAccount.Status == EducationAccountStatus.Closed)
                {
                    continue;
                }

                var taxRate = await GetTaxRateAsync(cancellationToken);
                var taxAmount = CourseFeeCalculator.CalculateTaxAmount(
                    course.CourseFeeAmount,
                    course.MiscFeeAmount,
                    taxRate);
                var grossAmount = course.CourseFeeAmount
                    + course.MiscFeeAmount
                    + taxAmount;
                var charge = new Charge
                {
                    EnrollmentId = enrollment.Id,
                    Status = grossAmount == 0m ? ChargeStatus.Paid : ChargeStatus.PendingPayment,
                    SchoolNameSnapshot = course.School.SchoolName,
                    CourseCodeSnapshot = course.CourseCode,
                    CourseNameSnapshot = course.CourseName,
                    CourseStartDateSnapshot = course.StartDate,
                    CourseEndDateSnapshot = course.EndDate,
                    CourseFeeAmountSnapshot = course.CourseFeeAmount,
                    MiscFeeAmountSnapshot = course.MiscFeeAmount,
                    GstAmountSnapshot = taxAmount,
                    TaxRateSnapshot = taxRate,
                    GrossAmount = grossAmount,
                    SubsidyAmount = 0m,
                    NetAmount = grossAmount,
                    PaidAmount = 0m,
                    RemainingAmount = grossAmount
                };

                charge.TryValidate();
                await _chargeRepository.AddAsync(charge, cancellationToken);

                var user = enrollment.SchoolStudent.EducationAccount.Citizen.User;
                if (user != null &&
                    charge.Status == ChargeStatus.PendingPayment &&
                    charge.RemainingAmount > 0)
                {
                    await _notificationWriter.CreateAsync(
                        user.Id,
                        NotificationType.TuitionChargeCreated,
                        NotificationSeverity.Info,
                        "New tuition fee generated",
                        $"A tuition fee for {course.CourseName} is now available for payment. Amount due: ${charge.RemainingAmount:N2}.",
                        nameof(Charge),
                        charge.Id,
                        new
                        {
                            course.Id,
                            course.CourseCode,
                            course.CourseName,
                            charge.GrossAmount,
                            charge.NetAmount,
                            charge.RemainingAmount
                        },
                        cancellationToken);
                }

                enrollment.Charge = charge;
                generatedCount++;
            }

            return generatedCount;
        }

        private async Task<decimal> GetTaxRateAsync(CancellationToken cancellationToken)
        {
            return await _settingRepository.Query()
                .OrderBy(setting => setting.Id)
                .Select(setting => setting.TaxRate)
                .FirstOrDefaultAsync(cancellationToken);
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
}
