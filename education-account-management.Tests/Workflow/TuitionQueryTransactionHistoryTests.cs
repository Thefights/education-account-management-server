using DTOs.Payments;
using education_account_management.Tests.Fakes;
using education_account_management.Tests.Support;
using Enums;
using Filters.Courses;
using Filters.TransactionHistory;
using Mappers;
using Microsoft.EntityFrameworkCore;
using Services.Payments;
using Services.TransactionHistory;

namespace education_account_management.Tests.Workflow;

public class TuitionQueryTransactionHistoryTests
{
    [Fact]
    public async Task StudentTuitionService_ReturnsStatusFromChargeAndOrderedInstallments()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("3000001");
        var course = await db.CreateCourseAsync(graph, "TQ00001", status: CourseStatus.Upcoming);
        var enrollment = await db.CreateEnrollmentAsync(graph, course);
        var charge = await db.CreateChargeAsync(enrollment, status: ChargeStatus.Overdue, paymentPlanMonths: 3);
        await db.CreateInstallmentsAsync(
            charge,
            (2, 40m, ChargeInstallmentStatus.Overdue),
            (1, 40m, ChargeInstallmentStatus.Paid),
            (3, 40m, ChargeInstallmentStatus.PendingPayment));
        var service = new StudentTuitionService(db.CreateUnitOfWork(), new TestCurrentUserService(graph.UserId));

        var result = await service.GetTuitionChargesPaginatedAsync(new StudentTuitionFilterDTO
        {
            Statuses = [StudentTuitionFilterStatus.Overdue],
            IsInstallment = true
        });

