using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SystemTopupConditionSeedBuilder : ISeedBuilder
    {
        public int Priority => 170;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<SystemTopupConditionGroup>().HasData(
                new SystemTopupConditionGroup { Id = 1, SystemTopupId = 1, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 2, SystemTopupId = 2, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 3, SystemTopupId = 3, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 4, SystemTopupId = 4, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 5, SystemTopupId = 5, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 6, SystemTopupId = 6, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 7, SystemTopupId = 7, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 8, SystemTopupId = 8, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 9, SystemTopupId = 9, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 },
                new SystemTopupConditionGroup { Id = 10, SystemTopupId = 10, LogicalOperator = TopupLogicalOperator.And, DisplayOrder = 0 });

            modelBuilder.Entity<SystemTopupCondition>().HasData(
                new SystemTopupCondition { Id = 1, GroupId = 1, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 2, GroupId = 2, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 3, GroupId = 3, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 4, GroupId = 4, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 5, GroupId = 5, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 6, GroupId = 6, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 7, GroupId = 7, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 8, GroupId = 8, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 9, GroupId = 9, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 },
                new SystemTopupCondition { Id = 10, GroupId = 10, Field = TopupConditionField.SchoolingStatus, Operator = TopupConditionOperator.Equals, ValueText = "Enrolled", DisplayOrder = 0 });

            return modelBuilder;
        }
    }
}
