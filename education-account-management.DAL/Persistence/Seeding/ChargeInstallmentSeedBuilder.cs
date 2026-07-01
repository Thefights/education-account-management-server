using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ChargeInstallmentSeedBuilder : ISeedBuilder
    {
        public int Priority => 120;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var installments = new List<ChargeInstallment>();

            for (int i = 1; i <= 50; i++)
            {
                if (CanSeedChargeForCourse(i))
                {
                    AddInstallments(installments, chargeId: i, courseId: i, schoolStudentId: i, createdAt);
                }
            }

            foreach (var link in SeedScenarioConstants.GetAdditionalCourseTestEnrollments())
            {
                AddInstallments(
                    installments,
                    chargeId: link.EnrollmentId,
                    courseId: link.CourseId,
                    schoolStudentId: link.SchoolStudentId,
                    createdAt);
            }

            modelBuilder.Entity<ChargeInstallment>().HasData(installments);

            return modelBuilder;
        }

        private static bool CanSeedChargeForCourse(int courseId)
        {
            var status = SeedScenarioConstants.GetCourseStatus(courseId);

            return status is not CourseStatus.Draft and not CourseStatus.Enrolling;
        }

        private static void AddInstallments(
            List<ChargeInstallment> installments,
            int chargeId,
            int courseId,
            int schoolStudentId,
            DateTime createdAt)
        {
            var netAmount = SeedScenarioConstants.GetNetAmount(courseId);
            var isSterling = SeedScenarioConstants.IsSterlingEnrollment(schoolStudentId);

            if (!isSterling)
            {
                var status = SeedScenarioConstants.GetCourseStatus(courseId) == CourseStatus.Closed
                    ? ChargeInstallmentStatus.Overdue
                    : ChargeInstallmentStatus.PendingPayment;

                installments.Add(new ChargeInstallment
                {
                    Id = chargeId,
                    ChargeId = chargeId,
                    InstallmentNumber = 1,
                    Status = status,
                    DueDate = GetDueDate(courseId, 1),
                    Amount = netAmount,
                    CreatedAt = createdAt
                });

                return;
            }

            var baseAmount = Math.Round(netAmount / 6m, 2);
            for (var installmentNumber = 1; installmentNumber <= 6; installmentNumber++)
            {
                var amount = installmentNumber < 6
                    ? baseAmount
                    : netAmount - (baseAmount * 5m);

                var status = GetSterlingInstallmentStatus(courseId, installmentNumber);

                installments.Add(new ChargeInstallment
                {
                    Id = SeedScenarioConstants.GetSterlingInstallmentId(chargeId, installmentNumber),
                    ChargeId = chargeId,
                    InstallmentNumber = installmentNumber,
                    Status = status,
                    DueDate = GetDueDate(courseId, installmentNumber),
                    Amount = amount,
                    CreatedAt = createdAt
                });
            }
        }

        private static ChargeInstallmentStatus GetSterlingInstallmentStatus(
            int courseId,
            int installmentNumber)
        {
            var sterlingCourseIndex = SeedScenarioConstants.GetSterlingCourseIndex(courseId);
            if (sterlingCourseIndex is >= 0 and < 3)
            {
                return ChargeInstallmentStatus.Paid;
            }

            if (SeedScenarioConstants.GetCourseStatus(courseId) == CourseStatus.Closed)
            {
                return installmentNumber == 1
                    ? ChargeInstallmentStatus.Paid
                    : ChargeInstallmentStatus.Overdue;
            }

            return ChargeInstallmentStatus.PendingPayment;
        }

        private static DateTime GetDueDate(int courseId, int installmentNumber)
        {
            return SeedScenarioConstants.GetCourseStartDate(courseId).AddMonths(installmentNumber - 1);
        }
    }
}
