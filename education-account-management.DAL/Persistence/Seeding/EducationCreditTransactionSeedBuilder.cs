using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationCreditTransactionSeedBuilder : ISeedBuilder
    {
        public int Priority => 130;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var transactions = new List<EducationCreditTransaction>();

            for (int i = 1; i <= 50; i++)
            {
                transactions.Add(new EducationCreditTransaction
                {
                    Id = 100 + i,
                    TransactionCode = SeedScenarioConstants.GetSeedGuid("initial-topup", i),
                    Type = EducationCreditTransactionType.Topup,
                    Direction = EducationCreditTransactionDirection.Increased,
                    Amount = 1000m,
                    BalanceBefore = 0m,
                    BalanceAfter = 1000m,
                    Description = "Initial system top-up",
                    EducationAccountId = i,
                    CreatedAt = createdAt
                });
            }

            transactions.Add(new EducationCreditTransaction
            {
                Id = 203,
                TransactionCode = SeedScenarioConstants.GetSeedGuid("manual-adjustment", 203),
                Type = EducationCreditTransactionType.Topup,
                Direction = EducationCreditTransactionDirection.Increased,
                Amount = 105m,
                BalanceBefore = 1000m,
                BalanceAfter = 1105m,
                Description = "Manual Account Adjustment",
                EducationAccountId = 3,
                CreatedAt = createdAt.AddDays(3)
            });

            transactions.Add(new EducationCreditTransaction
            {
                Id = 207,
                TransactionCode = SeedScenarioConstants.GetSeedGuid("stem-enrichment-credit", 207),
                Type = EducationCreditTransactionType.Topup,
                Direction = EducationCreditTransactionDirection.Increased,
                Amount = 145m,
                BalanceBefore = 1000m,
                BalanceAfter = 1145m,
                Description = "STEM Enrichment Credit",
                EducationAccountId = 7,
                CreatedAt = createdAt.AddDays(7)
            });

            var balance = 1000m;
            foreach (var paidCharge in GetSterlingPaidCharges())
            {
                transactions.Add(new EducationCreditTransaction
                {
                    Id = paidCharge.TransactionId,
                    TransactionCode = SeedScenarioConstants.GetSeedGuid("sterling-course-fee", paidCharge.TransactionId),
                    Type = EducationCreditTransactionType.CourseFeePayment,
                    Direction = EducationCreditTransactionDirection.Decreased,
                    Amount = paidCharge.Amount,
                    BalanceBefore = balance,
                    BalanceAfter = balance - paidCharge.Amount,
                    Description = $"Course fee payment for {SeedScenarioConstants.GetCourseName(paidCharge.CourseId)}",
                    EducationAccountId = 1,
                    CreatedAt = createdAt.AddDays(10 + paidCharge.CourseIndex)
                });

                balance -= paidCharge.Amount;
            }

            modelBuilder.Entity<EducationCreditTransaction>().HasData(transactions);

            return modelBuilder;
        }

        internal static IReadOnlyList<SterlingPaidChargeSeed> GetSterlingPaidCharges()
        {
            var paidCharges = new List<SterlingPaidChargeSeed>();
            for (var index = 0; index < SeedScenarioConstants.SterlingCourseIds.Length; index++)
            {
                var courseId = SeedScenarioConstants.SterlingCourseIds[index];
                if (index < 3)
                {
                    AddPaidInstallments(paidCharges, index, courseId, [1, 2, 3, 4, 5, 6]);
                }
                else if (SeedScenarioConstants.GetCourseStatus(courseId) == CourseStatus.Closed)
                {
                    AddPaidInstallments(paidCharges, index, courseId, [1]);
                }
            }

            return paidCharges;
        }

        private static void AddPaidInstallments(
            List<SterlingPaidChargeSeed> paidCharges,
            int courseIndex,
            int courseId,
            int[] installmentNumbers)
        {
            var netAmount = SeedScenarioConstants.GetNetAmount(courseId);
            var baseInstallmentAmount = Math.Round(netAmount / 6m, 2);
            var chargeId = SeedScenarioConstants.GetEnrollmentId(courseId, schoolStudentId: 1);

            foreach (var installmentNumber in installmentNumbers)
            {
                var amount = installmentNumber < 6
                    ? baseInstallmentAmount
                    : netAmount - (baseInstallmentAmount * 5m);
                var seedId = 3000 + (courseIndex * 10) + installmentNumber;

                paidCharges.Add(new SterlingPaidChargeSeed(
                    CourseIndex: courseIndex,
                    CourseId: courseId,
                    ChargeId: chargeId,
                    ChargeInstallmentId: SeedScenarioConstants.GetSterlingInstallmentId(
                        chargeId,
                        installmentNumber),
                    InstallmentNumber: installmentNumber,
                    TransactionId: seedId,
                    PaymentId: seedId,
                    Amount: amount));
            }
        }
    }

    internal sealed record SterlingPaidChargeSeed(
        int CourseIndex,
        int CourseId,
        int ChargeId,
        int ChargeInstallmentId,
        int InstallmentNumber,
        int TransactionId,
        int PaymentId,
        decimal Amount);
}
