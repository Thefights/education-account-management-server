using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class OutboxMessageSeedBuilder : ISeedBuilder
    {
        public int Priority => 290;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            // OutboxMessage seed intentionally disabled for lean demo data.

            return modelBuilder;
        }
    }
}
