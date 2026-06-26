using Enums;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class TopupExecutionTargetSeedBuilder : ISeedBuilder
    {
        public int Priority => 151;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt.AddDays(10);

            modelBuilder.Entity<TopupExecutionTarget>().HasData(
                new TopupExecutionTarget
                {
                    Id = 1,
                    TopupExecutionId = 1,
                    EducationAccountId = 1,
                    AccountNumber = SeedAccountNumberUtil.Generate(1),
                    Amount = 100m,
                    Status = TopupTargetStatus.Success,
                    EducationCreditTransactionId = 1,
                    CreatedAt = createdAt,
                    CreatedBy = 7
                },
                new TopupExecutionTarget
                {
                    Id = 2,
                    TopupExecutionId = 1,
                    EducationAccountId = 2,
                    AccountNumber = SeedAccountNumberUtil.Generate(2),
                    Amount = 100m,
                    Status = TopupTargetStatus.Failed,
                    FailureReason = "Account holder verification was pending.",
                    CreatedAt = createdAt,
                    CreatedBy = 7
                },
                new TopupExecutionTarget
                {
                    Id = 3,
                    TopupExecutionId = 2,
                    EducationAccountId = 3,
                    AccountNumber = SeedAccountNumberUtil.Generate(3),
                    Amount = 200m,
                    Status = TopupTargetStatus.Success,
                    EducationCreditTransactionId = 3,
                    CreatedAt = createdAt.AddDays(1)
                });

            return modelBuilder;
        }
    }
}
