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

        modelBuilder.Entity<TopupRule>().HasData(
            Enumerable.Range(1, 10).Select(id => new TopupRule
            {
                Id = id,
                RuleName = $"Top-up Rule {id:000}",
                TopupAmount = 100m + id * 10m,
                Status = id % 5 == 0 ? TopupRuleStatus.Inactive : TopupRuleStatus.Active,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }
}
