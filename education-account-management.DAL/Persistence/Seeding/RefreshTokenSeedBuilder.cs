using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class RefreshTokenSeedBuilder : ISeedBuilder
    {
        public int Priority => 270;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            // RefreshToken seed intentionally disabled for lean demo data.

            return modelBuilder;
        }
    }
}
