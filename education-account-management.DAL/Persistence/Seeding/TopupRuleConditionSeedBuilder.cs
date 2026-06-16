using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class TopupRuleConditionSeedBuilder : ISeedBuilder
{
    public int Priority => 100;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<TopupRuleCondition>().HasData(
            Enumerable.Range(1, 10).Select(id => new TopupRuleCondition
            {
                Id = id,
                Field = id % 3 == 0
                    ? TopupRuleConditionField.SchoolingStatus
                    : id % 2 == 0 ? TopupRuleConditionField.Balance : TopupRuleConditionField.Age,
                Operator = id % 2 == 0
                    ? TopupRuleConditionOperator.GreaterThanOrEqual
                    : TopupRuleConditionOperator.Equals,
                ValueText = id % 3 == 0 ? "Enrolled" : null,
                ValueNumber = id % 3 == 0 ? null : 18m + id,
                TopupRuleId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
