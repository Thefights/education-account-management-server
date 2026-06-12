using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class RoleSeedBuilder : ISeedBuilder
    {
        public int Priority => 10;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", CreationDate = SeedConstants.CreatedAt },
                new Role { Id = 2, Name = "TenantUser", CreationDate = SeedConstants.CreatedAt });

            return modelBuilder;
        }
    }
}
