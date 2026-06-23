using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountStatusHistorySeedBuilder : ISeedBuilder
    {
        public int Priority => 107;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var changedAt = SeedDataConstants.CreatedAt.AddDays(120);

            modelBuilder.Entity<EducationAccountStatusHistory>().HasData(
                new EducationAccountStatusHistory
                {
                    Id = 1,
                    EducationAccountId = 1,
                    PreviousStatus = EducationAccountStatus.Active,
                    NewStatus = EducationAccountStatus.Closed,
                    Reason = "Education account closed by the scheduled sweep.",
                    ChangedAt = changedAt,
                    ChangedByUserId = 1
                },
                new EducationAccountStatusHistory
                {
                    Id = 2,
                    EducationAccountId = 6,
                    PreviousStatus = EducationAccountStatus.Active,
                    NewStatus = EducationAccountStatus.Extended,
                    Reason = "Education account extended because outstanding charges remain.",
                    ChangedAt = changedAt.AddDays(1),
                    ChangedByUserId = 1
                });

            return modelBuilder;
        }
    }
}