using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SystemTopupConditionSeedBuilder : ISeedBuilder
    {
        public int Priority => 100;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var groups = new List<SystemTopupConditionGroup>();
            var conditions = new List<SystemTopupCondition>();
            var groupId = 1;
            var conditionId = 1;

            for (var systemTopupId = 21; systemTopupId <= 50; systemTopupId++)
            {
                var rootId = groupId++;
                groups.Add(new SystemTopupConditionGroup
                {
                    Id = rootId,
                    SystemTopupId = systemTopupId,
                    LogicalOperator = TopupLogicalOperator.And,
                    DisplayOrder = 0
                });
                conditions.Add(new SystemTopupCondition
                {
                    Id = conditionId++,
                    GroupId = rootId,
                    Field = TopupConditionField.SchoolingStatus,
                    Operator = TopupConditionOperator.Equals,
                    ValueText = "Enrolled",
                    DisplayOrder = 0
                });
                conditions.Add(new SystemTopupCondition
                {
                    Id = conditionId++,
                    GroupId = rootId,
                    Field = TopupConditionField.Age,
                    Operator = TopupConditionOperator.Between,
                    ValueNumber = 16,
                    ValueNumberTo = 25,
                    DisplayOrder = 1
                });
            }

            modelBuilder.Entity<SystemTopupConditionGroup>().HasData(groups);
            modelBuilder.Entity<SystemTopupCondition>().HasData(conditions);
            return modelBuilder;
        }
    }
}