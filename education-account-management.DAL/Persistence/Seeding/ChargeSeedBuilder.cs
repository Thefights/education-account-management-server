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
            new Charge { Id = 1, EnrollmentId = 1, CourseFeeAmountSnapshot = 100m, MiscFeeAmountSnapshot = 10m, GstAmountSnapshot = 10m, GrossAmount = 120m, SubsidyAmount = 0m, NetAmount = 120m, PaidAmount = 120m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 2, EnrollmentId = 2, CourseFeeAmountSnapshot = 115m, MiscFeeAmountSnapshot = 12m, GstAmountSnapshot = 13m, GrossAmount = 140m, SubsidyAmount = 0m, NetAmount = 140m, PaidAmount = 70m, RemainingAmount = 70m, Status = ChargeStatus.PartiallyPaid, CreatedAt = createdAt },
            new Charge { Id = 3, EnrollmentId = 3, CourseFeeAmountSnapshot = 130m, MiscFeeAmountSnapshot = 15m, GstAmountSnapshot = 15m, GrossAmount = 160m, SubsidyAmount = 20m, NetAmount = 140m, PaidAmount = 140m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 4, EnrollmentId = 4, CourseFeeAmountSnapshot = 145m, MiscFeeAmountSnapshot = 17m, GstAmountSnapshot = 18m, GrossAmount = 180m, SubsidyAmount = 0m, NetAmount = 180m, PaidAmount = 180m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 5, EnrollmentId = 5, CourseFeeAmountSnapshot = 160m, MiscFeeAmountSnapshot = 20m, GstAmountSnapshot = 20m, GrossAmount = 200m, SubsidyAmount = 20m, NetAmount = 180m, PaidAmount = 180m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 6, EnrollmentId = 6, CourseFeeAmountSnapshot = 175m, MiscFeeAmountSnapshot = 22m, GstAmountSnapshot = 23m, GrossAmount = 220m, SubsidyAmount = 0m, NetAmount = 220m, PaidAmount = 100m, RemainingAmount = 120m, Status = ChargeStatus.Outstanding, CreatedAt = createdAt },
            new Charge { Id = 7, EnrollmentId = 7, CourseFeeAmountSnapshot = 190m, MiscFeeAmountSnapshot = 25m, GstAmountSnapshot = 25m, GrossAmount = 240m, SubsidyAmount = 40m, NetAmount = 200m, PaidAmount = 200m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 8, EnrollmentId = 8, CourseFeeAmountSnapshot = 205m, MiscFeeAmountSnapshot = 27m, GstAmountSnapshot = 28m, GrossAmount = 260m, SubsidyAmount = 0m, NetAmount = 260m, PaidAmount = 130m, RemainingAmount = 130m, Status = ChargeStatus.PartiallyPaid, CreatedAt = createdAt },
            new Charge { Id = 9, EnrollmentId = 9, CourseFeeAmountSnapshot = 220m, MiscFeeAmountSnapshot = 30m, GstAmountSnapshot = 30m, GrossAmount = 280m, SubsidyAmount = 30m, NetAmount = 250m, PaidAmount = 250m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 10, EnrollmentId = 10, CourseFeeAmountSnapshot = 235m, MiscFeeAmountSnapshot = 32m, GstAmountSnapshot = 33m, GrossAmount = 300m, SubsidyAmount = 0m, NetAmount = 300m, PaidAmount = 300m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt });

        return modelBuilder;
    }
}
