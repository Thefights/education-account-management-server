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
            Status = StudentTuitionFilterStatus.Overdue,
            IsInstallment = true
        });

        var item = Assert.Single(result.Collection);
        Assert.Equal(ChargeStatus.Overdue, item.Status);
        Assert.Equal(course.EndDate, item.PaymentDueDate);
        Assert.Equal([1, 2, 3], item.Installments.Select(i => i.InstallmentNumber).ToList());
        Assert.Contains(item.Installments, installment => installment.Status == ChargeInstallmentStatus.Overdue);
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
