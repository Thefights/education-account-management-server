using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ScheduleTopUpConditionSeedBuilder : ISeedBuilder
    {
        public int Priority => 190;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<ScheduleTopUpConditionGroup>().HasData(
                new ScheduleTopUpConditionGroup { Id = 1, ScheduleTopUpId = 1, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 2, ScheduleTopUpId = 2, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 3, ScheduleTopUpId = 3, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 4, ScheduleTopUpId = 4, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 5, ScheduleTopUpId = 5, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 6, ScheduleTopUpId = 6, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 7, ScheduleTopUpId = 7, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 8, ScheduleTopUpId = 8, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 9, ScheduleTopUpId = 9, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new ScheduleTopUpConditionGroup { Id = 10, ScheduleTopUpId = 10, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 });

            modelBuilder.Entity<ScheduleTopUpCondition>().HasData(
                new ScheduleTopUpCondition { Id = 1, GroupId = 1, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 550m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 2, GroupId = 2, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 600m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 3, GroupId = 3, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 650m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 4, GroupId = 4, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 700m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 5, GroupId = 5, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 750m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 6, GroupId = 6, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 800m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 7, GroupId = 7, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 850m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 8, GroupId = 8, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 900m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 9, GroupId = 9, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 950m, DisplayOrder = 0 },
                new ScheduleTopUpCondition { Id = 10, GroupId = 10, Field = TopupConditionField.Balance, Operator = TopupConditionOperator.LessThanOrEqual, ValueNumber = 1000m, DisplayOrder = 0 });

            return modelBuilder;
        }
    }
}
