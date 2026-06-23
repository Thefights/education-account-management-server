using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class PaymentAllocationSeedBuilder : ISeedBuilder
    {
        public int Priority => 170;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<PaymentAllocation>().HasData(
                new PaymentAllocation { Id = 1, PaymentId = 1, ChargeId = 2, CourseNameSnapshot = "Computer Science Fundamentals", SchoolNameSnapshot = "Eastbridge Secondary School", ChargeGrossAmountSnapshot = 140m, ChargeNetAmountSnapshot = 140m, ChargeRemainingAmountSnapshot = 140m, Amount = 70m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 2, PaymentId = 2, ChargeId = 3, CourseNameSnapshot = "Business Communication", SchoolNameSnapshot = "Westhaven Secondary School", ChargeGrossAmountSnapshot = 160m, ChargeNetAmountSnapshot = 160m, ChargeRemainingAmountSnapshot = 160m, Amount = 140m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 3, PaymentId = 3, ChargeId = 4, CourseNameSnapshot = "Environmental Science", SchoolNameSnapshot = "Southpoint Secondary School", ChargeGrossAmountSnapshot = 180m, ChargeNetAmountSnapshot = 180m, ChargeRemainingAmountSnapshot = 180m, Amount = 180m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 4, PaymentId = 4, ChargeId = 5, CourseNameSnapshot = "Digital Media Design", SchoolNameSnapshot = "Central Heights School", ChargeGrossAmountSnapshot = 200m, ChargeNetAmountSnapshot = 200m, ChargeRemainingAmountSnapshot = 200m, Amount = 180m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 5, PaymentId = 5, ChargeId = 1, CourseNameSnapshot = "Applied Mathematics", SchoolNameSnapshot = "Northview Secondary School", ChargeGrossAmountSnapshot = 120m, ChargeNetAmountSnapshot = 120m, ChargeRemainingAmountSnapshot = 120m, Amount = 120m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 6, PaymentId = 6, ChargeId = 6, CourseNameSnapshot = "Hospitality Operations", SchoolNameSnapshot = "Riverside Learning Institute", ChargeGrossAmountSnapshot = 220m, ChargeNetAmountSnapshot = 220m, ChargeRemainingAmountSnapshot = 220m, Amount = 100m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 7, PaymentId = 7, ChargeId = 7, CourseNameSnapshot = "Electrical Engineering Basics", SchoolNameSnapshot = "Lakeside Technical School", ChargeGrossAmountSnapshot = 240m, ChargeNetAmountSnapshot = 240m, ChargeRemainingAmountSnapshot = 240m, Amount = 200m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 8, PaymentId = 8, ChargeId = 8, CourseNameSnapshot = "Creative Writing", SchoolNameSnapshot = "Greenfield Academy", ChargeGrossAmountSnapshot = 260m, ChargeNetAmountSnapshot = 260m, ChargeRemainingAmountSnapshot = 260m, Amount = 130m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 9, PaymentId = 9, ChargeId = 6, CourseNameSnapshot = "Hospitality Operations", SchoolNameSnapshot = "Riverside Learning Institute", ChargeGrossAmountSnapshot = 220m, ChargeNetAmountSnapshot = 220m, ChargeRemainingAmountSnapshot = 120m, Amount = 50m, CreatedAt = createdAt },
                new PaymentAllocation { Id = 10, PaymentId = 10, ChargeId = 8, CourseNameSnapshot = "Creative Writing", SchoolNameSnapshot = "Greenfield Academy", ChargeGrossAmountSnapshot = 260m, ChargeNetAmountSnapshot = 260m, ChargeRemainingAmountSnapshot = 130m, Amount = 70m, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}