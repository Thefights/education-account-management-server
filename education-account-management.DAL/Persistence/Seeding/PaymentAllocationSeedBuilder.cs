using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class PaymentAllocationSeedBuilder : ISeedBuilder
{
    public int Priority => 170;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<PaymentAllocation>().HasData(
            new PaymentAllocation { Id = 1, PaymentId = 1, ChargeId = 1, Amount = 120m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 2, PaymentId = 2, ChargeId = 2, Amount = 70m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 3, PaymentId = 3, ChargeId = 3, Amount = 140m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 4, PaymentId = 4, ChargeId = 4, Amount = 180m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 5, PaymentId = 5, ChargeId = 5, Amount = 180m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 6, PaymentId = 6, ChargeId = 6, Amount = 100m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 7, PaymentId = 7, ChargeId = 7, Amount = 200m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 8, PaymentId = 8, ChargeId = 8, Amount = 130m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 9, PaymentId = 9, ChargeId = 9, Amount = 250m, CreatedAt = createdAt },
            new PaymentAllocation { Id = 10, PaymentId = 10, ChargeId = 10, Amount = 300m, CreatedAt = createdAt });

        return modelBuilder;
    }
}
