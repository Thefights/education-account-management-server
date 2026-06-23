using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class ScheduleTopUpConditionSeedBuilder : ISeedBuilder
{
    public int Priority => 100;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var groups = new List<ScheduleTopUpConditionGroup>();
        var conditions = new List<ScheduleTopUpCondition>();
        for (var scheduleId = 1; scheduleId <= 20; scheduleId++)
        {
            groups.Add(new ScheduleTopUpConditionGroup
            {
                Id = scheduleId,
                ScheduleTopUpId = scheduleId,
                LogicalOperator = TopupLogicalOperator.And,
                DisplayOrder = 0
            });
            conditions.Add(new ScheduleTopUpCondition
            {
                Id = scheduleId,
                GroupId = scheduleId,
                Field = TopupConditionField.Balance,
                Operator = TopupConditionOperator.LessThanOrEqual,
                ValueNumber = 1_000,
                DisplayOrder = 0
            });
        }

        modelBuilder.Entity<ScheduleTopUpConditionGroup>().HasData(groups);
        modelBuilder.Entity<ScheduleTopUpCondition>().HasData(conditions);
        return modelBuilder;
    }
}