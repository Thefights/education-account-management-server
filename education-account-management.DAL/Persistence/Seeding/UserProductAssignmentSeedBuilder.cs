using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class UserProductAssignmentSeedBuilder : ISeedBuilder
    {
        public int Priority => 100;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProductAssignment>().HasData(
                new UserProductAssignment { UserId = 3, ProductId = 1, RoleInProduct = ProductAssignmentRole.Student },
                new UserProductAssignment { UserId = 4, ProductId = 2, RoleInProduct = ProductAssignmentRole.Staff },
                new UserProductAssignment { UserId = 5, ProductId = 3, RoleInProduct = ProductAssignmentRole.Trainer },
                new UserProductAssignment { UserId = 6, ProductId = 4, RoleInProduct = ProductAssignmentRole.Student },
                new UserProductAssignment { UserId = 8, ProductId = 5, RoleInProduct = ProductAssignmentRole.Staff },
                new UserProductAssignment { UserId = 9, ProductId = 10, RoleInProduct = ProductAssignmentRole.Trainer },
                new UserProductAssignment { UserId = 10, ProductId = 1, RoleInProduct = ProductAssignmentRole.Student });

            return modelBuilder;
        }
    }
}
