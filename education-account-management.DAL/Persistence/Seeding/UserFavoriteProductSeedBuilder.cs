using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public class UserFavoriteProductSeedBuilder : ISeedBuilder
    {
        public int Priority => 90;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFavoriteProduct>().HasData(
                new UserFavoriteProduct { UserId = 1, ProductId = 1 },
                new UserFavoriteProduct { UserId = 2, ProductId = 2 },
                new UserFavoriteProduct { UserId = 3, ProductId = 3 },
                new UserFavoriteProduct { UserId = 4, ProductId = 4 },
                new UserFavoriteProduct { UserId = 5, ProductId = 5 },
                new UserFavoriteProduct { UserId = 6, ProductId = 6 },
                new UserFavoriteProduct { UserId = 7, ProductId = 7 },
                new UserFavoriteProduct { UserId = 8, ProductId = 8 },
                new UserFavoriteProduct { UserId = 9, ProductId = 10 },
                new UserFavoriteProduct { UserId = 10, ProductId = 1 });

            return modelBuilder;
        }
    }
}
