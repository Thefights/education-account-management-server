using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class PaymentAllocationSeedBuilder : ISeedBuilder
{
    public int Priority => 170;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;
        var courseNames = new[]
        {
            "Applied Mathematics",
            "Computer Science Fundamentals",
            "Business Communication",
            "Environmental Science",
            "Digital Media Design",
            "Hospitality Operations",
            "Electrical Engineering Basics",
            "Creative Writing",
            "Data Analytics",
            "Legacy Office Applications"
        };
        var schoolNames = new[]
        {
            "Northview Secondary School",
            "Eastbridge Secondary School",
            "Westhaven Secondary School",
            "Southpoint Secondary School",
            "Central Heights School",
            "Riverside Learning Institute",
            "Lakeside Technical School",
            "Greenfield Academy",
            "Harbourfront School",
            "Hillcrest Education Centre"
        };
        var chargeGrossAmounts = new[] { 120m, 140m, 160m, 180m, 200m, 220m, 240m, 260m, 280m, 300m };
        var chargeNetAmounts = new[] { 120m, 140m, 140m, 180m, 180m, 220m, 200m, 260m, 250m, 300m };
        var chargeRemainingAmounts = new[] { 0m, 70m, 0m, 0m, 0m, 120m, 0m, 130m, 0m, 0m };
        var allocationAmounts = new[] { 120m, 70m, 140m, 180m, 180m, 100m, 200m, 130m, 250m, 300m };

        PaymentAllocation CreatePaymentAllocation(int id)
        {
            return new PaymentAllocation
            {
                Id = id,
                PaymentId = id,
                ChargeId = id,
                CourseNameSnapshot = courseNames[id - 1],
                SchoolNameSnapshot = schoolNames[id - 1],
                ChargeGrossAmountSnapshot = chargeGrossAmounts[id - 1],
                ChargeNetAmountSnapshot = chargeNetAmounts[id - 1],
                ChargeRemainingAmountSnapshot = chargeRemainingAmounts[id - 1],
                Amount = allocationAmounts[id - 1],
                CreatedAt = createdAt
            };
        }

        modelBuilder.Entity<PaymentAllocation>().HasData(
            Enumerable.Range(1, 10).Select(CreatePaymentAllocation).ToArray());

        return modelBuilder;
    }
}