        var item = Assert.Single(result.Collection);
        Assert.Equal(ChargeStatus.Overdue, item.Status);
        Assert.Equal(course.EndDate, item.PaymentDueDate);
        Assert.Equal([2, 3, 1], item.Installments.Select(i => i.InstallmentNumber).ToList());
        Assert.Contains(item.Installments, installment => installment.Status == ChargeInstallmentStatus.Overdue);
    }

    [Fact]
    public async Task StudentTuitionService_OrdersChargesByBusinessStatusBeforeSecondarySort()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("3000005");
        var paidCourse = await db.CreateCourseAsync(graph, "TQ00008", status: CourseStatus.Upcoming);
        var pendingCourse = await db.CreateCourseAsync(graph, "TQ00009", status: CourseStatus.Upcoming);
        var overdueCourse = await db.CreateCourseAsync(graph, "TQ00010", status: CourseStatus.Upcoming);
        var paidEnrollment = await db.CreateEnrollmentAsync(graph, paidCourse);
        var pendingEnrollment = await db.CreateEnrollmentAsync(graph, pendingCourse);
        var overdueEnrollment = await db.CreateEnrollmentAsync(graph, overdueCourse);
        await db.CreateChargeAsync(paidEnrollment, status: ChargeStatus.Paid);
        await db.CreateChargeAsync(pendingEnrollment, status: ChargeStatus.PendingPayment);
        await db.CreateChargeAsync(overdueEnrollment, status: ChargeStatus.Overdue);
        var service = new StudentTuitionService(db.CreateUnitOfWork(), new TestCurrentUserService(graph.UserId));

        var result = await service.GetTuitionChargesPaginatedAsync(new StudentTuitionFilterDTO
        {
            Sort = "createdAt asc",
            PageSize = 10
        });

        Assert.Equal(
            [ChargeStatus.Overdue, ChargeStatus.PendingPayment, ChargeStatus.Paid],
            result.Collection.Select(item => item.Status).ToList());
    }

    [Fact]
    public async Task StudentTuitionService_HidesInstallmentPlansWithOnlyFutureUnpaidInstallmentsFromList()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("3000006");
        var futureCourse = await db.CreateCourseAsync(graph, "TQ00011", status: CourseStatus.Upcoming);
        var dueCourse = await db.CreateCourseAsync(graph, "TQ00012", status: CourseStatus.Upcoming);
        var futureEnrollment = await db.CreateEnrollmentAsync(graph, futureCourse);
        var dueEnrollment = await db.CreateEnrollmentAsync(graph, dueCourse);
        var futureCharge = await db.CreateChargeAsync(futureEnrollment, status: ChargeStatus.PendingPayment, paymentPlanMonths: 2);
        var dueCharge = await db.CreateChargeAsync(dueEnrollment, status: ChargeStatus.PendingPayment, paymentPlanMonths: 2);
        await db.CreateInstallmentsAsync(
            futureCharge,
            (1, 60m, ChargeInstallmentStatus.Paid),
            (2, 60m, ChargeInstallmentStatus.PendingPayment));
        await db.CreateInstallmentsAsync(
            dueCharge,
            (1, 60m, ChargeInstallmentStatus.Paid),
            (2, 60m, ChargeInstallmentStatus.PendingPayment));
        var dueInstallment = await db.Context.ChargeInstallment
            .SingleAsync(installment => installment.ChargeId == dueCharge.Id && installment.InstallmentNumber == 2);
        dueInstallment.DueDate = DateTime.UtcNow.Date;
        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();
        var service = new StudentTuitionService(db.CreateUnitOfWork(), new TestCurrentUserService(graph.UserId));

        var result = await service.GetTuitionChargesPaginatedAsync(new StudentTuitionFilterDTO
        {
            IsInstallment = true,
            PageSize = 10
        });

        var item = Assert.Single(result.Collection);
        Assert.Equal(dueCharge.Id, item.ChargeId);
    }

    [Fact]
    public async Task StudentTuitionService_FiltersByMultipleStatuses()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("3000004");
        var course1 = await db.CreateCourseAsync(graph, "TQ00005", status: CourseStatus.Upcoming);
        var course2 = await db.CreateCourseAsync(graph, "TQ00006", status: CourseStatus.Upcoming);
        var course3 = await db.CreateCourseAsync(graph, "TQ00007", status: CourseStatus.Upcoming);
        var enrollment1 = await db.CreateEnrollmentAsync(graph, course1);
        var enrollment2 = await db.CreateEnrollmentAsync(graph, course2);
        var enrollment3 = await db.CreateEnrollmentAsync(graph, course3);
        await db.CreateChargeAsync(enrollment1, status: ChargeStatus.PendingPayment);
        await db.CreateChargeAsync(enrollment2, status: ChargeStatus.Overdue);
        await db.CreateChargeAsync(enrollment3, status: ChargeStatus.Paid);
        var service = new StudentTuitionService(db.CreateUnitOfWork(), new TestCurrentUserService(graph.UserId));

        var result = await service.GetTuitionChargesPaginatedAsync(new StudentTuitionFilterDTO
        {
            Statuses = [StudentTuitionFilterStatus.Due, StudentTuitionFilterStatus.Overdue]
        });

        Assert.Equal(2, result.Collection.Count);
        Assert.Contains(result.Collection, item => item.Status == ChargeStatus.PendingPayment);
        Assert.Contains(result.Collection, item => item.Status == ChargeStatus.Overdue);
        Assert.DoesNotContain(result.Collection, item => item.Status == ChargeStatus.Paid);
    }

    [Fact]
    public async Task StudentTuitionSummary_CountsOnlyUnpaidChargesWithRemainingAmount()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("3000002");
        var course1 = await db.CreateCourseAsync(graph, "TQ00002", status: CourseStatus.Upcoming);
        var course2 = await db.CreateCourseAsync(graph, "TQ00004", status: CourseStatus.Upcoming);
        var enrollment1 = await db.CreateEnrollmentAsync(graph, course1);
        var enrollment2 = await db.CreateEnrollmentAsync(graph, course2);
        await db.CreateChargeAsync(enrollment1, netAmount: 120m, status: ChargeStatus.PendingPayment);
        await db.CreateChargeAsync(enrollment2, netAmount: 80m, status: ChargeStatus.Paid);
        var paidCharge = await db.Context.Charge.SingleAsync(c => c.EnrollmentId == enrollment2.Id);
        paidCharge.PaidAmount = 80m;
        paidCharge.RemainingAmount = 0m;
        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();
        var service = new StudentTuitionService(db.CreateUnitOfWork(), new TestCurrentUserService(graph.UserId));

        var summary = await service.GetTuitionSummaryAsync();

        Assert.Equal(120m, summary.TotalOutstandingAmount);
        Assert.Equal(1, summary.PendingPaymentInvoicesCount);
    }

    [Fact]
    public async Task TransactionHistory_ReturnsPaymentMethodForBalanceAndOnlineTransactions()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("3000003", balance: 50m);
        var course = await db.CreateCourseAsync(graph, "TQ00003", status: CourseStatus.Upcoming);
        var enrollment = await db.CreateEnrollmentAsync(graph, course);
        var charge = await db.CreateChargeAsync(enrollment);
        var stripe = new FakeStripeCheckoutGateway();
        var paymentService = db.CreateStripeService(graph.UserId, stripe);
        await paymentService.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 50m
        });
        var sessionId = await db.Context.Payment
            .Where(p => p.PaymentMethod == PaymentMethod.OnlinePayment)
            .Select(p => p.ExternalReference!)
            .SingleAsync();
        stripe.MarkPaid(sessionId);
        await paymentService.HandleSessionSuccessAsync(sessionId);
        db.Context.ChangeTracker.Clear();
        var service = new TransactionHistoryService(
            db.CreateUnitOfWork(),
            new TransactionHistoryMapper(),
            new TestCurrentUserService(graph.UserId));

        var result = await service.GetForCurrentAccountHolderAsync(new EducationCreditTransactionFilterDTO());

        Assert.Equal(2, result.Collection.Count);
        Assert.Contains(result.Collection, item => item.PaymentMethod == PaymentMethod.EducationBalance);
        Assert.Contains(result.Collection, item => item.PaymentMethod == PaymentMethod.OnlinePayment);
    }
}
