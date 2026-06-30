using Models;
using Persistence.Seeding.Constants;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistence.Seeding
{
    public sealed class ManagementActionLogSeedBuilder : ISeedBuilder
    {
        public int Priority => 200;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<ManagementActionLog>().HasData(
                new ManagementActionLog
                {
                    Id = 1,
                    BatchId = Guid.NewGuid(),
                    EntityType = ManagementActionEntityType.FasScheme,
                    EntityId = 1,
                    Action = ManagementAction.Publish,
                    PreviousStatus = "Draft",
                    NewStatus = "Active",
                    Reason = "Manual review passed, scheme published.",
                    ActorUserId = 1,
                    OccurredAt = createdAt.AddDays(1),
                    IpAddress = "127.0.0.1"
                },
                new ManagementActionLog
                {
                    Id = 2,
                    BatchId = Guid.NewGuid(),
                    EntityType = ManagementActionEntityType.Course,
                    EntityId = 5,
                    Action = ManagementAction.Close,
                    PreviousStatus = "InProgress",
                    NewStatus = "Closed",
                    Reason = "Course duration finished.",
                    ActorUserId = 1,
                    OccurredAt = createdAt.AddDays(2),
                    IpAddress = "127.0.0.1"
                }
            );

            return modelBuilder;
        }
    }
}
