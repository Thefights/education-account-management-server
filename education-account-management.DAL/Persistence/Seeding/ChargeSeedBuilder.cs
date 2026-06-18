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
            new Charge { Id = 1, EnrollmentId = 1, GrossAmount = 120m, SubsidyAmount = 0m, NetAmount = 120m, PaidAmount = 120m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 2, EnrollmentId = 2, GrossAmount = 140m, SubsidyAmount = 0m, NetAmount = 140m, PaidAmount = 70m, RemainingAmount = 70m, Status = ChargeStatus.PartiallyPaid, CreatedAt = createdAt },
            new Charge { Id = 3, EnrollmentId = 3, GrossAmount = 160m, SubsidyAmount = 20m, NetAmount = 140m, PaidAmount = 140m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 4, EnrollmentId = 4, GrossAmount = 180m, SubsidyAmount = 0m, NetAmount = 180m, PaidAmount = 180m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 5, EnrollmentId = 5, GrossAmount = 200m, SubsidyAmount = 20m, NetAmount = 180m, PaidAmount = 180m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 6, EnrollmentId = 6, GrossAmount = 220m, SubsidyAmount = 0m, NetAmount = 220m, PaidAmount = 100m, RemainingAmount = 120m, Status = ChargeStatus.Outstanding, CreatedAt = createdAt },
            new Charge { Id = 7, EnrollmentId = 7, GrossAmount = 240m, SubsidyAmount = 40m, NetAmount = 200m, PaidAmount = 200m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 8, EnrollmentId = 8, GrossAmount = 260m, SubsidyAmount = 0m, NetAmount = 260m, PaidAmount = 130m, RemainingAmount = 130m, Status = ChargeStatus.PartiallyPaid, CreatedAt = createdAt },
            new Charge { Id = 9, EnrollmentId = 9, GrossAmount = 280m, SubsidyAmount = 30m, NetAmount = 250m, PaidAmount = 250m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt },
            new Charge { Id = 10, EnrollmentId = 10, GrossAmount = 300m, SubsidyAmount = 0m, NetAmount = 300m, PaidAmount = 300m, RemainingAmount = 0m, Status = ChargeStatus.Paid, CreatedAt = createdAt });

        return modelBuilder;
    }
}
