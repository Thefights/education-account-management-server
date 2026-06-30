using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class PaymentSeedBuilder : ISeedBuilder
    {
        public int Priority => 140;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var payments = new List<Payment>();

            foreach (var paidCharge in EducationCreditTransactionSeedBuilder.GetSterlingPaidCharges())
            {
                payments.Add(new Payment
                {
                    Id = paidCharge.PaymentId,
                    EducationCreditTransactionId = paidCharge.TransactionId,
                    PaymentMethod = PaymentMethod.EducationBalance,
                    Status = PaymentStatus.Succeeded,
                    AccountNumberSnapshot = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 1),
                    CitizenNricSnapshot = SingaporeNricUtil.Generate(1),
                    CitizenFullNameSnapshot = "Sterling Quach",
                    TotalAmount = paidCharge.Amount,
                    CreatedAt = createdAt.AddDays(10 + paidCharge.CourseIndex),
                    PaidAt = createdAt.AddDays(10 + paidCharge.CourseIndex)
                });
            }

            modelBuilder.Entity<Payment>().HasData(payments);

            return modelBuilder;
        }
    }
}
