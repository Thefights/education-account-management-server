using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountStatusHistorySeedBuilder : ISeedBuilder
    {
        public int Priority => 230;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<EducationAccountStatusHistory>().HasData(
                new EducationAccountStatusHistory { Id = 1, EducationAccountId = 1, PreviousStatus = EducationAccountStatus.Active, NewStatus = EducationAccountStatus.Closed, Reason = "Account status updated after lifecycle review", ChangedAt = createdAt.AddDays(1), ChangedByUserId = 1 },
                new EducationAccountStatusHistory { Id = 2, EducationAccountId = 2, PreviousStatus = EducationAccountStatus.Active, NewStatus = EducationAccountStatus.Extended, Reason = "Account status updated after lifecycle review", ChangedAt = createdAt.AddDays(2), ChangedByUserId = 1 },
                new EducationAccountStatusHistory { Id = 3, EducationAccountId = 3, PreviousStatus = EducationAccountStatus.Active, NewStatus = EducationAccountStatus.Closed, Reason = "Account status updated after lifecycle review", ChangedAt = createdAt.AddDays(3), ChangedByUserId = 1 },
                new EducationAccountStatusHistory { Id = 4, EducationAccountId = 4, PreviousStatus = EducationAccountStatus.Active, NewStatus = EducationAccountStatus.Extended, Reason = "Account status updated after lifecycle review", ChangedAt = createdAt.AddDays(4), ChangedByUserId = 1 },
                new EducationAccountStatusHistory { Id = 5, EducationAccountId = 5, PreviousStatus = EducationAccountStatus.Active, NewStatus = EducationAccountStatus.Closed, Reason = "Account status updated after lifecycle review", ChangedAt = createdAt.AddDays(5), ChangedByUserId = 1 },
                new EducationAccountStatusHistory { Id = 6, EducationAccountId = 6, PreviousStatus = EducationAccountStatus.Active, NewStatus = EducationAccountStatus.Extended, Reason = "Account status updated after lifecycle review", ChangedAt = createdAt.AddDays(6), ChangedByUserId = 1 });

            return modelBuilder;
        }
    }
}
