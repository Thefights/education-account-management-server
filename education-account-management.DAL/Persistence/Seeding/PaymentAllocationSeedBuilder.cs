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
                var baseInstallmentAmount = Math.Round(netAmount / 6m, 2);

                foreach (var installmentNumber in paidCharge.PaidInstallmentNumbers)
                {
                    var amount = installmentNumber < 6
                        ? baseInstallmentAmount
                        : netAmount - (baseInstallmentAmount * 5m);

                    allocations.Add(new PaymentAllocation
                    {
                        Id = allocationId++,
                        PaymentId = paidCharge.PaymentId,
                        ChargeId = paidCharge.ChargeId,
                        ChargeInstallmentId = SeedScenarioConstants.GetSterlingInstallmentId(
                            paidCharge.ChargeId,
                            installmentNumber),
                        CourseNameSnapshot = SeedScenarioConstants.GetCourseName(paidCharge.CourseId),
                        SchoolNameSnapshot = "Northview Secondary School",
                        ChargeGrossAmountSnapshot = netAmount + (paidCharge.CourseId % 4 == 0 ? 30m : 0m),
                        ChargeNetAmountSnapshot = netAmount,
                        ChargeRemainingAmountSnapshot = netAmount,
                        Amount = amount,
                        CreatedAt = createdAt.AddDays(10 + paidCharge.CourseIndex)
                    });
                }
            }

            modelBuilder.Entity<PaymentAllocation>().HasData(allocations);

            return modelBuilder;
        }
    }
}
