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

            modelBuilder.Entity<PaymentAllocation>().HasData(
                new PaymentAllocation { Id = 1, PaymentId = 2, ChargeId = 2, ChargeInstallmentId = 2, CourseNameSnapshot = "Academic Writing Cohort 02", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 162m, ChargeNetAmountSnapshot = 162m, ChargeRemainingAmountSnapshot = 162m, Amount = 128m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 2, PaymentId = 5, ChargeId = 5, ChargeInstallmentId = 5, CourseNameSnapshot = "Business Numeracy Cohort 05", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 180m, ChargeNetAmountSnapshot = 180m, ChargeRemainingAmountSnapshot = 180m, Amount = 140m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 3, PaymentId = 8, ChargeId = 8, ChargeInstallmentId = 8, CourseNameSnapshot = "Digital Literacy Cohort 08", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 198m, ChargeNetAmountSnapshot = 198m, ChargeRemainingAmountSnapshot = 198m, Amount = 152m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 4, PaymentId = 11, ChargeId = 11, ChargeInstallmentId = 11, CourseNameSnapshot = "Career Readiness Cohort 11", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 216m, ChargeNetAmountSnapshot = 216m, ChargeRemainingAmountSnapshot = 216m, Amount = 164m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 5, PaymentId = 14, ChargeId = 14, ChargeInstallmentId = 14, CourseNameSnapshot = "Applied Science Cohort 14", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 234m, ChargeNetAmountSnapshot = 234m, ChargeRemainingAmountSnapshot = 234m, Amount = 176m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 6, PaymentId = 17, ChargeId = 17, ChargeInstallmentId = 17, CourseNameSnapshot = "Financial Literacy Cohort 17", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 252m, ChargeNetAmountSnapshot = 252m, ChargeRemainingAmountSnapshot = 252m, Amount = 188m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 7, PaymentId = 20, ChargeId = 20, ChargeInstallmentId = 20, CourseNameSnapshot = "Project Collaboration Cohort 20", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 270m, ChargeNetAmountSnapshot = 270m, ChargeRemainingAmountSnapshot = 270m, Amount = 200m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 8, PaymentId = 23, ChargeId = 23, ChargeInstallmentId = 23, CourseNameSnapshot = "Data Skills Cohort 23", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 288m, ChargeNetAmountSnapshot = 288m, ChargeRemainingAmountSnapshot = 288m, Amount = 212m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 9, PaymentId = 26, ChargeId = 26, ChargeInstallmentId = 26, CourseNameSnapshot = "Workplace Communication Cohort 26", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 306m, ChargeNetAmountSnapshot = 306m, ChargeRemainingAmountSnapshot = 306m, Amount = 224m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 10, PaymentId = 29, ChargeId = 29, ChargeInstallmentId = 29, CourseNameSnapshot = "Software Foundations Cohort 29", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 324m, ChargeNetAmountSnapshot = 324m, ChargeRemainingAmountSnapshot = 324m, Amount = 236m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 11, PaymentId = 32, ChargeId = 32, ChargeInstallmentId = 32, CourseNameSnapshot = "Academic Writing Cohort 32", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 342m, ChargeNetAmountSnapshot = 342m, ChargeRemainingAmountSnapshot = 342m, Amount = 248m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 12, PaymentId = 35, ChargeId = 35, ChargeInstallmentId = 35, CourseNameSnapshot = "Business Numeracy Cohort 35", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 360m, ChargeNetAmountSnapshot = 360m, ChargeRemainingAmountSnapshot = 360m, Amount = 260m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 13, PaymentId = 38, ChargeId = 38, ChargeInstallmentId = 38, CourseNameSnapshot = "Digital Literacy Cohort 38", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 378m, ChargeNetAmountSnapshot = 378m, ChargeRemainingAmountSnapshot = 378m, Amount = 272m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 14, PaymentId = 41, ChargeId = 41, ChargeInstallmentId = 41, CourseNameSnapshot = "Career Readiness Cohort 41", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 396m, ChargeNetAmountSnapshot = 396m, ChargeRemainingAmountSnapshot = 396m, Amount = 284m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 15, PaymentId = 44, ChargeId = 44, ChargeInstallmentId = 44, CourseNameSnapshot = "Applied Science Cohort 44", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 414m, ChargeNetAmountSnapshot = 414m, ChargeRemainingAmountSnapshot = 414m, Amount = 296m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 16, PaymentId = 47, ChargeId = 47, ChargeInstallmentId = 47, CourseNameSnapshot = "Financial Literacy Cohort 47", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 432m, ChargeNetAmountSnapshot = 432m, ChargeRemainingAmountSnapshot = 432m, Amount = 308m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 17, PaymentId = 50, ChargeId = 50, ChargeInstallmentId = 50, CourseNameSnapshot = "Project Collaboration Cohort 50", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 450m, ChargeNetAmountSnapshot = 450m, ChargeRemainingAmountSnapshot = 450m, Amount = 320m, CreatedAt = createdAt },
                // Sterling Quach payment allocation for the paid installment
                new PaymentAllocation { Id = 18, PaymentId = 51, ChargeId = 51, ChargeInstallmentId = 51, CourseNameSnapshot = "Creative Thinking Cohort 51", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 185m, ChargeNetAmountSnapshot = 185m, ChargeRemainingAmountSnapshot = 185m, Amount = 185m, CreatedAt = createdAt.AddDays(30) });

            return modelBuilder;
        }
    }
}
