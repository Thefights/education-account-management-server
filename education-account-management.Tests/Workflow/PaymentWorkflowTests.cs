using DTOs.Payments;
using education_account_management.Tests.Fakes;
using education_account_management.Tests.Support;
using Enums;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace education_account_management.Tests.Workflow;

public class PaymentWorkflowTests
{
    [Fact]
    public async Task PayFullCharges_BalanceOnly_FinalizesChargeAndCreatesDecreasedTransaction()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000001", balance: 120m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PF00001");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        var response = await service.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 120m
        });

        db.Context.ChangeTracker.Clear();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        var account = await db.Context.EducationAccount.FindAsync(graph.AccountId);
        var payment = await db.Context.Payment.Include(p => p.PaymentAllocations).SingleAsync();
        var transaction = await db.Context.EducationCreditTransaction.Include(t => t.Payment).SingleAsync();

        Assert.Equal(PaymentStatus.Succeeded.ToString(), response.Status);
        Assert.Equal("WalletOnly", response.PaymentMode);
        Assert.False(response.RequiresRedirect);
        Assert.True(response.IsWalletOnly);
        Assert.Equal(120m, response.TotalAmount);
        Assert.Equal(120m, response.WalletAmount);
        Assert.Equal(0m, response.OnlineAmount);
        Assert.Single(response.PaymentIds);
        Assert.Equal(0, stripe.CreateCallCount);
        Assert.Equal(ChargeStatus.Paid, reloadedCharge!.Status);
        Assert.Equal(120m, reloadedCharge.PaidAmount);
        Assert.Equal(0m, reloadedCharge.RemainingAmount);
        Assert.Equal(0m, account!.EducationCreditBalance);
        Assert.Equal(PaymentMethod.EducationBalance, payment.PaymentMethod);
        Assert.Single(payment.PaymentAllocations);
        Assert.Null(payment.PaymentAllocations.Single().ChargeInstallmentId);
        Assert.Equal(EducationCreditTransactionDirection.Decreased, transaction.Direction);
        Assert.Equal(PaymentMethod.EducationBalance, transaction.Payment!.PaymentMethod);
    }

    [Fact]
    public async Task PayFullCharges_OnlineOnly_CreatesPendingPaymentAndDoesNotFinalizeBeforeSuccess()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000002");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PF00002");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        var response = await service.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 0m
        });

        db.Context.ChangeTracker.Clear();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        var payment = await db.Context.Payment.Include(p => p.PaymentAllocations).SingleAsync();

        Assert.Equal(PaymentStatus.Pending.ToString(), response.Status);
        Assert.Equal("OnlineOnly", response.PaymentMode);
        Assert.True(response.RequiresRedirect);
        Assert.False(response.IsWalletOnly);
        Assert.Equal(120m, response.TotalAmount);
        Assert.Equal(0m, response.WalletAmount);
        Assert.Equal(120m, response.OnlineAmount);
        Assert.Single(response.PaymentIds);
        Assert.Equal(1, stripe.CreateCallCount);
        Assert.NotEmpty(response.Link);
        Assert.Equal(ChargeStatus.PendingPayment, reloadedCharge!.Status);
        Assert.Equal(0m, reloadedCharge.PaidAmount);
        Assert.Equal(120m, reloadedCharge.RemainingAmount);
        Assert.Equal(PaymentMethod.OnlinePayment, payment.PaymentMethod);
        Assert.Single(payment.PaymentAllocations);
        Assert.Null(payment.PaymentAllocations.Single().ChargeInstallmentId);
    }

    [Fact]
    public async Task PayFullCharges_MixedPayment_OnSuccessCreatesTwoPaymentsAndTwoTransactions()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000003", balance: 50m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PF00003");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        var checkoutResponse = await service.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 50m
        });
        var sessionId = await db.Context.Payment
            .Where(p => p.PaymentMethod == PaymentMethod.OnlinePayment)
            .Select(p => p.ExternalReference!)
            .SingleAsync();
        stripe.MarkPaid(sessionId);

        var successResponse = await service.HandleSessionSuccessAsync(sessionId);

        db.Context.ChangeTracker.Clear();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        var account = await db.Context.EducationAccount.FindAsync(graph.AccountId);
        var payments = await db.Context.Payment.OrderBy(p => p.PaymentMethod).ToListAsync();
        var transactions = await db.Context.EducationCreditTransaction
            .OrderBy(t => t.Direction)
            .ToListAsync();

        Assert.Equal(ChargeStatus.Paid, reloadedCharge!.Status);
        Assert.Equal("Mixed", checkoutResponse.PaymentMode);
        Assert.True(checkoutResponse.RequiresRedirect);
        Assert.Equal(120m, checkoutResponse.TotalAmount);
        Assert.Equal(50m, checkoutResponse.WalletAmount);
        Assert.Equal(70m, checkoutResponse.OnlineAmount);
        Assert.Equal("Mixed", successResponse.PaymentMode);
        Assert.False(successResponse.RequiresRedirect);
        Assert.Equal(PaymentStatus.Succeeded.ToString(), successResponse.Status);
        Assert.Equal(0m, account!.EducationCreditBalance);
        Assert.Equal(2, payments.Count);
        Assert.All(payments, payment => Assert.Equal(PaymentStatus.Succeeded, payment.Status));
        Assert.Contains(transactions, transaction => transaction.Direction == EducationCreditTransactionDirection.Decreased && transaction.Amount == 50m);
        Assert.Contains(transactions, transaction => transaction.Direction == EducationCreditTransactionDirection.Unchanged && transaction.Amount == 70m);
    }

    [Fact]
    public async Task CreateInstallmentPlans_BalanceOnly_CreatesPlanAndMarksFirstInstallmentPaid()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000004", balance: 40m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "IP00004");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.CreateInstallmentPlansAsync(new CreateInstallmentPlansRequest
        {
            Items = [new CreateInstallmentPlanItemRequest { ChargeId = charge.Id, PaymentPlanMonths = 3 }],
            CreditBalanceApplied = 40m
        });

        db.Context.ChangeTracker.Clear();
        var reloadedCharge = await db.Context.Charge
            .Include(c => c.Installments)
            .Include(c => c.PaymentAllocations)
            .SingleAsync(c => c.Id == charge.Id);

        Assert.Equal(3, reloadedCharge.PaymentPlanMonths);
        Assert.Equal(40m, reloadedCharge.PaidAmount);
        Assert.Equal(80m, reloadedCharge.RemainingAmount);
        Assert.Equal(3, reloadedCharge.Installments.Count);
        Assert.Equal(ChargeInstallmentStatus.Paid, reloadedCharge.Installments.Single(i => i.InstallmentNumber == 1).Status);
        Assert.All(reloadedCharge.Installments.Where(i => i.InstallmentNumber > 1), installment =>
            Assert.Equal(ChargeInstallmentStatus.PendingPayment, installment.Status));
        Assert.Equal(reloadedCharge.Installments.Single(i => i.InstallmentNumber == 1).Id, reloadedCharge.PaymentAllocations.Single().ChargeInstallmentId);
    }

    [Fact]
    public async Task CreateInstallmentPlans_UsesApplicationSettingInstallmentDueDay()
    {
        await using var db = new TestDatabase();
        db.Context.ApplicationSetting.Add(new ApplicationSetting
        {
            Id = 1,
            InstallmentDueDay = 12,
            TaxRate = 0.09m
        });
        await db.Context.SaveChangesAsync();

        var graph = await db.CreateAccountGraphAsync("2000010", balance: 40m);
        var courseEndDate = new DateTime(2026, 8, 20, 0, 0, 0, DateTimeKind.Utc);
        var course = await db.CreateCourseAsync(graph, "IP00010", status: CourseStatus.Upcoming, endDate: courseEndDate);
        var enrollment = await db.CreateEnrollmentAsync(graph, course);
        var charge = await db.CreateChargeAsync(enrollment);
        var service = db.CreateStripeService(graph.UserId, new FakeStripeCheckoutGateway());

        await service.CreateInstallmentPlansAsync(new CreateInstallmentPlansRequest
        {
            Items = [new CreateInstallmentPlanItemRequest { ChargeId = charge.Id, PaymentPlanMonths = 3 }],
            CreditBalanceApplied = 40m
        });

        db.Context.ChangeTracker.Clear();
        var dueDates = await db.Context.ChargeInstallment
            .Where(installment => installment.ChargeId == charge.Id)
            .OrderBy(installment => installment.InstallmentNumber)
            .Select(installment => installment.DueDate)
            .ToListAsync();

        Assert.Equal(
            [
                new DateTime(2026, 9, 12, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 10, 12, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc)
            ],
            dueDates);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task PayDueInstallments_BalanceOnly_MarksRequestedOldestInstallmentsPaid(int installmentCount)
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000005", balance: 90m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PN00005");
        await SetChargeProgressAsync(db, charge.Id, paid: 30m, remaining: 90m, planMonths: 6);
        await db.CreateInstallmentsAsync(
            charge,
            (1, 30m, ChargeInstallmentStatus.Paid),
            (2, 30m, ChargeInstallmentStatus.Overdue),
            (3, 30m, ChargeInstallmentStatus.Overdue),
            (4, 30m, ChargeInstallmentStatus.Overdue));
        await SetUnpaidInstallmentsDueAsync(db, charge.Id);
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.PayDueInstallmentsAsync(new PayDueInstallmentsRequest
        {
            Items = [new PayDueInstallmentsItemRequest { ChargeId = charge.Id, InstallmentCount = installmentCount }],
            CreditBalanceApplied = installmentCount * 30m
        });

        db.Context.ChangeTracker.Clear();
        var installments = await db.Context.ChargeInstallment
            .Where(i => i.ChargeId == charge.Id)
            .OrderBy(i => i.InstallmentNumber)
            .ToListAsync();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);

        Assert.All(
            installments.Take(installmentCount + 1),
            installment => Assert.Equal(ChargeInstallmentStatus.Paid, installment.Status));
        Assert.All(
            installments.Skip(installmentCount + 1),
            installment => Assert.Equal(ChargeInstallmentStatus.Overdue, installment.Status));
        Assert.Equal(30m + installmentCount * 30m, reloadedCharge!.PaidAmount);
        Assert.Equal(90m - installmentCount * 30m, reloadedCharge.RemainingAmount);
    }

    [Fact]
    public async Task PayDueInstallments_FutureOnlyInstallment_IsRejected()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000018", balance: 40m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PN00018");
        await SetChargeProgressAsync(db, charge.Id, paid: 40m, remaining: 80m, planMonths: 3);
        await db.CreateInstallmentsAsync(
            charge,
            (1, 40m, ChargeInstallmentStatus.Paid),
            (2, 40m, ChargeInstallmentStatus.PendingPayment));
        var service = db.CreateStripeService(graph.UserId, new FakeStripeCheckoutGateway());

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(() =>
            service.PayDueInstallmentsAsync(new PayDueInstallmentsRequest
            {
                Items = [new PayDueInstallmentsItemRequest { ChargeId = charge.Id, InstallmentCount = 1 }],
                CreditBalanceApplied = 40m
            }));

        Assert.Contains($"Charge_{charge.Id}_NoInstallmentDue", exception.FieldErrors.Keys);
    }

    [Fact]
    public async Task PayDueInstallments_CountExceedsUnlockedInstallments_IsRejected()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000020", balance: 80m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PD00020");
        await SetChargeProgressAsync(db, charge.Id, paid: 40m, remaining: 80m, planMonths: 3);
        await db.CreateInstallmentsAsync(
            charge,
            (1, 40m, ChargeInstallmentStatus.Paid),
            (2, 40m, ChargeInstallmentStatus.Overdue),
            (3, 40m, ChargeInstallmentStatus.PendingPayment));
        await SetUnpaidInstallmentsDueAsync(db, charge.Id);
        var service = db.CreateStripeService(graph.UserId, new FakeStripeCheckoutGateway());

        var exception = await Assert.ThrowsAsync<ValidationFailureException>(() =>
            service.PayDueInstallmentsAsync(new PayDueInstallmentsRequest
            {
                Items = [new PayDueInstallmentsItemRequest { ChargeId = charge.Id, InstallmentCount = 3 }],
                CreditBalanceApplied = 80m
            }));

        Assert.Contains($"Charge_{charge.Id}_InstallmentCount", exception.FieldErrors.Keys);
    }

    [Fact]
    public async Task PayDueInstallments_BatchCreatesAllocationsForEachRequestedCount()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000021");
        var (_, firstCharge) = await CreateChargeForAccountAsync(db, graph, "PB00021");
        var (_, secondCharge) = await CreateChargeForAccountAsync(db, graph, "PB00022");
        await SetChargeProgressAsync(db, firstCharge.Id, paid: 40m, remaining: 80m, planMonths: 3);
        await SetChargeProgressAsync(db, secondCharge.Id, paid: 40m, remaining: 80m, planMonths: 3);
        await db.CreateInstallmentsAsync(
            firstCharge,
            (1, 40m, ChargeInstallmentStatus.Overdue),
            (2, 40m, ChargeInstallmentStatus.Overdue));
        await db.CreateInstallmentsAsync(
            secondCharge,
            (1, 40m, ChargeInstallmentStatus.Overdue),
            (2, 40m, ChargeInstallmentStatus.Overdue));
        await SetUnpaidInstallmentsDueAsync(db, firstCharge.Id, secondCharge.Id);
        var service = db.CreateStripeService(graph.UserId, new FakeStripeCheckoutGateway());

        await service.PayDueInstallmentsAsync(new PayDueInstallmentsRequest
        {
            Items =
            [
                new PayDueInstallmentsItemRequest { ChargeId = firstCharge.Id, InstallmentCount = 1 },
                new PayDueInstallmentsItemRequest { ChargeId = secondCharge.Id, InstallmentCount = 2 }
            ]
        });

        var allocations = await db.Context.PaymentAllocation
            .Where(allocation => allocation.Payment.Status == PaymentStatus.Pending)
            .ToListAsync();

        Assert.Single(allocations, allocation => allocation.ChargeId == firstCharge.Id);
        Assert.Equal(2, allocations.Count(allocation => allocation.ChargeId == secondCharge.Id));
        Assert.All(allocations, allocation => Assert.NotNull(allocation.ChargeInstallmentId));
    }

    [Fact]
    public async Task PayDueInstallments_PendingSessionMatchesInstallmentCount()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000022");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PM00022");
        await SetChargeProgressAsync(db, charge.Id, paid: 40m, remaining: 80m, planMonths: 3);
        await db.CreateInstallmentsAsync(
            charge,
            (1, 40m, ChargeInstallmentStatus.Overdue),
            (2, 40m, ChargeInstallmentStatus.Overdue));
        await SetUnpaidInstallmentsDueAsync(db, charge.Id);
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);
        var request = new PayDueInstallmentsRequest
        {
            Items = [new PayDueInstallmentsItemRequest { ChargeId = charge.Id, InstallmentCount = 2 }]
        };

        var first = await service.PayDueInstallmentsAsync(request);
        db.Context.ChangeTracker.Clear();
        var resumed = await service.PayDueInstallmentsAsync(request);

        Assert.Equal(first.Link, resumed.Link);
        Assert.Equal(1, stripe.CreateCallCount);

        await Assert.ThrowsAsync<ValidationFailureException>(() =>
            service.PayDueInstallmentsAsync(new PayDueInstallmentsRequest
            {
                Items = [new PayDueInstallmentsItemRequest { ChargeId = charge.Id, InstallmentCount = 1 }]
            }));
        Assert.Equal(1, stripe.CreateCallCount);
    }

    [Fact]
    public async Task PayRemainingInstallments_AllowsFutureInstallments()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000019", balance: 80m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PR00019");
        await SetChargeProgressAsync(db, charge.Id, paid: 40m, remaining: 80m, planMonths: 3);
        await db.CreateInstallmentsAsync(
            charge,
            (1, 40m, ChargeInstallmentStatus.Paid),
            (2, 40m, ChargeInstallmentStatus.PendingPayment),
            (3, 40m, ChargeInstallmentStatus.PendingPayment));
        var service = db.CreateStripeService(graph.UserId, new FakeStripeCheckoutGateway());

        await service.PayRemainingInstallmentsAsync(new PayRemainingInstallmentsRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 80m
        });

        db.Context.ChangeTracker.Clear();
        var installments = await db.Context.ChargeInstallment
            .Where(i => i.ChargeId == charge.Id)
            .ToListAsync();

        Assert.All(installments, installment => Assert.Equal(ChargeInstallmentStatus.Paid, installment.Status));
    }

    [Fact]
    public async Task PayRemainingInstallments_BalanceOnly_MarksRemainingInstallmentsPaidAndChargePaid()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000006", balance: 80m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PR00006");
        await SetChargeProgressAsync(db, charge.Id, paid: 40m, remaining: 80m, planMonths: 3);
        await db.CreateInstallmentsAsync(
            charge,
            (1, 40m, ChargeInstallmentStatus.Paid),
            (2, 40m, ChargeInstallmentStatus.Overdue),
            (3, 40m, ChargeInstallmentStatus.PendingPayment));
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.PayRemainingInstallmentsAsync(new PayRemainingInstallmentsRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 80m
        });

        db.Context.ChangeTracker.Clear();
        var installments = await db.Context.ChargeInstallment
            .Where(i => i.ChargeId == charge.Id)
            .ToListAsync();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);

        Assert.All(installments, installment => Assert.Equal(ChargeInstallmentStatus.Paid, installment.Status));
        Assert.Equal(ChargeStatus.Paid, reloadedCharge!.Status);
        Assert.Equal(120m, reloadedCharge.PaidAmount);
        Assert.Equal(0m, reloadedCharge.RemainingAmount);
    }

    [Fact]
    public async Task PayFullCharges_DuplicateChargeIds_AreRejected()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000007", balance: 120m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PD00007");
        var service = db.CreateStripeService(graph.UserId, new FakeStripeCheckoutGateway());

        await Assert.ThrowsAsync<ValidationFailureException>(() =>
            service.PayFullChargesAsync(new PayFullChargesRequest
            {
                ChargeIds = [charge.Id, charge.Id],
                CreditBalanceApplied = 120m
            }));
    }

    [Fact]
    public async Task HandleSessionCancelled_DoesNotMutateChargeOrBalance()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000008", balance: 50m);
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PC00008");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 50m
        });
        var sessionId = await db.Context.Payment
            .Where(p => p.PaymentMethod == PaymentMethod.OnlinePayment)
            .Select(p => p.ExternalReference!)
            .SingleAsync();

        var cancelResponse = await service.HandleSessionCancelledAsync(sessionId);

        db.Context.ChangeTracker.Clear();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        var account = await db.Context.EducationAccount.FindAsync(graph.AccountId);
        var payments = await db.Context.Payment.ToListAsync();

        Assert.Equal(ChargeStatus.PendingPayment, reloadedCharge!.Status);
        Assert.Equal("Mixed", cancelResponse.PaymentMode);
        Assert.False(cancelResponse.RequiresRedirect);
        Assert.Equal(PaymentStatus.Canceled.ToString(), cancelResponse.Status);
        Assert.Equal(120m, cancelResponse.TotalAmount);
        Assert.Equal(50m, cancelResponse.WalletAmount);
        Assert.Equal(70m, cancelResponse.OnlineAmount);
        Assert.Equal(0m, reloadedCharge.PaidAmount);
        Assert.Equal(120m, reloadedCharge.RemainingAmount);
        Assert.Equal(50m, account!.EducationCreditBalance);
        Assert.All(payments, payment => Assert.Equal(PaymentStatus.Canceled, payment.Status));
        Assert.Empty(await db.Context.EducationCreditTransaction.ToListAsync());
        Assert.Equal(1, stripe.ExpireCallCount);
    }

    [Fact]
    public async Task PayFullCharges_DoesNotCreateZeroAmountStripeLineItems()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000009", balance: 120m);
        var (_, coveredCharge) = await CreateChargeForAccountAsync(db, graph, "PZ00009");
        var (_, onlineCharge) = await CreateChargeForAccountAsync(db, graph, "PZ00010");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [coveredCharge.Id, onlineCharge.Id],
            CreditBalanceApplied = 120m
        });

        Assert.Equal(1, stripe.CreateCallCount);
        Assert.Single(stripe.LastCreateOptions!.LineItems);
        Assert.Equal(12000, stripe.LastCreateOptions.LineItems.Single().PriceData.UnitAmount);
    }

    [Fact]
    public async Task PayFullCharges_ExpiredPendingSession_IsFailedAndUnlocked()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000011");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PX00011");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 0m
        });

        var firstPayment = await db.Context.Payment.SingleAsync();
        firstPayment.CreatedAt = DateTime.UtcNow.AddHours(-2);
        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();

        var response = await service.PayFullChargesAsync(new PayFullChargesRequest
        {
            ChargeIds = [charge.Id],
            CreditBalanceApplied = 0m
        });

        var payments = await db.Context.Payment.OrderBy(payment => payment.Id).ToListAsync();
        Assert.Equal(PaymentStatus.Pending.ToString(), response.Status);
        Assert.Equal(2, stripe.CreateCallCount);
        Assert.Equal(1, stripe.ExpireCallCount);
        Assert.Equal(PaymentStatus.Failed, payments[0].Status);
        Assert.Equal(PaymentStatus.Pending, payments[1].Status);
    }

    [Fact]
    public async Task PayFullCharges_MatchingPendingSession_ReturnsExistingCheckoutLink()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000012");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PR00012");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);
        var request = new PayFullChargesRequest { ChargeIds = [charge.Id] };

        var first = await service.PayFullChargesAsync(request);
        db.Context.ChangeTracker.Clear();
        var resumed = await service.PayFullChargesAsync(request);

        Assert.Equal(1, stripe.CreateCallCount);
        Assert.Equal(first.Link, resumed.Link);
        Assert.Equal(first.PaymentIds, resumed.PaymentIds);
        Assert.True(resumed.RequiresRedirect);
        Assert.Single(await db.Context.Payment.ToListAsync());
    }

    [Fact]
    public async Task PayFullCharges_PaidPendingSession_FinalizesInsteadOfCreatingAnotherSession()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000013");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PP00013");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);
        var request = new PayFullChargesRequest { ChargeIds = [charge.Id] };

        await service.PayFullChargesAsync(request);
        var sessionId = await db.Context.Payment.Select(payment => payment.ExternalReference!).SingleAsync();
        stripe.MarkPaid(sessionId);
        db.Context.ChangeTracker.Clear();

        var recovered = await service.PayFullChargesAsync(request);

        db.Context.ChangeTracker.Clear();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        Assert.Equal(1, stripe.CreateCallCount);
        Assert.Equal(PaymentStatus.Succeeded.ToString(), recovered.Status);
        Assert.Equal(ChargeStatus.Paid, reloadedCharge!.Status);
        Assert.Equal(120m, reloadedCharge.PaidAmount);
        Assert.Single(await db.Context.Payment.ToListAsync());
        Assert.Single(await db.Context.EducationCreditTransaction.ToListAsync());
    }

    [Fact]
    public async Task PayFullCharges_StripeLookupFailure_KeepsExistingPaymentPending()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000014");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PL00014");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);
        var request = new PayFullChargesRequest { ChargeIds = [charge.Id] };

        await service.PayFullChargesAsync(request);
        db.Context.ChangeTracker.Clear();
        stripe.GetException = new InvalidOperationException("Stripe unavailable");

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.PayFullChargesAsync(request));

        db.Context.ChangeTracker.Clear();
        var payment = await db.Context.Payment.SingleAsync();
        Assert.Equal(1, stripe.CreateCallCount);
        Assert.Equal(PaymentStatus.Pending, payment.Status);
    }

    [Fact]
    public async Task PayFullCharges_StripeCreateFailure_MarksReservationFailed()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000015");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PC00015");
        var stripe = new FakeStripeCheckoutGateway
        {
            CreateException = new InvalidOperationException("Stripe unavailable")
        };
        var service = db.CreateStripeService(graph.UserId, stripe);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.PayFullChargesAsync(new PayFullChargesRequest { ChargeIds = [charge.Id] }));

        db.Context.ChangeTracker.Clear();
        var payment = await db.Context.Payment.SingleAsync();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        Assert.Equal(PaymentStatus.Failed, payment.Status);
        Assert.Equal(0m, reloadedCharge!.PaidAmount);
        Assert.Equal(120m, reloadedCharge.RemainingAmount);
    }

    [Fact]
    public async Task HandleSessionSuccess_RepeatedCall_DoesNotApplyPaymentTwice()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000016");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "PI00016");
        var stripe = new FakeStripeCheckoutGateway();
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.PayFullChargesAsync(new PayFullChargesRequest { ChargeIds = [charge.Id] });
        var sessionId = await db.Context.Payment.Select(payment => payment.ExternalReference!).SingleAsync();
        stripe.MarkPaid(sessionId);

        await service.HandleSessionSuccessAsync(sessionId);
        await service.HandleSessionSuccessAsync(sessionId);

        db.Context.ChangeTracker.Clear();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        Assert.Equal(120m, reloadedCharge!.PaidAmount);
        Assert.Equal(0m, reloadedCharge.RemainingAmount);
        Assert.Single(await db.Context.EducationCreditTransaction.ToListAsync());
    }

    [Fact]
    public async Task HandleSessionCancelled_WhenPaymentCompletesDuringExpire_FinalizesSuccess()
    {
        await using var db = new TestDatabase();
        var graph = await db.CreateAccountGraphAsync("2000017");
        var (_, charge) = await CreateChargeForAccountAsync(db, graph, "CR00017");
        var stripe = new FakeStripeCheckoutGateway { CompletePaymentOnExpire = true };
        var service = db.CreateStripeService(graph.UserId, stripe);

        await service.PayFullChargesAsync(new PayFullChargesRequest { ChargeIds = [charge.Id] });
        var sessionId = await db.Context.Payment.Select(payment => payment.ExternalReference!).SingleAsync();

        var response = await service.HandleSessionCancelledAsync(sessionId);

        db.Context.ChangeTracker.Clear();
        var payment = await db.Context.Payment.SingleAsync();
        var reloadedCharge = await db.Context.Charge.FindAsync(charge.Id);
        Assert.Equal(PaymentStatus.Succeeded.ToString(), response.Status);
        Assert.Equal(PaymentStatus.Succeeded, payment.Status);
        Assert.Equal(ChargeStatus.Paid, reloadedCharge!.Status);
        Assert.Equal(1, stripe.ExpireCallCount);
    }

    private static async Task<(Course Course, Charge Charge)> CreateChargeForAccountAsync(
        TestDatabase db,
        AccountGraph graph,
        string suffix)
    {
        var course = await db.CreateCourseAsync(graph, suffix, status: CourseStatus.Upcoming);
        var enrollment = await db.CreateEnrollmentAsync(graph, course);
        var charge = await db.CreateChargeAsync(enrollment);
        return (course, charge);
    }

    private static async Task SetChargeProgressAsync(
        TestDatabase db,
        int chargeId,
        decimal paid,
        decimal remaining,
        int planMonths)
    {
        var charge = await db.Context.Charge.FindAsync(chargeId);
        charge!.PaidAmount = paid;
        charge.RemainingAmount = remaining;
        charge.PaymentPlanMonths = planMonths;
        charge.Status = ChargeStatus.Overdue;
        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();
    }

    private static async Task SetUnpaidInstallmentsDueAsync(TestDatabase db, params int[] chargeIds)
    {
        var installments = await db.Context.ChargeInstallment
            .Where(installment => chargeIds.Contains(installment.ChargeId) &&
                                  installment.Status != ChargeInstallmentStatus.Paid)
            .ToListAsync();

        foreach (var installment in installments)
            installment.DueDate = DateTime.UtcNow.AddDays(-1);

        await db.Context.SaveChangesAsync();
        db.Context.ChangeTracker.Clear();
    }
}
