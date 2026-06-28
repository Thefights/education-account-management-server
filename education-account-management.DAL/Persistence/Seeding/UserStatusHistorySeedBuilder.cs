using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class UserStatusHistorySeedBuilder : ISeedBuilder
    {
        public int Priority => 280;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            // UserStatusHistory seed intentionally disabled for lean demo data.

            return modelBuilder;
        }
    }
}
