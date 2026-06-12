using education_account_management.DAL.Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class UserRoleSeedBuilder : ISeedBuilder
    {
        public int Priority => 80;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = 1, RoleId = 1 },
                new UserRole { UserId = 1, RoleId = 2 },
                new UserRole { UserId = 2, RoleId = 2 },
                new UserRole { UserId = 3, RoleId = 2 },
                new UserRole { UserId = 4, RoleId = 2 },
                new UserRole { UserId = 5, RoleId = 2 },
                new UserRole { UserId = 6, RoleId = 2 },
                new UserRole { UserId = 7, RoleId = 2 },
                new UserRole { UserId = 8, RoleId = 2 },
                new UserRole { UserId = 9, RoleId = 2 },
                new UserRole { UserId = 10, RoleId = 2 });

            return modelBuilder;
        }
    }
}
