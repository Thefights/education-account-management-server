using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class UserStatusHistorySeedBuilder : ISeedBuilder
    {
        public int Priority => 108;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStatusHistory>().HasData(
                new UserStatusHistory
                {
                    Id = 1,
                    UserId = 5,
                    PreviousStatus = UserStatus.Active,
                    NewStatus = UserStatus.Inactive,
                    Reason = "Seeded administrative suspension.",
                    ChangedAt = SeedDataConstants.CreatedAt.AddDays(30),
                    ChangedByUserId = 1
                });

            return modelBuilder;
        }
    }
}
