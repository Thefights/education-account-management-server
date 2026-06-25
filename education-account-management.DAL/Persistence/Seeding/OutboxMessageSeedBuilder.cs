using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class OutboxMessageSeedBuilder : ISeedBuilder
    {
        public int Priority => 190;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<OutboxMessage>().HasData(
                Enumerable.Range(1, 10).Select(id => new OutboxMessage
                {
                    Id = id,
                    Type = id % 2 == 0 ? "EmailNotification" : "AuditProjection",
                    PayloadJson = $"{{\"messageId\":{id}}}",
                    Status = id % 5 == 0
                        ? OutboxMessageStatus.Failed
                        : id % 2 == 0 ? OutboxMessageStatus.Completed : OutboxMessageStatus.Pending,
                    RetryCount = id % 3,
                    OccurredAt = createdAt.AddMinutes(id)
                }).ToArray());

            return modelBuilder;
        }

    }
}
