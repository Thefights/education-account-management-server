using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class UserSeedBuilder : ISeedBuilder
    {
        public int Priority => 30;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, AuthAccountId = 1, FullName = "Nguyen Van An", PhoneNumber = "+84901234567", Gender = UserGender.Male, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 2, AuthAccountId = 2, FullName = "Tran Thi Binh", PhoneNumber = "+84901234568", Gender = UserGender.Female, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 3, AuthAccountId = 3, FullName = "Le Minh Chau", PhoneNumber = "+84901234569", Gender = UserGender.Other, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 4, AuthAccountId = 4, FullName = "Pham Quoc Dung", PhoneNumber = "+84901234570", Gender = UserGender.Male, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 5, AuthAccountId = 5, FullName = "Hoang Mai Giang", PhoneNumber = "+84901234571", Gender = UserGender.Female, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 6, AuthAccountId = 6, FullName = "Dang Thanh Hai", PhoneNumber = "+84901234572", Gender = UserGender.Male, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 7, AuthAccountId = 7, FullName = "Bui Ngoc Lan", PhoneNumber = "+84901234573", Gender = UserGender.Female, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 8, AuthAccountId = 8, FullName = "Vo Anh Minh", PhoneNumber = "+84901234574", Gender = UserGender.Male, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 9, AuthAccountId = 9, FullName = "Do Thuy Ngan", PhoneNumber = "+84901234575", Gender = UserGender.Female, CreationDate = SeedConstants.CreatedAt },
                new User { Id = 10, AuthAccountId = 10, FullName = "Ngo Duc Phuc", PhoneNumber = "+84901234576", Gender = UserGender.Male, CreationDate = SeedConstants.CreatedAt });

            return modelBuilder;
        }
    }
}
