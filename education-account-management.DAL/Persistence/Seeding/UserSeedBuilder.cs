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

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = createdAt.AddDays(1), CreatedAt = createdAt },
                new User { Id = 2, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = createdAt.AddDays(2), CreatedAt = createdAt },
                new User { Id = 3, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = createdAt.AddDays(3), CreatedAt = createdAt },
                new User { Id = 4, Role = UserRole.AccountHolder, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = createdAt.AddDays(4), CitizenId = 1, CreatedAt = createdAt },
                new User { Id = 5, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 6, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 7, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 8, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 9, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 10, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 11, Role = UserRole.SystemAdmin, Status = UserStatus.Inactive, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 12, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 13, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 14, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 15, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 16, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 17, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 18, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 19, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 20, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 21, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 22, Role = UserRole.SchoolAdmin, Status = UserStatus.Inactive, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 23, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 24, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 25, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 26, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 27, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 28, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 29, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 30, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 31, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 32, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 33, Role = UserRole.FinanceAdmin, Status = UserStatus.Inactive, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 34, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 35, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 36, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 37, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 38, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 39, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 40, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 41, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 42, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 43, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 44, Role = UserRole.SystemAdmin, Status = UserStatus.Inactive, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 45, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 46, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 47, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 48, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 49, Role = UserRole.SchoolAdmin, Status = UserStatus.Active, FailedLoginCount = 1, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 50, Role = UserRole.SystemAdmin, Status = UserStatus.Active, FailedLoginCount = 2, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt },
                new User { Id = 51, Role = UserRole.FinanceAdmin, Status = UserStatus.Active, FailedLoginCount = 0, LockedUntil = null, LastLoginAt = null, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
