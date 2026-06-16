using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class SsoIdentitySeedBuilder : ISeedBuilder
{
    public int Priority => 30;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<SsoIdentity>().HasData(
            Enumerable.Range(1, 10).Select(id => new SsoIdentity
            {
                Id = id,
                Provider = id % 3 == 0 ? SsoProvider.AzureAD : SsoProvider.Singpass,
                ProviderUserId = id % 3 == 0 ? $"azure-user-{id:000}" : $"singpass-user-{id:000}",
                AuthAccountId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
