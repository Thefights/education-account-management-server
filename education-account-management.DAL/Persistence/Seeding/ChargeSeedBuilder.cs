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
            string[] courseNames = { "Academic Writing", "Business Numeracy", "Digital Literacy", "Career Readiness", "Applied Science", "Financial Literacy", "Project Collaboration", "Data Skills", "Workplace Communication", "Software Foundations" };
            
            for (int i = 1; i <= 50; i++)
            {
                decimal courseFee = 125m + (i * 5m);
                decimal miscFee = 23m;
                decimal gst = Math.Round((courseFee + miscFee) * 0.09m, 2);
                decimal grossAmount = courseFee + miscFee + gst;
                
                decimal subsidyAmount = 0m;
                int? appliedFasApplicationId = null;
                string? appliedScheme = null;
                string? appliedTier = null;
                FasSubsidyType? subsidyType = null;
                decimal? subsidyValue = null;
                
                if (i % 4 == 0)
                {
                    subsidyAmount = 30m;
                    int[] fasIds = { 4, 8, 2, 6, 10 };
                    appliedFasApplicationId = fasIds[(i / 4 - 1) % 5];
                    appliedScheme = "Low Income Course Fee Support";
                    appliedTier = "Tier 1";
                    subsidyType = FasSubsidyType.FixedAmount;
                    subsidyValue = 30m;
                }
                
                decimal netAmount = grossAmount - subsidyAmount;
                
                var status = ChargeStatus.PendingPayment;
                if (i % 3 == 0) status = ChargeStatus.Paid;
                else if (i % 3 == 2) status = ChargeStatus.Overdue;
                
                decimal paidAmount = status == ChargeStatus.Paid ? netAmount : 0m;
                decimal remainingAmount = netAmount - paidAmount;
                
                charges.Add(new Charge
                {
                    Id = i,
                    Status = status,
                    GrossAmount = grossAmount,
                    SubsidyAmount = subsidyAmount,
                    NetAmount = netAmount,
                    PaidAmount = paidAmount,
                    RemainingAmount = remainingAmount,
                    SchoolNameSnapshot = "Northview Secondary School",
                    CourseCodeSnapshot = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.CoursePrefix, i),
                    CourseNameSnapshot = courseNames[(i - 1) % 10] + " Cohort " + i.ToString("D2"),
                    CourseDescriptionSnapshot = "Tuition charge generated from enrollment.",
                    CourseStartDateSnapshot = new DateTime(2026, 8, (i % 28) + 1, 0, 0, 0, DateTimeKind.Utc),
                    CourseEndDateSnapshot = new DateTime(2026, 10, (i % 28) + 1, 0, 0, 0, DateTimeKind.Utc),
                    CourseFeeAmountSnapshot = courseFee,
                    MiscFeeAmountSnapshot = miscFee,
                    GstAmountSnapshot = gst,
                    TaxRateSnapshot = 0.09m,
                    AppliedFasSchemeNameSnapshot = appliedScheme,
                    AppliedFasTierNameSnapshot = appliedTier,
                    AppliedFasSubsidyTypeSnapshot = subsidyType,
                    AppliedFasIsPerComponentSnapshot = false,
                    AppliedFasSubsidyValueSnapshot = subsidyValue,
                    EnrollmentId = i,
                    AppliedFasApplicationId = appliedFasApplicationId,
                    CreatedAt = createdAt
                });
            }
            
            // Sterling Quach special installment coverage (Id=51)
            charges.Add(new Charge { Id = 51, Status = ChargeStatus.PendingPayment, GrossAmount = 1110m, SubsidyAmount = 0m, NetAmount = 1110m, PaidAmount = 185m, RemainingAmount = 925m, SchoolNameSnapshot = "Northview Secondary School", CourseCodeSnapshot = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.CoursePrefix, 51), CourseNameSnapshot = "Creative Thinking Cohort 51", CourseDescriptionSnapshot = "Tuition charge generated from enrollment.", CourseStartDateSnapshot = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc), CourseEndDateSnapshot = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc), CourseFeeAmountSnapshot = 900m, MiscFeeAmountSnapshot = 118.35m, GstAmountSnapshot = 91.65m, TaxRateSnapshot = 0.09m, AppliedFasSchemeNameSnapshot = null, AppliedFasTierNameSnapshot = null, AppliedFasSubsidyTypeSnapshot = null, AppliedFasIsPerComponentSnapshot = false, AppliedFasSubsidyValueSnapshot = null, EnrollmentId = 51, AppliedFasApplicationId = null, CreatedAt = createdAt });

            modelBuilder.Entity<Charge>().HasData(charges);
            return modelBuilder;
        }
    }
}
