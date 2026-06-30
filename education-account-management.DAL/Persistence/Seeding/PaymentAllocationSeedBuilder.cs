using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class PaymentAllocationSeedBuilder : ISeedBuilder
    {
        public int Priority => 150;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var allocations = new List<PaymentAllocation>();
            var allocationId = 1;

            foreach (var paidCharge in EducationCreditTransactionSeedBuilder.GetSterlingPaidCharges())
            {
                var netAmount = SeedScenarioConstants.GetNetAmount(paidCharge.CourseId);

                allocations.Add(new PaymentAllocation
                {
                    Id = allocationId++,
                    PaymentId = paidCharge.PaymentId,
                    ChargeId = paidCharge.ChargeId,
                    ChargeInstallmentId = paidCharge.ChargeInstallmentId,
                    CourseNameSnapshot = SeedScenarioConstants.GetCourseName(paidCharge.CourseId),
                    SchoolNameSnapshot = "Northview Secondary School",
                    ChargeGrossAmountSnapshot = netAmount + (paidCharge.CourseId % 4 == 0 ? 30m : 0m),
                    ChargeNetAmountSnapshot = netAmount,
                    ChargeRemainingAmountSnapshot = netAmount,
                    Amount = paidCharge.Amount,
                    CreatedAt = createdAt.AddDays(10 + paidCharge.CourseIndex)
                });
            }

            modelBuilder.Entity<PaymentAllocation>().HasData(allocations);

            return modelBuilder;
        }
    }
}
