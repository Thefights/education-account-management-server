using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class RefreshTokenSeedBuilder : ISeedBuilder
{
    public int Priority => 40;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<RefreshToken>().HasData(
            Enumerable.Range(1, 10).Select(id => new RefreshToken
            {
                Id = id,
                TokenHash = $"refresh-token-hash-{id:000}",
                ExpiresAt = createdAt.AddDays(30 + id),
                RevokedAt = id % 4 == 0 ? createdAt.AddDays(id) : null,
                UserId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
