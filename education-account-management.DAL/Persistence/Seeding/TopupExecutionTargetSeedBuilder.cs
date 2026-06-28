using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class TopupExecutionTargetSeedBuilder : ISeedBuilder
    {
        public int Priority => 210;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<TopupExecutionTarget>().HasData(
                new TopupExecutionTarget { Id = 1, TopupExecutionId = 1, EducationAccountId = 1, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 1), Amount = 85m, Status = TopupTargetStatus.Pending, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(1), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 2, TopupExecutionId = 2, EducationAccountId = 2, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 2), Amount = 95m, Status = TopupTargetStatus.Processing, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(2), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 3, TopupExecutionId = 3, EducationAccountId = 3, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 3), Amount = 105m, Status = TopupTargetStatus.Success, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = 3, CreatedAt = createdAt.AddDays(3), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 4, TopupExecutionId = 4, EducationAccountId = 4, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 4), Amount = 115m, Status = TopupTargetStatus.Failed, FailureReason = "Seed failure for review", MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(4), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 5, TopupExecutionId = 5, EducationAccountId = 5, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 5), Amount = 125m, Status = TopupTargetStatus.Pending, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(5), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 6, TopupExecutionId = 6, EducationAccountId = 6, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 6), Amount = 135m, Status = TopupTargetStatus.Processing, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(6), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 7, TopupExecutionId = 7, EducationAccountId = 7, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 7), Amount = 145m, Status = TopupTargetStatus.Success, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = 7, CreatedAt = createdAt.AddDays(7), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 8, TopupExecutionId = 8, EducationAccountId = 8, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 8), Amount = 155m, Status = TopupTargetStatus.Failed, FailureReason = "Seed failure for review", MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(8), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 9, TopupExecutionId = 9, EducationAccountId = 9, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 9), Amount = 165m, Status = TopupTargetStatus.Pending, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(9), CreatedBy = 2 },
                new TopupExecutionTarget { Id = 10, TopupExecutionId = 10, EducationAccountId = 10, AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 10), Amount = 175m, Status = TopupTargetStatus.Processing, FailureReason = null, MatchedConditionsSnapshot = "Seed matched condition", EducationCreditTransactionId = null, CreatedAt = createdAt.AddDays(10), CreatedBy = 2 });

            return modelBuilder;
        }
    }
}
