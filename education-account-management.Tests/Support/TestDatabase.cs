using education_account_management.BLL;
using education_account_management.Tests.Fakes;
using Enums;
using Interfaces.Audit;
using Interfaces.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Persistence.SqlServer;
using Services.Courses;
using Services.Payments;
using Utils;

namespace Support;

internal sealed class TestDatabase : IAsyncDisposable
{
    public TestDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        Context = new ApplicationDbContext(options, new TestAuditUserContext(1));
    }

    public ApplicationDbContext Context { get; }

    public UnitOfWork CreateUnitOfWork()
    {
        return new UnitOfWork(Context, NullLogger<UnitOfWork>.Instance);
    }

    public CourseLifecycleService CreateCourseLifecycleService(IAuditLogWriter? auditLogWriter = null)
    {
        return new CourseLifecycleService(CreateUnitOfWork(), auditLogWriter ?? new NoopAuditLogWriter());
    }

    public StripeService CreateStripeService(
        int currentUserId,
        FakeStripeCheckoutGateway stripeGateway,
        IOutboxWriter? outboxWriter = null,
        IAuditLogWriter? auditLogWriter = null)
    {
        return new StripeService(
            CreateAppConfiguration(),
            CreateUnitOfWork(),
            outboxWriter ?? new NoopOutboxWriter(),
            new TestCurrentUserService(currentUserId),
            auditLogWriter ?? new NoopAuditLogWriter(),
            stripeGateway);
    }

    public static AppConfiguration CreateAppConfiguration()
    {
        return new AppConfiguration
        {
            StripeConfig = new StripeConfig
            {
                SecretKey = "sk_test_fake",
                WebhookSecret = "whsec_fake",
                SuccessUrl = "https://app.test/success",
                CancelUrl = "https://app.test/cancel",
                Method = "card",
                Mode = "payment",
                SessionExpiryMinutes = 30
            }
        };
    }

    public async Task<AccountGraph> CreateAccountGraphAsync(
        string suffix,
        decimal balance = 0m,
        EducationAccountStatus accountStatus = EducationAccountStatus.Active,
        SchoolStudentStatus studentStatus = SchoolStudentStatus.Active)
    {
        var serialNumber = int.Parse(suffix);
        var citizen = new Citizen
        {
            Nric = SingaporeNricUtil.Generate(serialNumber),
            FullName = $"Student {suffix}",
            Email = $"student{suffix}@example.com",
            PhoneNumber = $"+659{suffix.PadLeft(7, '0')[..7]}",
            DateOfBirth = new DateOnly(2010, 1, 1),
            IsSingaporean = true
        };
        var user = new User
        {
            Role = UserRole.AccountHolder,
            Status = UserStatus.Active,
            Citizen = citizen
        };
        var account = new EducationAccount
        {
            AccountNumber = $"ACC-{suffix}",
            EducationCreditBalance = balance,
            Status = accountStatus,
            Citizen = citizen
        };
        var school = new School
        {
            SchoolName = $"School {suffix}",
            Address = $"Address {suffix}",
            PhoneNumber = $"+658{suffix.PadLeft(7, '0')[..7]}",
            Email = $"school{suffix}@example.com"
        };
        var student = new SchoolStudent
        {
            School = school,
            EducationAccount = account,
            Status = studentStatus
        };

        Context.AddRange(citizen, user, account, school, student);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
        return new AccountGraph(citizen.Id, user.Id, account.Id, school.Id, student.Id);
    }

    public async Task<Course> CreateCourseAsync(
        AccountGraph graph,
        string suffix,
        CourseStatus status = CourseStatus.Enrolling,
        DateTime? enrollmentDeadline = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal courseFee = 100m,
        decimal miscFee = 20m)
    {
        var deadline = enrollmentDeadline ?? DateTime.UtcNow.AddDays(-1);
        var start = startDate ?? DateTime.UtcNow.AddDays(5);
        var end = endDate ?? DateTime.UtcNow.AddDays(30);
        var course = new Course
        {
            SchoolId = graph.SchoolId,
            Status = status,
            CourseCode = $"CRS-2026-{suffix.PadLeft(7, 'A')[..7]}",
            CourseName = $"Course {suffix}",
            CourseFeeAmount = courseFee,
            MiscFeeAmount = miscFee,
            EnrollmentDeadline = deadline,
            StartDate = start,
            EndDate = end
        };
        Context.Course.Add(course);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
        return course;
    }

    public async Task<Enrollment> CreateEnrollmentAsync(
        AccountGraph graph,
        Course course,
        EnrollmentStatus status = EnrollmentStatus.Active)
    {
        var account = await Context.EducationAccount
            .Include(a => a.Citizen)
            .FirstAsync(a => a.Id == graph.AccountId);
        var school = await Context.School.FirstAsync(s => s.Id == graph.SchoolId);
        var enrollment = new Enrollment
        {
            CourseId = course.Id,
            SchoolStudentId = graph.SchoolStudentId,
            Status = status,
            SchoolNameSnapshot = school.SchoolName,
            CourseNameSnapshot = course.CourseName,
            CourseDescriptionSnapshot = $"Description {course.Id}",
            CitizenNricSnapshot = account.Citizen.Nric,
            CitizenFullNameSnapshot = account.Citizen.FullName,
            CitizenEmailSnapshot = account.Citizen.Email,
            CitizenPhoneNumberSnapshot = account.Citizen.PhoneNumber,
            AccountNumberSnapshot = account.AccountNumber
        };
        Context.Enrollment.Add(enrollment);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
        return enrollment;
    }

    public async Task<Charge> CreateChargeAsync(
        Enrollment enrollment,
        decimal netAmount = 120m,
        ChargeStatus status = ChargeStatus.PendingPayment,
        int? paymentPlanMonths = null)
    {
        var reloadedEnrollment = await Context.Enrollment
            .Include(e => e.Course)
            .FirstAsync(e => e.Id == enrollment.Id);
        var charge = new Charge
        {
            EnrollmentId = enrollment.Id,
            Status = status,
            PaymentPlanMonths = paymentPlanMonths,
            SchoolNameSnapshot = enrollment.SchoolNameSnapshot,
            CourseCodeSnapshot = reloadedEnrollment.Course.CourseCode,
            CourseNameSnapshot = enrollment.CourseNameSnapshot,
            CourseDescriptionSnapshot = enrollment.CourseDescriptionSnapshot,
            CourseStartDateSnapshot = reloadedEnrollment.Course.StartDate,
            CourseEndDateSnapshot = reloadedEnrollment.Course.EndDate,
            CourseFeeAmountSnapshot = 100m,
            MiscFeeAmountSnapshot = 20m,
            GstAmountSnapshot = 0m,
            TaxRateSnapshot = 0m,
            GrossAmount = netAmount,
            SubsidyAmount = 0m,
            NetAmount = netAmount,
            PaidAmount = status == ChargeStatus.Paid ? netAmount : 0m,
            RemainingAmount = status == ChargeStatus.Paid ? 0m : netAmount
        };
        Context.Charge.Add(charge);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
        return charge;
    }

    public async Task<List<ChargeInstallment>> CreateInstallmentsAsync(Charge charge, params (int Number, decimal Amount, ChargeInstallmentStatus Status)[] installments)
    {
        var entities = installments.Select(item => new ChargeInstallment
        {
            ChargeId = charge.Id,
            InstallmentNumber = item.Number,
            Amount = item.Amount,
            DueDate = DateTime.UtcNow.AddMonths(item.Number - 1),
            Status = item.Status
        }).ToList();
        Context.ChargeInstallment.AddRange(entities);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
        return entities;
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
    }
}

internal sealed record AccountGraph(
    int CitizenId,
    int UserId,
    int AccountId,
    int SchoolId,
    int SchoolStudentId);
