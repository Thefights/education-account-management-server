using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class AuthAccountSeedBuilder : ISeedBuilder
    {
        public int Priority => 20;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthAccount>().HasData(
                new AuthAccount
                {
                    Id = 1,
                    UserIdText = "Sterling",
                    Email = "phuckhang1088@gmail.com",
                    Status = AuthAccountStatus.Active,
                    FailedLoginCount = 0,
                    PasswordHash = "$2a$12$S8UUGws0L8l3mowPuKUsiOMNSgSQR4k9Jzic1gRdvXVKDTUvpxYty",
                    CreationDate = SeedConstants.CreatedAt
                },
                new AuthAccount { Id = 2, UserIdText = "user-002", Email = "binh.tran@example.com", Status = AuthAccountStatus.Active, FailedLoginCount = 0, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 3, UserIdText = "user-003", Email = "chau.le@example.com", Status = AuthAccountStatus.Active, FailedLoginCount = 1, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 4, UserIdText = "user-004", Email = "dung.pham@example.com", Status = AuthAccountStatus.Active, FailedLoginCount = 0, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 5, UserIdText = "user-005", Email = "giang.hoang@example.com", Status = AuthAccountStatus.Active, FailedLoginCount = 0, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 6, UserIdText = "user-006", Email = "hai.dang@example.com", Status = AuthAccountStatus.Inactive, FailedLoginCount = 0, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 7, UserIdText = "user-007", Email = "lan.bui@example.com", Status = AuthAccountStatus.Active, FailedLoginCount = 2, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 8, UserIdText = "user-008", Email = "minh.vo@example.com", Status = AuthAccountStatus.Active, FailedLoginCount = 0, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 9, UserIdText = "user-009", Email = "ngan.do@example.com", Status = AuthAccountStatus.Active, FailedLoginCount = 0, CreationDate = SeedConstants.CreatedAt },
                new AuthAccount { Id = 10, UserIdText = "user-010", Email = "phuc.ngo@example.com", Status = AuthAccountStatus.Inactive, FailedLoginCount = 0, CreationDate = SeedConstants.CreatedAt });

            return modelBuilder;
        }
    }
}
