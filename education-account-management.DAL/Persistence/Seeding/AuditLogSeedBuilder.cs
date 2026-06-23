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

            var logs = Enumerable.Range(1, 10).Select(id => new AuditLog
            {
                Id = id,
                Category = id % 3 == 0
                       ? AuditLogCategory.Topup
                       : id % 2 == 0 ? AuditLogCategory.Transaction : AuditLogCategory.Security,
                Action = id % 2 == 0 ? "CreditTransactionCreated" : "AdminLoginSucceeded",
                IpAddress = $"127.0.0.{id}",
                OccurredAt = createdAt.AddMinutes(id),
                ActorUserId = id
            }).ToList();

            logs.Add(new AuditLog
            {
                Id = 11,
                Category = AuditLogCategory.AccountCreation,
                Action = "Auto Provisioning Sweep Completed",
                IpAddress = "127.0.0.1",
                OccurredAt = createdAt.AddHours(1),
                ActorUserId = 1
            });

            modelBuilder.Entity<AuditLog>().HasData(logs.ToArray());

            return modelBuilder;
        }
    }
}
