using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class AuditLogSeedBuilder : ISeedBuilder
    {
        public int Priority => 110;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<AuditLog>().HasData(
               Enumerable.Range(1, 10).Select(id => new AuditLog
               {
                   Id = id,
                   Category = id % 3 == 0
                       ? AuditLogCategory.TopupConfig
                       : id % 2 == 0 ? AuditLogCategory.Transaction : AuditLogCategory.Security,
                   Action = id % 2 == 0 ? "CreditTransactionCreated" : "AdminLoginSucceeded",
                   IpAddress = $"127.0.0.{id}",
                   PayloadJson = $"{{\"seedId\":{id}}}",
                   OccurredAt = createdAt.AddMinutes(id),
                   ActorUserId = id
               }).ToArray());

            return modelBuilder;
        }
    }
}
