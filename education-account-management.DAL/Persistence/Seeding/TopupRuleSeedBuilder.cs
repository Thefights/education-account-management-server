using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupRuleSeedBuilder : ISeedBuilder
{
    public int Priority => 90;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;
        var random = new Random(12345);
        var rules = new List<TopupRule>();

        for (int i = 1; i <= 50; i++)
        {
            rules.Add(new TopupRule
            {
                Id = i,
                RuleName = $"Random Top-up Rule {i:000}",
                TopupAmount = random.Next(1, 100) * 10m, // 10 to 990
                Status = random.NextDouble() > 0.1 ? TopupRuleStatus.Active : TopupRuleStatus.Inactive, // 90% active
                CreatedAt = createdAt
            });
        }

        modelBuilder.Entity<TopupRule>().HasData(rules);

        return modelBuilder;
    }
}
