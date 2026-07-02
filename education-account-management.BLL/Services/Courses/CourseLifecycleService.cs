using Interfaces.Audit;
using Interfaces.Courses;
using Helpers.FasSchemes;
using Services.Courses.Utils;
using Interfaces.Email;
using Interfaces.Notifications;
using Services.Payments;

namespace Services.Courses
{
    public class CourseLifecycleService(
        IUnitOfWork unitOfWork,
        IAuditLogWriter auditLogWriter,
        INotificationWriter notificationWriter,
        IOutboxWriter outboxWriter,
        EmailTemplateBuilder emailTemplateBuilder,
        AppConfiguration configuration)
        : ICourseLifecycleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<Course> _courseRepository = unitOfWork.Repository<Course>();
        private readonly IGenericRepository<Charge> _chargeRepository = unitOfWork.Repository<Charge>();
        private readonly IGenericRepository<ChargeInstallment> _installmentRepository =
            unitOfWork.Repository<ChargeInstallment>();
        private readonly IGenericRepository<ApplicationSetting> _settingRepository =
            unitOfWork.Repository<ApplicationSetting>();
        private readonly IGenericRepository<FasApplication> _fasApplicationRepository =
            unitOfWork.Repository<FasApplication>();
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly INotificationWriter _notificationWriter = notificationWriter;
        private readonly IOutboxWriter _outboxWriter = outboxWriter;
        private readonly EmailTemplateBuilder _emailTemplateBuilder = emailTemplateBuilder;
        private readonly AppConfiguration _configuration = configuration;

        public async Task<int> ProcessDateTransitionsAsync(
            DateTime utcNow,
            CancellationToken cancellationToken = default)
        {
            utcNow = CourseDateTimeHelper.NormalizeToUtc(utcNow, nameof(utcNow));
            var reminderCount = await SendDueRemindersAsync(utcNow, cancellationToken);
            var overdueChargeCount = await MarkOverdueChargesAsync(utcNow, cancellationToken);
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

            return transitionCount + reminderCount + overdueChargeCount + overdueInstallmentCount;
        }

        private async Task<int> SendDueRemindersAsync(
            DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var reminderDate = utcNow.Date.AddDays(7);
            var reminderCount = 0;

            var charges = await _chargeRepository.Query(tracking: false)
                .Include(charge => charge.Enrollment)
                    .ThenInclude(enrollment => enrollment.SchoolStudent)
                        .ThenInclude(student => student.EducationAccount)
                            .ThenInclude(account => account.Citizen)
                .Where(charge => charge.Status == ChargeStatus.PendingPayment
                    && charge.RemainingAmount > 0
                    && !charge.PaymentPlanMonths.HasValue
                    && !charge.Installments.Any()
                    && charge.CourseEndDateSnapshot.Date == reminderDate)
                .ToListAsync(cancellationToken);

            foreach (var charge in charges)
            {
                var citizen = charge.Enrollment.SchoolStudent.EducationAccount.Citizen;
                if (string.IsNullOrWhiteSpace(citizen.Email))
                {
                    continue;
                }

                var template = _emailTemplateBuilder.BuildPaymentDueReminderEmail(
                    citizen.FullName,
                    charge.CourseNameSnapshot,
                    charge.RemainingAmount,
                    charge.CourseEndDateSnapshot,
                    BuildAccountHolderPortalLink("/account-holder/tuition-payment"));

                await _outboxWriter.EnqueueEmailOnceAsync(citizen.Email, template, cancellationToken);
                reminderCount++;
            }

            var installments = await _installmentRepository.Query(tracking: false)
                .Include(installment => installment.Charge)
                    .ThenInclude(charge => charge.Enrollment)
                        .ThenInclude(enrollment => enrollment.SchoolStudent)
                            .ThenInclude(student => student.EducationAccount)
                                .ThenInclude(account => account.Citizen)
                .Where(installment => installment.Status == ChargeInstallmentStatus.PendingPayment
                    && installment.DueDate.Date == reminderDate)
                .ToListAsync(cancellationToken);

            foreach (var installment in installments)
            {
                var citizen = installment.Charge.Enrollment.SchoolStudent.EducationAccount.Citizen;
                if (string.IsNullOrWhiteSpace(citizen.Email))
                {
                    continue;
                }

                var template = _emailTemplateBuilder.BuildInstallmentDueReminderEmail(
                    citizen.FullName,
                    installment.Charge.CourseNameSnapshot,
                    installment.Amount,
                    installment.DueDate,
                    BuildAccountHolderPortalLink("/account-holder/tuition-payment"));

                await _outboxWriter.EnqueueEmailOnceAsync(citizen.Email, template, cancellationToken);
                reminderCount++;
            }

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return reminderCount;
        }

