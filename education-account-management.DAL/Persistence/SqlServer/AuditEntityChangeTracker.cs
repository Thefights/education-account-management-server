using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.SqlServer
{
    public static class AuditEntityChangeTracker
    {
        public static void Apply(ChangeTracker changeTracker, int? currentUserId, DateTime now)
        {
            foreach (var entry in changeTracker.Entries<AuditEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = currentUserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = currentUserId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = now;
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = currentUserId;
                        break;
                }
            }
        }
    }
}
