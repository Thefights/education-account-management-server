using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ScheduleTopUpConditionSeedBuilder : ISeedBuilder
    {
        public int Priority => 100;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var groups = new List<ScheduleTopUpConditionGroup>();
            var conditions = new List<ScheduleTopUpCondition>();
            var groupId = 1;
            var conditionId = 1;

            for (var scheduleId = 1; scheduleId <= 10; scheduleId++)
            {
                var rootId = groupId++;
                groups.Add(new ScheduleTopUpConditionGroup
                {
                    Id = rootId,
                    ScheduleTopUpId = scheduleId,
                    LogicalOperator = TopupLogicalOperator.And,
                    DisplayOrder = 0
                });
                conditions.Add(new ScheduleTopUpCondition
                {
                    Id = conditionId++,
                    GroupId = rootId,
                    Field = TopupConditionField.Balance,
                    Operator = TopupConditionOperator.LessThanOrEqual,
                    ValueNumber = 1_000,
                    DisplayOrder = 0
                });

                if (scheduleId <= 5)
                {
                    continue;
                }

                var childGroupId = groupId++;
                groups.Add(new ScheduleTopUpConditionGroup
                {
                    Id = childGroupId,
                    ScheduleTopUpId = scheduleId,
                    ParentGroupId = rootId,
                    LogicalOperator = TopupLogicalOperator.Or,
                    DisplayOrder = 1
                });
                conditions.Add(new ScheduleTopUpCondition
                {
                    Id = conditionId++,
                    GroupId = childGroupId,
                    Field = TopupConditionField.Age,
                    Operator = TopupConditionOperator.Between,
                    ValueNumber = 16,
                    ValueNumberTo = 21,
                    DisplayOrder = 0
                });
                conditions.Add(new ScheduleTopUpCondition
                {
                    Id = conditionId++,
                    GroupId = childGroupId,
                    Field = TopupConditionField.SchoolingStatus,
                    Operator = TopupConditionOperator.Equals,
                    ValueText = "Enrolled",
                    DisplayOrder = 1
                });
            }

            modelBuilder.Entity<ScheduleTopUpConditionGroup>().HasData(groups);
            modelBuilder.Entity<ScheduleTopUpCondition>().HasData(conditions);
            return modelBuilder;
        }
    }
}