        private async Task<int> MarkOverdueChargesAsync(
            DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var utcToday = utcNow.Date;
            return await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var charges = await _chargeRepository.Query(tracking: true)
                        .Include(charge => charge.Enrollment)
                            .ThenInclude(enrollment => enrollment.SchoolStudent)
                                .ThenInclude(student => student.EducationAccount)
                                    .ThenInclude(account => account.Citizen)
                                        .ThenInclude(citizen => citizen.User)
                        .Where(charge => charge.Status == ChargeStatus.PendingPayment
                            && charge.RemainingAmount > 0
                            && !charge.PaymentPlanMonths.HasValue
                            && !charge.Installments.Any()
                            && charge.CourseEndDateSnapshot.Date < utcToday)
                        .ToListAsync(token);

                    if (charges.Count == 0)
                    {
                        return 0;
                    }

                    foreach (var charge in charges)
                    {
                        charge.Status = ChargeStatus.Overdue;
                        charge.TryValidate();

                        var user = charge.Enrollment.SchoolStudent.EducationAccount.Citizen.User;
                        if (user != null)
                        {
                            await _notificationWriter.CreateAsync(
                                user.Id,
                                NotificationType.TuitionChargeOverdue,
                                NotificationSeverity.Warning,
                                "Payment overdue",
                                $"Your tuition fee for {charge.CourseNameSnapshot} is overdue.",
                                nameof(Charge),
                                charge.Id,
                                new
                                {
                                    charge.Id,
                                    dueDate = charge.CourseEndDateSnapshot,
                                    charge.RemainingAmount
                                },
                                token);
                        }

                        var citizen = charge.Enrollment.SchoolStudent.EducationAccount.Citizen;
                        if (!string.IsNullOrWhiteSpace(citizen.Email))
                        {
                            var template = _emailTemplateBuilder.BuildPaymentOverdueEmail(
                                citizen.FullName,
                                charge.CourseNameSnapshot,
                                charge.RemainingAmount,
                                BuildAccountHolderPortalLink("/account-holder/tuition-payment"));

                            await _outboxWriter.EnqueueEmailAsync(citizen.Email, template, token);
                        }
                    }

                    _chargeRepository.UpdateRange(charges);
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.Billing,
                        $"Marked {charges.Count} charge(s) overdue.",
                        cancellationToken: token);
                    await _unitOfWork.SaveChangeAsync(token);
                    return charges.Count;
                },
                cancellationToken);
        }

        private async Task<int> MarkOverdueInstallmentsAsync(
            DateTime utcNow,
            CancellationToken cancellationToken)
        {
            var utcToday = utcNow.Date;
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
                            && installment.DueDate.Date < utcToday)
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

                        var citizen = installment.Charge.Enrollment.SchoolStudent.EducationAccount.Citizen;
                        if (!string.IsNullOrWhiteSpace(citizen.Email))
                        {
                            var template = _emailTemplateBuilder.BuildPaymentOverdueEmail(
                                citizen.FullName,
                                installment.Charge.CourseNameSnapshot,
                                installment.Amount,
                                BuildAccountHolderPortalLink("/account-holder/tuition-payment"));

                            await _outboxWriter.EnqueueEmailAsync(citizen.Email, template, token);
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
                        generatedChargeCount = await GenerateMissingChargesAsync(course, utcNow, token);
                    }

                    var overdueCharges = targetStatus == CourseStatus.Closed
                        ? course.Enrollments
                            .Select(enrollment => enrollment.Charge)
                            .Where(charge => charge != null
                                && charge.RemainingAmount > 0
                                && charge.Status == ChargeStatus.PendingPayment
                                && PaymentDueWindow.IsOverdue(charge.CourseEndDateSnapshot, utcNow))
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
            DateTime utcNow,
            CancellationToken cancellationToken)
        {
            if (course.Status is CourseStatus.Draft or CourseStatus.Enrolling)
            {
                throw new DataConflictException(
                    "Charges can only be generated after enrollment has been finalized.");
            }

            var generatedCount = 0;
            var taxRate = await GetTaxRateAsync(cancellationToken);
            foreach (var enrollment in course.Enrollments)
            {
                if (enrollment.Charge != null
                    || enrollment.SchoolStudent.Status != SchoolStudentStatus.Active
                    || enrollment.SchoolStudent.EducationAccount.Status == EducationAccountStatus.Closed)
                {
                    continue;
                }

                var taxAmount = CourseFeeCalculator.CalculateTaxAmount(
                    course.CourseFeeAmount,
                    course.MiscFeeAmount,
                    taxRate);
                var grossAmount = course.CourseFeeAmount
                    + course.MiscFeeAmount
                    + taxAmount;
                var selectedFasApplication = await SelectBestFasApplicationAsync(
                    enrollment.SchoolStudentId,
                    course.Id,
                    course.CourseFeeAmount,
                    course.MiscFeeAmount,
                    taxRate,
                    grossAmount,
                    utcNow.Date,
                    cancellationToken);
                var subsidyResult = FasChargeSubsidyCalculator.Calculate(
                    course.CourseFeeAmount,
                    course.MiscFeeAmount,
                    taxRate,
                    grossAmount,
                    selectedFasApplication);

                var charge = new Charge
                {
                    EnrollmentId = enrollment.Id,
                    Status = subsidyResult.NetAmount == 0m ? ChargeStatus.Paid : ChargeStatus.PendingPayment,
                    SchoolNameSnapshot = course.School.SchoolName,
                    CourseCodeSnapshot = course.CourseCode,
                    CourseNameSnapshot = course.CourseName,
                    CourseDescriptionSnapshot = enrollment.CourseDescriptionSnapshot,
                    CourseStartDateSnapshot = course.StartDate,
                    CourseEndDateSnapshot = course.EndDate,
                    CourseFeeAmountSnapshot = course.CourseFeeAmount,
                    MiscFeeAmountSnapshot = course.MiscFeeAmount,
                    GstAmountSnapshot = taxAmount,
                    TaxRateSnapshot = taxRate,
                    GrossAmount = grossAmount,
                    SubsidyAmount = subsidyResult.SubsidyAmount,
                    NetAmount = subsidyResult.NetAmount,
                    PaidAmount = 0m,
                    RemainingAmount = subsidyResult.NetAmount,
                    AppliedFasApplicationId = selectedFasApplication?.Id,
                    AppliedFasSchemeNameSnapshot = selectedFasApplication?.FasScheme.SchemeName,
                    AppliedFasTierNameSnapshot = selectedFasApplication?.ApprovedTier?.TierName,
                    AppliedFasSubsidyTypeSnapshot = selectedFasApplication?.ApprovedTier?.SubsidyType,
                    AppliedFasIsPerComponentSnapshot = selectedFasApplication?.ApprovedTier?.IsPerComponent ?? false,
                    AppliedFasSubsidyValueSnapshot = selectedFasApplication?.ApprovedTier?.SubsidyValue,
                    AppliedFasCourseFeeSubsidyValueSnapshot = selectedFasApplication?.ApprovedTier?.CourseFeeSubsidyValue,
                    AppliedFasMiscFeeSubsidyValueSnapshot = selectedFasApplication?.ApprovedTier?.MiscFeeSubsidyValue
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

                var citizen = enrollment.SchoolStudent.EducationAccount.Citizen;
                if (!string.IsNullOrWhiteSpace(citizen.Email)
                    && charge.Status == ChargeStatus.PendingPayment
                    && charge.RemainingAmount > 0)
                {
                    var template = _emailTemplateBuilder.BuildCourseFeePayableEmail(
                        citizen.FullName,
                        course.CourseName,
                        charge.GrossAmount,
                        charge.SubsidyAmount,
                        charge.NetAmount,
                        charge.CourseEndDateSnapshot,
                        BuildAccountHolderPortalLink("/account-holder/tuition-payment"));

                    await _outboxWriter.EnqueueEmailAsync(citizen.Email, template, cancellationToken);
                }

                enrollment.Charge = charge;
                generatedCount++;
            }

            return generatedCount;
        }

        private async Task<FasApplication?> SelectBestFasApplicationAsync(
            int schoolStudentId,
            int courseId,
            decimal courseFee,
            decimal miscFee,
            decimal taxRate,
            decimal grossAmount,
            DateTime effectiveDate,
            CancellationToken cancellationToken)
        {
            var candidates = await _fasApplicationRepository.Query()
                .Include(application => application.FasScheme)
                    .ThenInclude(scheme => scheme.SchemeCourses)
                .Include(application => application.ApprovedTier)
                .Where(application =>
                    application.SchoolStudentId == schoolStudentId
                    && application.Status == FasApplicationStatus.Approved
                    && application.ApprovedTierId.HasValue
                    && application.ValidityStartDate.HasValue
                    && application.ValidityStartDate.Value.Date <= effectiveDate
                    && (!application.ValidityEndDate.HasValue || application.ValidityEndDate.Value.Date >= effectiveDate)
                    && application.FasScheme.SchemeCourses.Any(link => link.CourseId == courseId))
                .ToListAsync(cancellationToken);

            return candidates
                .Select(application => new
                {
                    Application = application,
                    SubsidyAmount = FasChargeSubsidyCalculator.CalculateSubsidyOnly(
                        courseFee,
                        miscFee,
                        taxRate,
                        grossAmount,
                        application)
                })
                .OrderByDescending(item => item.SubsidyAmount)
                .ThenBy(item => item.Application.ApprovedAt)
                .ThenBy(item => item.Application.Id)
                .Select(item => item.Application)
                .FirstOrDefault();
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

        private string BuildAccountHolderPortalLink(string path)
        {
            var frontendUrl = _configuration.UrlsConfig?.FrontendUrl?.Trim();
            if (string.IsNullOrWhiteSpace(frontendUrl))
            {
                return "#";
            }

            return $"{frontendUrl.TrimEnd('/')}{path}";
        }
    }
}
