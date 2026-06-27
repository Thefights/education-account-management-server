using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class AuditLogSeedBuilder : ISeedBuilder
    {
        public int Priority => 260;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<AuditLog>().HasData(
                new AuditLog { Id = 1, Category = AuditLogCategory.AccountCreation, Action = "SeedAction01", IpAddress = "127.0.0.2", OccurredAt = createdAt.AddHours(1), ActorUserId = 1 },
                new AuditLog { Id = 2, Category = AuditLogCategory.StatusChange, Action = "SeedAction02", IpAddress = "127.0.0.3", OccurredAt = createdAt.AddHours(2), ActorUserId = 2 },
                new AuditLog { Id = 3, Category = AuditLogCategory.Topup, Action = "SeedAction03", IpAddress = "127.0.0.4", OccurredAt = createdAt.AddHours(3), ActorUserId = 3 },
                new AuditLog { Id = 4, Category = AuditLogCategory.Security, Action = "SeedAction04", IpAddress = "127.0.0.5", OccurredAt = createdAt.AddHours(4), ActorUserId = 4 },
                new AuditLog { Id = 5, Category = AuditLogCategory.Transaction, Action = "SeedAction05", IpAddress = "127.0.0.6", OccurredAt = createdAt.AddHours(5), ActorUserId = 1 },
                new AuditLog { Id = 6, Category = AuditLogCategory.Billing, Action = "SeedAction06", IpAddress = "127.0.0.7", OccurredAt = createdAt.AddHours(6), ActorUserId = 2 },
                new AuditLog { Id = 7, Category = AuditLogCategory.AI, Action = "SeedAction07", IpAddress = "127.0.0.8", OccurredAt = createdAt.AddHours(7), ActorUserId = 3 },
                new AuditLog { Id = 8, Category = AuditLogCategory.AccountCreation, Action = "SeedAction08", IpAddress = "127.0.0.9", OccurredAt = createdAt.AddHours(8), ActorUserId = 4 },
                new AuditLog { Id = 9, Category = AuditLogCategory.StatusChange, Action = "SeedAction09", IpAddress = "127.0.0.10", OccurredAt = createdAt.AddHours(9), ActorUserId = 1 },
                new AuditLog { Id = 10, Category = AuditLogCategory.Topup, Action = "SeedAction10", IpAddress = "127.0.0.11", OccurredAt = createdAt.AddHours(10), ActorUserId = 2 },
                new AuditLog { Id = 11, Category = AuditLogCategory.Security, Action = "SeedAction11", IpAddress = "127.0.0.12", OccurredAt = createdAt.AddHours(11), ActorUserId = 3 },
                new AuditLog { Id = 12, Category = AuditLogCategory.Transaction, Action = "SeedAction12", IpAddress = "127.0.0.13", OccurredAt = createdAt.AddHours(12), ActorUserId = 4 },
                new AuditLog { Id = 13, Category = AuditLogCategory.Billing, Action = "SeedAction13", IpAddress = "127.0.0.14", OccurredAt = createdAt.AddHours(13), ActorUserId = 1 },
                new AuditLog { Id = 14, Category = AuditLogCategory.AI, Action = "SeedAction14", IpAddress = "127.0.0.15", OccurredAt = createdAt.AddHours(14), ActorUserId = 2 },
                new AuditLog { Id = 15, Category = AuditLogCategory.AccountCreation, Action = "SeedAction15", IpAddress = "127.0.0.16", OccurredAt = createdAt.AddHours(15), ActorUserId = 3 },
                new AuditLog { Id = 16, Category = AuditLogCategory.StatusChange, Action = "SeedAction16", IpAddress = "127.0.0.17", OccurredAt = createdAt.AddHours(16), ActorUserId = 4 },
                new AuditLog { Id = 17, Category = AuditLogCategory.Topup, Action = "SeedAction17", IpAddress = "127.0.0.18", OccurredAt = createdAt.AddHours(17), ActorUserId = 1 },
                new AuditLog { Id = 18, Category = AuditLogCategory.Security, Action = "SeedAction18", IpAddress = "127.0.0.19", OccurredAt = createdAt.AddHours(18), ActorUserId = 2 },
                new AuditLog { Id = 19, Category = AuditLogCategory.Transaction, Action = "SeedAction19", IpAddress = "127.0.0.20", OccurredAt = createdAt.AddHours(19), ActorUserId = 3 },
                new AuditLog { Id = 20, Category = AuditLogCategory.Billing, Action = "SeedAction20", IpAddress = "127.0.0.21", OccurredAt = createdAt.AddHours(20), ActorUserId = 4 },
                new AuditLog { Id = 21, Category = AuditLogCategory.AI, Action = "SeedAction21", IpAddress = "127.0.0.22", OccurredAt = createdAt.AddHours(21), ActorUserId = 1 },
                new AuditLog { Id = 22, Category = AuditLogCategory.AccountCreation, Action = "SeedAction22", IpAddress = "127.0.0.23", OccurredAt = createdAt.AddHours(22), ActorUserId = 2 },
                new AuditLog { Id = 23, Category = AuditLogCategory.StatusChange, Action = "SeedAction23", IpAddress = "127.0.0.24", OccurredAt = createdAt.AddHours(23), ActorUserId = 3 },
                new AuditLog { Id = 24, Category = AuditLogCategory.Topup, Action = "SeedAction24", IpAddress = "127.0.0.25", OccurredAt = createdAt.AddHours(24), ActorUserId = 4 },
                new AuditLog { Id = 25, Category = AuditLogCategory.Security, Action = "SeedAction25", IpAddress = "127.0.0.26", OccurredAt = createdAt.AddHours(25), ActorUserId = 1 },
                new AuditLog { Id = 26, Category = AuditLogCategory.Transaction, Action = "SeedAction26", IpAddress = "127.0.0.27", OccurredAt = createdAt.AddHours(26), ActorUserId = 2 },
                new AuditLog { Id = 27, Category = AuditLogCategory.Billing, Action = "SeedAction27", IpAddress = "127.0.0.28", OccurredAt = createdAt.AddHours(27), ActorUserId = 3 },
                new AuditLog { Id = 28, Category = AuditLogCategory.AI, Action = "SeedAction28", IpAddress = "127.0.0.29", OccurredAt = createdAt.AddHours(28), ActorUserId = 4 },
                new AuditLog { Id = 29, Category = AuditLogCategory.AccountCreation, Action = "SeedAction29", IpAddress = "127.0.0.30", OccurredAt = createdAt.AddHours(29), ActorUserId = 1 },
                new AuditLog { Id = 30, Category = AuditLogCategory.StatusChange, Action = "SeedAction30", IpAddress = "127.0.0.31", OccurredAt = createdAt.AddHours(30), ActorUserId = 2 },
                new AuditLog { Id = 31, Category = AuditLogCategory.Topup, Action = "SeedAction31", IpAddress = "127.0.0.32", OccurredAt = createdAt.AddHours(31), ActorUserId = 3 },
                new AuditLog { Id = 32, Category = AuditLogCategory.Security, Action = "SeedAction32", IpAddress = "127.0.0.33", OccurredAt = createdAt.AddHours(32), ActorUserId = 4 },
                new AuditLog { Id = 33, Category = AuditLogCategory.Transaction, Action = "SeedAction33", IpAddress = "127.0.0.34", OccurredAt = createdAt.AddHours(33), ActorUserId = 1 },
                new AuditLog { Id = 34, Category = AuditLogCategory.Billing, Action = "SeedAction34", IpAddress = "127.0.0.35", OccurredAt = createdAt.AddHours(34), ActorUserId = 2 },
                new AuditLog { Id = 35, Category = AuditLogCategory.AI, Action = "SeedAction35", IpAddress = "127.0.0.36", OccurredAt = createdAt.AddHours(35), ActorUserId = 3 },
                new AuditLog { Id = 36, Category = AuditLogCategory.AccountCreation, Action = "SeedAction36", IpAddress = "127.0.0.37", OccurredAt = createdAt.AddHours(36), ActorUserId = 4 },
                new AuditLog { Id = 37, Category = AuditLogCategory.StatusChange, Action = "SeedAction37", IpAddress = "127.0.0.38", OccurredAt = createdAt.AddHours(37), ActorUserId = 1 },
                new AuditLog { Id = 38, Category = AuditLogCategory.Topup, Action = "SeedAction38", IpAddress = "127.0.0.39", OccurredAt = createdAt.AddHours(38), ActorUserId = 2 },
                new AuditLog { Id = 39, Category = AuditLogCategory.Security, Action = "SeedAction39", IpAddress = "127.0.0.40", OccurredAt = createdAt.AddHours(39), ActorUserId = 3 },
                new AuditLog { Id = 40, Category = AuditLogCategory.Transaction, Action = "SeedAction40", IpAddress = "127.0.0.41", OccurredAt = createdAt.AddHours(40), ActorUserId = 4 },
                new AuditLog { Id = 41, Category = AuditLogCategory.Billing, Action = "SeedAction41", IpAddress = "127.0.0.42", OccurredAt = createdAt.AddHours(41), ActorUserId = 1 },
                new AuditLog { Id = 42, Category = AuditLogCategory.AI, Action = "SeedAction42", IpAddress = "127.0.0.43", OccurredAt = createdAt.AddHours(42), ActorUserId = 2 },
                new AuditLog { Id = 43, Category = AuditLogCategory.AccountCreation, Action = "SeedAction43", IpAddress = "127.0.0.44", OccurredAt = createdAt.AddHours(43), ActorUserId = 3 },
                new AuditLog { Id = 44, Category = AuditLogCategory.StatusChange, Action = "SeedAction44", IpAddress = "127.0.0.45", OccurredAt = createdAt.AddHours(44), ActorUserId = 4 },
                new AuditLog { Id = 45, Category = AuditLogCategory.Topup, Action = "SeedAction45", IpAddress = "127.0.0.46", OccurredAt = createdAt.AddHours(45), ActorUserId = 1 },
                new AuditLog { Id = 46, Category = AuditLogCategory.Security, Action = "SeedAction46", IpAddress = "127.0.0.47", OccurredAt = createdAt.AddHours(46), ActorUserId = 2 },
                new AuditLog { Id = 47, Category = AuditLogCategory.Transaction, Action = "SeedAction47", IpAddress = "127.0.0.48", OccurredAt = createdAt.AddHours(47), ActorUserId = 3 },
                new AuditLog { Id = 48, Category = AuditLogCategory.Billing, Action = "SeedAction48", IpAddress = "127.0.0.49", OccurredAt = createdAt.AddHours(48), ActorUserId = 4 },
                new AuditLog { Id = 49, Category = AuditLogCategory.AI, Action = "SeedAction49", IpAddress = "127.0.0.50", OccurredAt = createdAt.AddHours(49), ActorUserId = 1 },
                new AuditLog { Id = 50, Category = AuditLogCategory.AccountCreation, Action = "SeedAction50", IpAddress = "127.0.0.51", OccurredAt = createdAt.AddHours(50), ActorUserId = 2 });

            return modelBuilder;
        }
    }
}
