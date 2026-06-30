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
            string[] courseNames = { "Academic Writing", "Business Numeracy", "Digital Literacy", "Career Readiness", "Applied Science", "Financial Literacy", "Project Collaboration", "Data Skills", "Workplace Communication", "Software Foundations" };
            
            int allocationId = 1;
            for (int i = 1; i <= 50; i++)
            {
                if (i % 3 == 0) // Succeeded payments
                {
                    decimal courseFee = 125m + (i * 5m);
                    decimal miscFee = 23m;
                    decimal gst = Math.Round((courseFee + miscFee) * 0.09m, 2);
                    decimal grossAmount = courseFee + miscFee + gst;
                    decimal subsidyAmount = i % 4 == 0 ? 30m : 0m;
                    decimal netAmount = grossAmount - subsidyAmount;
                    
                    allocations.Add(new PaymentAllocation
                    {
                        Id = allocationId++,
                        PaymentId = i,
                        ChargeId = i,
                        ChargeInstallmentId = i,
                        CourseNameSnapshot = courseNames[(i - 1) % 10] + " Cohort " + i.ToString("D2"),
                        SchoolNameSnapshot = "Northview Secondary School",
                        ChargeGrossAmountSnapshot = grossAmount,
                        ChargeNetAmountSnapshot = netAmount,
                        ChargeRemainingAmountSnapshot = netAmount,
                        Amount = netAmount,
                        CreatedAt = createdAt
                    });
                }
            }

            allocations.Add(new PaymentAllocation { Id = allocationId++, PaymentId = 51, ChargeId = 51, ChargeInstallmentId = 51, CourseNameSnapshot = "Creative Thinking Cohort 51", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 1110m, ChargeNetAmountSnapshot = 1110m, ChargeRemainingAmountSnapshot = 1110m, Amount = 185m, CreatedAt = createdAt.AddDays(30) });

            modelBuilder.Entity<PaymentAllocation>().HasData(allocations);
            return modelBuilder;
        }
    }
}
