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
                Provider = id <= 3 ? SsoProvider.AzureAD : SsoProvider.Singpass,
                ProviderUserId = id switch
                {
                    1 => "ca993f32-b85d-4757-8482-99f49d58c32a",
                    <= 3 => $"azure-object-{id:000}",
                    _ => $"singpass-subject-{id:000}"
                },
                AuthAccountId = id,
                CreatedAt = createdAt
            }).ToArray());

        return modelBuilder;
    }

}
