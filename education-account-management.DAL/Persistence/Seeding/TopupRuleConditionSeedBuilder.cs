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
        var random = new Random(12345);
        var conditions = new List<TopupRuleCondition>();
        
        int conditionId = 1;
        for (int ruleId = 1; ruleId <= 50; ruleId++)
        {
            var field = random.NextDouble() > 0.5 ? TopupRuleConditionField.Balance : TopupRuleConditionField.Age;

            decimal minVal;
            decimal maxVal;

            if (field == TopupRuleConditionField.Age)
            {
                minVal = random.Next(12, 18);
                maxVal = random.Next((int)minVal + 1, 25);
            }
            else
            {
                minVal = random.Next(0, 10) * 100m; // 0 to 900
                maxVal = minVal + random.Next(1, 10) * 100m; // min + 100 to 900
            }

            // Min Condition (>=)
            conditions.Add(new TopupRuleCondition
            {
                Id = conditionId++,
                Field = field,
                Operator = TopupRuleConditionOperator.GreaterThanOrEqual,
                ValueNumber = minVal,
                TopupRuleId = ruleId,
                CreatedAt = createdAt
            });

            // Max Condition (<=)
            conditions.Add(new TopupRuleCondition
            {
                Id = conditionId++,
                Field = field,
                Operator = TopupRuleConditionOperator.LessThanOrEqual,
                ValueNumber = maxVal,
                TopupRuleId = ruleId,
                CreatedAt = createdAt
            });
        }

        modelBuilder.Entity<TopupRuleCondition>().HasData(conditions);

        return modelBuilder;
    }

}
