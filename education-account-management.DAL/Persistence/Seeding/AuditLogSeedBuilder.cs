using Models;
using Persistence.Seeding.Constants;
using System.Text.Json;

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
                       ? AuditLogCategory.TopupConfig
                       : id % 2 == 0 ? AuditLogCategory.Transaction : AuditLogCategory.Security,
                Action = id % 2 == 0 ? "CreditTransactionCreated" : "AdminLoginSucceeded",
                IpAddress = $"127.0.0.{id}",
                PayloadJson = $"{{\"seedId\":{id}}}",
                OccurredAt = createdAt.AddMinutes(id),
                ActorUserId = id
            }).ToList();

            var batchDate = DateOnly.FromDateTime(createdAt);
            logs.Add(new AuditLog
            {
                Id = 11,
                Category = AuditLogCategory.AccountCreation,
                Action = "Auto Provisioning Sweep Completed",
                IpAddress = "127.0.0.1",
                PayloadJson = JsonSerializer.Serialize(new
                {
                    BatchDate = batchDate.ToString("yyyy-MM-dd"),
                    StartedAt = "2026-01-01T00:00:03+08:00",
                    CompletedAt = "2026-01-01T00:04:15+08:00",
                    ExecutionTime = "00:04:12",
                    CreationMetrics = new
                    {
                        SuccessCount = 7,
                        DuplicateSkippedCount = 2,
                        FailedCount = 1
                    },
                    FailedCases = new[]
                    {
                        new
                        {
                            Nric = SingaporeNricUtil.Generate(15),
                            FullName = "Citizen 015",
                            Reason = "Manual review is required for the seeded batch record."
                        }
                    },
                    FailedCsvBase64 = string.Empty,
                    NotificationSummary = new { EmailSentCount = 7 },
                    ClosureBatchSummary = new
                    {
                        ClosedAccountsCount = 2,
                        TotalAmountTransferredToCpf = 450.00m,
                        NotificationsSent = new Dictionary<string, int>
                        {
                            ["t90"] = 3,
                            ["t30"] = 2,
                            ["t7"] = 1
                        }
                    }
                }),
                OccurredAt = createdAt.AddHours(1),
                ActorUserId = 1
            });

            modelBuilder.Entity<AuditLog>().HasData(logs.ToArray());

            return modelBuilder;
        }
    }
}
