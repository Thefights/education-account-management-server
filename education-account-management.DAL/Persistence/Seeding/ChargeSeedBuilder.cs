using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class ChargeSeedBuilder : ISeedBuilder
{
    public int Priority => 130;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<Charge>().HasData(
            new Charge { Id = 1, EnrollmentId = 1, Status = ChargeStatus.Paid, CourseFeeAmountSnapshot = 100m, MiscFeeAmountSnapshot = 10m, GstAmountSnapshot = 10m, GrossAmount = 120m, SubsidyAmount = 0m, NetAmount = 120m, PaidAmount = 120m, RemainingAmount = 0m, PaymentDueDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 2, EnrollmentId = 2, Status = ChargeStatus.PartiallyPaid, CourseFeeAmountSnapshot = 115m, MiscFeeAmountSnapshot = 12m, GstAmountSnapshot = 13m, GrossAmount = 140m, SubsidyAmount = 0m, NetAmount = 140m, PaidAmount = 70m, RemainingAmount = 70m, PaymentDueDate = new DateTime(2026, 7, 2, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 3, EnrollmentId = 3, Status = ChargeStatus.Paid, CourseFeeAmountSnapshot = 130m, MiscFeeAmountSnapshot = 15m, GstAmountSnapshot = 15m, GrossAmount = 160m, SubsidyAmount = 20m, NetAmount = 140m, PaidAmount = 140m, RemainingAmount = 0m, PaymentDueDate = new DateTime(2026, 7, 3, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 4, EnrollmentId = 4, Status = ChargeStatus.Paid, CourseFeeAmountSnapshot = 145m, MiscFeeAmountSnapshot = 17m, GstAmountSnapshot = 18m, GrossAmount = 180m, SubsidyAmount = 0m, NetAmount = 180m, PaidAmount = 180m, RemainingAmount = 0m, PaymentDueDate = new DateTime(2026, 2, 4, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 5, EnrollmentId = 5, Status = ChargeStatus.Paid, CourseFeeAmountSnapshot = 160m, MiscFeeAmountSnapshot = 20m, GstAmountSnapshot = 20m, GrossAmount = 200m, SubsidyAmount = 20m, NetAmount = 180m, PaidAmount = 180m, RemainingAmount = 0m, PaymentDueDate = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 6, EnrollmentId = 6, Status = ChargeStatus.Outstanding, CourseFeeAmountSnapshot = 175m, MiscFeeAmountSnapshot = 22m, GstAmountSnapshot = 23m, GrossAmount = 220m, SubsidyAmount = 0m, NetAmount = 220m, PaidAmount = 150m, RemainingAmount = 70m, PaymentDueDate = new DateTime(2026, 2, 6, 0, 0, 0, DateTimeKind.Utc), BecameOutstandingAt = new DateTime(2026, 2, 7, 0, 0, 0, DateTimeKind.Utc), LastAutoDeductedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 7, EnrollmentId = 7, Status = ChargeStatus.Paid, CourseFeeAmountSnapshot = 190m, MiscFeeAmountSnapshot = 25m, GstAmountSnapshot = 25m, GrossAmount = 240m, SubsidyAmount = 40m, NetAmount = 200m, PaidAmount = 200m, RemainingAmount = 0m, PaymentDueDate = new DateTime(2026, 2, 7, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 8, EnrollmentId = 8, Status = ChargeStatus.Outstanding, CourseFeeAmountSnapshot = 205m, MiscFeeAmountSnapshot = 27m, GstAmountSnapshot = 28m, GrossAmount = 260m, SubsidyAmount = 0m, NetAmount = 260m, PaidAmount = 200m, RemainingAmount = 60m, PaymentDueDate = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc), BecameOutstandingAt = new DateTime(2026, 2, 9, 0, 0, 0, DateTimeKind.Utc), LastAutoDeductedAt = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 9, EnrollmentId = 9, Status = ChargeStatus.Outstanding, CourseFeeAmountSnapshot = 220m, MiscFeeAmountSnapshot = 30m, GstAmountSnapshot = 30m, GrossAmount = 280m, SubsidyAmount = 30m, NetAmount = 250m, PaidAmount = 0m, RemainingAmount = 250m, PaymentDueDate = new DateTime(2026, 2, 9, 0, 0, 0, DateTimeKind.Utc), BecameOutstandingAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
            new Charge { Id = 10, EnrollmentId = 10, Status = ChargeStatus.Outstanding, CourseFeeAmountSnapshot = 235m, MiscFeeAmountSnapshot = 32m, GstAmountSnapshot = 33m, GrossAmount = 300m, SubsidyAmount = 0m, NetAmount = 300m, PaidAmount = 0m, RemainingAmount = 300m, PaymentDueDate = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), BecameOutstandingAt = new DateTime(2026, 2, 11, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt });

        return modelBuilder;
    }
}
