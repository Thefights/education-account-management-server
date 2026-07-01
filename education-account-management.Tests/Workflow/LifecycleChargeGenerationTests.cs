using education_account_management.Tests.Support;
using Enums;
using Microsoft.EntityFrameworkCore;

namespace education_account_management.Tests.Workflow;

public class LifecycleChargeGenerationTests
{
    [Fact]
    public async Task ProcessDateTransitions_GeneratesChargesForActiveAndWithdrawnEnrollments()
    {
        await using var db = new TestDatabase();
        var active = await db.CreateAccountGraphAsync("1000001");
        var withdrawn = await db.CreateAccountGraphAsync("1000002");
        var inactive = await db.CreateAccountGraphAsync("1000003", studentStatus: SchoolStudentStatus.Inactive);
        var closed = await db.CreateAccountGraphAsync("1000004", accountStatus: EducationAccountStatus.Closed);
        var course = await db.CreateCourseAsync(active, "LC00001");
        await db.CreateEnrollmentAsync(active, course);
        await db.CreateEnrollmentAsync(withdrawn, course, EnrollmentStatus.Withdrawn);
        await db.CreateEnrollmentAsync(inactive, course);
        await db.CreateEnrollmentAsync(closed, course);

        var service = db.CreateCourseLifecycleService();

        await service.ProcessDateTransitionsAsync(DateTime.UtcNow);

        var enrollments = await db.Context.Enrollment
            .Include(e => e.Charge)
            .OrderBy(e => e.Id)
            .ToListAsync();

        Assert.Equal(CourseStatus.Upcoming, await db.Context.Course.Where(c => c.Id == course.Id).Select(c => c.Status).SingleAsync());
        Assert.NotNull(enrollments[0].Charge);
        Assert.NotNull(enrollments[1].Charge);
        Assert.Null(enrollments[2].Charge);
        Assert.Null(enrollments[3].Charge);
        Assert.All(enrollments.Take(2), enrollment =>
        {
            Assert.Equal(ChargeStatus.PendingPayment, enrollment.Charge!.Status);
            Assert.Null(enrollment.Charge.PaymentPlanMonths);
            Assert.Empty(enrollment.Charge.Installments);
            Assert.Equal(enrollment.CourseNameSnapshot, enrollment.Charge.CourseNameSnapshot);
            Assert.Equal(enrollment.CourseDescriptionSnapshot, enrollment.Charge.CourseDescriptionSnapshot);
            Assert.Equal(120m, enrollment.Charge.GrossAmount);
            Assert.Equal(120m, enrollment.Charge.NetAmount);
            Assert.Equal(120m, enrollment.Charge.RemainingAmount);
        });
    }

    [Fact]
    public async Task ProcessDateTransitions_WhenCourseCloses_MarksUnpaidChargesOverdue()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("1000011");
        var course = await db.CreateCourseAsync(
            graph,
            "LC00011",
            status: CourseStatus.InProgress,
            enrollmentDeadline: DateTime.UtcNow.AddDays(-20),
            startDate: DateTime.UtcNow.AddDays(-10),
            endDate: DateTime.UtcNow.AddDays(-1));
        var enrollment = await db.CreateEnrollmentAsync(graph, course);
        var charge = await db.CreateChargeAsync(enrollment);

        var service = db.CreateCourseLifecycleService();

        await service.ProcessDateTransitionsAsync(DateTime.UtcNow);

        var reloadedCourse = await db.Context.Course.FindAsync(course.Id);
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        Assert.Equal(CourseStatus.Closed, reloadedCourse!.Status);
        Assert.Equal(ChargeStatus.Overdue, reloadedCharge!.Status);
    }

    [Fact]
    public async Task ProcessDateTransitions_MarksPendingInstallmentsOverdue_AndChargeOverdue()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("1000012");
        var course = await db.CreateCourseAsync(graph, "LC00012", status: CourseStatus.Upcoming);
        var enrollment = await db.CreateEnrollmentAsync(graph, course);
        var charge = await db.CreateChargeAsync(enrollment, paymentPlanMonths: 3);
        var installments = await db.CreateInstallmentsAsync(
            charge,
            (1, 40m, ChargeInstallmentStatus.Paid),
            (2, 40m, ChargeInstallmentStatus.PendingPayment));
        var pending = installments.Single(i => i.InstallmentNumber == 2);
        pending.DueDate = DateTime.UtcNow.AddDays(-1);
        db.Context.ChargeInstallment.Update(pending);
        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();

        var service = db.CreateCourseLifecycleService();

        await service.ProcessDateTransitionsAsync(DateTime.UtcNow);

        var reloadedInstallment = await db.Context.ChargeInstallment.FindAsync(pending.Id);
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        Assert.Equal(ChargeInstallmentStatus.Overdue, reloadedInstallment!.Status);
        Assert.Equal(ChargeStatus.Overdue, reloadedCharge!.Status);
    }
}
