using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class AuditLogSeedBuilder : ISeedBuilder
    {
        public int Priority => 110;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().HasData(
                new AuditLog { Id = 1, ActorUserId = 1, ActorFullName = "An Nguyen", ActorUserIdText = "admin", Category = AuditLogCategory.Authentication, Action = AuditLogAction.Login, Object = "AuthAccount:1:admin", IpAddress = "127.0.0.1", CreatedAt = SeedConstants.CreatedAt.AddMinutes(1) },
                new AuditLog { Id = 2, ActorUserId = 1, ActorFullName = "An Nguyen", ActorUserIdText = "admin", Category = AuditLogCategory.AccountManagement, Action = AuditLogAction.CreateAccount, Object = "AuthAccount:2:tenant.user", IpAddress = "127.0.0.1", CreatedAt = SeedConstants.CreatedAt.AddMinutes(2) },
                new AuditLog { Id = 5, ActorUserId = 1, ActorFullName = "An Nguyen", ActorUserIdText = "admin", Category = AuditLogCategory.Product, Action = AuditLogAction.CreateProduct, Object = "Product:10:MOS Audit", IpAddress = "127.0.0.1", CreatedAt = SeedConstants.CreatedAt.AddMinutes(5) },
                new AuditLog { Id = 7, ActorUserId = 1, ActorFullName = "An Nguyen", ActorUserIdText = "admin", Category = AuditLogCategory.AuditLog, Action = AuditLogAction.ViewAuditLogs, Object = "AuditLog:Pagination", IpAddress = "127.0.0.1", CreatedAt = SeedConstants.CreatedAt.AddMinutes(7) });

            return modelBuilder;
        }
    }
}
