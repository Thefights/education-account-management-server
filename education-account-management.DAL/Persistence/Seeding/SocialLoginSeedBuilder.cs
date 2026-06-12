using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class SocialLoginSeedBuilder : ISeedBuilder
    {
        public int Priority => 120;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SocialLogin>().HasData(
                new SocialLogin { Id = 1, AuthAccountId = 1, Provider = SocialLoginProvider.Google, ProviderUserId = "google-user-001", ProviderEmail = "an.nguyen@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 2, AuthAccountId = 2, Provider = SocialLoginProvider.Google, ProviderUserId = "google-user-002", ProviderEmail = "binh.tran@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 3, AuthAccountId = 3, Provider = SocialLoginProvider.Microsoft365, ProviderUserId = "ms-user-003", ProviderEmail = "chau.le@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 4, AuthAccountId = 4, Provider = SocialLoginProvider.Facebook, ProviderUserId = "fb-user-004", ProviderEmail = "dung.pham@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 5, AuthAccountId = 5, Provider = SocialLoginProvider.Google, ProviderUserId = "google-user-005", ProviderEmail = "giang.hoang@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 6, AuthAccountId = 6, Provider = SocialLoginProvider.Microsoft365, ProviderUserId = "ms-user-006", ProviderEmail = "hai.dang@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 7, AuthAccountId = 7, Provider = SocialLoginProvider.Google, ProviderUserId = "google-user-007", ProviderEmail = "lan.bui@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 8, AuthAccountId = 8, Provider = SocialLoginProvider.Facebook, ProviderUserId = "fb-user-008", ProviderEmail = "minh.vo@example.com", EmailVerified = false, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 9, AuthAccountId = 9, Provider = SocialLoginProvider.Microsoft365, ProviderUserId = "ms-user-009", ProviderEmail = "ngan.do@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt },
                new SocialLogin { Id = 10, AuthAccountId = 10, Provider = SocialLoginProvider.Google, ProviderUserId = "google-user-010", ProviderEmail = "phuc.ngo@example.com", EmailVerified = true, LinkedAt = SeedConstants.CreatedAt });

            return modelBuilder;
        }
    }
}
