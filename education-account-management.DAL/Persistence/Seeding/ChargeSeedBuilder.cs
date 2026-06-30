using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ChargeSeedBuilder : ISeedBuilder
    {
        public int Priority => 110;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var charges = new List<Charge>();

            for (int i = 1; i <= 50; i++)
            {
                if (CanSeedChargeForCourse(i))
                {
                    charges.Add(CreateCharge(
                        id: i,
                        enrollmentId: i,
                        courseId: i,
                        schoolStudentId: i,
                        createdAt));
                }
            }

            foreach (var link in SeedScenarioConstants.GetAdditionalCourseTestEnrollments())
            {
                charges.Add(CreateCharge(
                    id: link.EnrollmentId,
                    enrollmentId: link.EnrollmentId,
                    courseId: link.CourseId,
                    schoolStudentId: link.SchoolStudentId,
                    createdAt));
            }

            modelBuilder.Entity<Charge>().HasData(charges);

            return modelBuilder;
        }

        private static bool CanSeedChargeForCourse(int courseId)
        {
            var status = SeedScenarioConstants.GetCourseStatus(courseId);

            return status is not CourseStatus.Draft and not CourseStatus.Enrolling;
        }

        private static Charge CreateCharge(
            int id,
            int enrollmentId,
            int courseId,
            int schoolStudentId,
            DateTime createdAt)
        {
            var courseFee = SeedScenarioConstants.GetCourseFee(courseId);
            var miscFee = SeedScenarioConstants.GetMiscFee(courseId);
            var gst = SeedScenarioConstants.GetGst(courseId);
            var grossAmount = courseFee + miscFee + gst;
            var subsidyAmount = courseId % 4 == 0 ? 30m : 0m;
            var netAmount = grossAmount - subsidyAmount;
            var status = GetChargeStatus(courseId, schoolStudentId);
            var paidAmount = GetPaidAmount(status, courseId, schoolStudentId, netAmount);

            var appliedFasApplicationId = courseId % 4 == 0
                ? new[] { 4, 8, 2, 6, 10 }[(courseId / 4 - 1) % 5]
                : (int?)null;

            return new Charge
            {
                Id = id,
                Status = status,
                GrossAmount = grossAmount,
                SubsidyAmount = subsidyAmount,
                NetAmount = netAmount,
                PaidAmount = paidAmount,
                RemainingAmount = netAmount - paidAmount,
                SchoolNameSnapshot = "Northview Secondary School",
                CourseCodeSnapshot = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.CoursePrefix, courseId),
                CourseNameSnapshot = SeedScenarioConstants.GetCourseName(courseId),
                CourseDescriptionSnapshot = "Tuition charge generated from enrollment.",
                CourseStartDateSnapshot = SeedScenarioConstants.GetCourseStartDate(courseId),
                CourseEndDateSnapshot = SeedScenarioConstants.GetCourseEndDate(courseId),
                CourseFeeAmountSnapshot = courseFee,
                MiscFeeAmountSnapshot = miscFee,
                GstAmountSnapshot = gst,
                TaxRateSnapshot = 0.09m,
                AppliedFasSchemeNameSnapshot = appliedFasApplicationId.HasValue
                    ? "Low Income Course Fee Support"
                    : null,
                AppliedFasTierNameSnapshot = appliedFasApplicationId.HasValue ? "Tier 1" : null,
                AppliedFasSubsidyTypeSnapshot = appliedFasApplicationId.HasValue
                    ? FasSubsidyType.FixedAmount
                    : null,
                AppliedFasIsPerComponentSnapshot = false,
                AppliedFasSubsidyValueSnapshot = appliedFasApplicationId.HasValue ? 30m : null,
                EnrollmentId = enrollmentId,
                AppliedFasApplicationId = appliedFasApplicationId,
                CreatedAt = createdAt
            };
        }

        private static ChargeStatus GetChargeStatus(int courseId, int schoolStudentId)
        {
            if (SeedScenarioConstants.IsSterlingEnrollment(schoolStudentId))
            {
                var sterlingCourseIndex = SeedScenarioConstants.GetSterlingCourseIndex(courseId);
                if (sterlingCourseIndex is >= 0 and < 3)
                {
                    return ChargeStatus.Paid;
                }
            }

            return SeedScenarioConstants.GetCourseStatus(courseId) == CourseStatus.Closed
                ? ChargeStatus.Overdue
                : ChargeStatus.PendingPayment;
        }

        private static decimal GetPaidAmount(
            ChargeStatus status,
            int courseId,
            int schoolStudentId,
            decimal netAmount)
        {
            if (status == ChargeStatus.Paid)
            {
                return netAmount;
            }

            if (status == ChargeStatus.Overdue && SeedScenarioConstants.IsSterlingEnrollment(schoolStudentId))
            {
                return Math.Round(netAmount / 6m, 2);
            }

            return 0m;
        }
    }
}
