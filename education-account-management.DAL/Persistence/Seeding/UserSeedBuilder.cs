using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class UserSeedBuilder : ISeedBuilder
    {
        public int Priority => 20;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            var users = new List<User>();
            
            // System Admins (1-3, 5-6)
            int[] sysAdminIds = { 1, 5, 8, 11, 14 };
            foreach (var id in sysAdminIds)
            {
                users.Add(new User { Id = id, Role = UserRole.SystemAdmin, Status = id % 5 == 0 ? UserStatus.Inactive : UserStatus.Active, FailedLoginCount = 0, LastLoginAt = createdAt.AddDays(id), CreatedAt = createdAt });
            }
            
            // Finance Admins
            int[] financeAdminIds = { 2, 6, 9, 12, 15 };
            foreach (var id in financeAdminIds)
            {
                users.Add(new User { Id = id, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LastLoginAt = createdAt.AddDays(id), CreatedAt = createdAt });
            }

            // School Admins
            int[] schoolAdminIds = { 3, 7, 10, 13, 16, 17, 18, 19, 20, 21 };
            foreach (var id in schoolAdminIds)
            {
                users.Add(new User { Id = id, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LastLoginAt = createdAt.AddDays(id), CreatedAt = createdAt });
            }

            // The main test AccountHolder MUST be Id = 4 (for SsoIdentity "singpass-subject-004")
            users.Add(new User { Id = 4, Role = UserRole.AccountHolder, Status = UserStatus.Active, FailedLoginCount = 0, LastLoginAt = createdAt.AddDays(4), CitizenId = 1, CreatedAt = createdAt });

            // Account Holders (22-50) linked to Citizens 2-30
            for (int i = 22; i <= 50; i++)
            {
                users.Add(new User { Id = i, Role = UserRole.AccountHolder, Status = UserStatus.Active, FailedLoginCount = 0, LastLoginAt = createdAt.AddDays(i), CitizenId = i - 20, CreatedAt = createdAt });
            }

            modelBuilder.Entity<User>().HasData(users);

            return modelBuilder;
        }
    }
}
