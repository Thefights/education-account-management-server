using Enums;
using Models;
using Persistence.Seeding.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
            var matchMode = i % 4 == 0 ? TopupMatchMode.Or : TopupMatchMode.And;
            rules.Add(new TopupRule
            {
                Id = i,
                RuleName = $"Random Top-up Rule {i:000}",
                Type = i <= 20 ? TopupRuleType.Schedule : TopupRuleType.System,
                MatchMode = matchMode,
                TopupAmount = matchMode == TopupMatchMode.And ? random.Next(1, 100) * 10m : null,
                Status = random.NextDouble() > 0.1 ? TopupRuleStatus.Active : TopupRuleStatus.Inactive, // 90% active
                CreatedAt = createdAt
            });
        }

        modelBuilder.Entity<TopupRule>().HasData(rules);

        return modelBuilder;
    }
}
