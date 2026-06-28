using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class SsoIdentitySeedBuilder : ISeedBuilder
    {
        public int Priority => 30;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<SsoIdentity>().HasData(
                new SsoIdentity { Id = 1, Provider = SsoProvider.AzureAD, ProviderUserId = "5fc549a1-ee08-4273-9497-27607842e1f9", UserId = 1, CreatedAt = createdAt },
                new SsoIdentity { Id = 2, Provider = SsoProvider.AzureAD, ProviderUserId = "1fedf576-1c66-4742-881b-f7a456b2b027", UserId = 2, CreatedAt = createdAt },
                new SsoIdentity { Id = 3, Provider = SsoProvider.AzureAD, ProviderUserId = "78ce9568-1d38-44aa-a9c5-ea50293934de", UserId = 3, CreatedAt = createdAt },
                new SsoIdentity { Id = 4, Provider = SsoProvider.Singpass, ProviderUserId = "singpass-subject-004", UserId = 4, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
