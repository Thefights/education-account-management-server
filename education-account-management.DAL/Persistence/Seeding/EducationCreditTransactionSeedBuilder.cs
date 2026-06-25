using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationCreditTransactionSeedBuilder : ISeedBuilder
    {
        public int Priority => 150;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<EducationCreditTransaction>().HasData(
                new EducationCreditTransaction { Id = 1, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000001"), Type = EducationCreditTransactionType.Topup, Direction = EducationCreditTransactionDirection.Credit, Amount = 100m, BalanceBefore = 0m, BalanceAfter = 100m, Description = "Monthly Transport Allowance credited to the account.", EducationAccountId = 1, CreatedAt = createdAt },
                new EducationCreditTransaction { Id = 2, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000002"), Type = EducationCreditTransactionType.Topup, Direction = EducationCreditTransactionDirection.Credit, Amount = 100m, BalanceBefore = 1100m, BalanceAfter = 1200m, Description = "Monthly Meal Support Credit credited to the account.", EducationAccountId = 2, CreatedAt = createdAt },
                new EducationCreditTransaction { Id = 3, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000003"), Type = EducationCreditTransactionType.Topup, Direction = EducationCreditTransactionDirection.Credit, Amount = 200m, BalanceBefore = 1100m, BalanceAfter = 1300m, Description = "Post-Secondary Study Support credited to the account.", EducationAccountId = 3, CreatedAt = createdAt },
                new EducationCreditTransaction { Id = 4, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000004"), Type = EducationCreditTransactionType.ExpiredBalance, Direction = EducationCreditTransactionDirection.Debit, Amount = 30m, BalanceBefore = 30m, BalanceAfter = 0m, Description = "Education account balance expired at age 31.", EducationAccountId = 1, CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new EducationCreditTransaction { Id = 5, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000005"), Type = EducationCreditTransactionType.CourseFeePayment, Direction = EducationCreditTransactionDirection.Debit, Amount = 70m, BalanceBefore = 1200m, BalanceAfter = 1130m, Description = "Course fee payment for Software Foundations with C#.", EducationAccountId = 2, CreatedAt = new DateTime(2026, 1, 21, 0, 0, 0, DateTimeKind.Utc) },
                new EducationCreditTransaction { Id = 6, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000006"), Type = EducationCreditTransactionType.CourseFeePayment, Direction = EducationCreditTransactionDirection.Debit, Amount = 140m, BalanceBefore = 1300m, BalanceAfter = 1160m, Description = "Course fee payment for Professional Communication Lab.", EducationAccountId = 3, CreatedAt = new DateTime(2026, 1, 22, 0, 0, 0, DateTimeKind.Utc) },
                new EducationCreditTransaction { Id = 7, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000007"), Type = EducationCreditTransactionType.OutstandingAutoDeduction, Direction = EducationCreditTransactionDirection.Debit, Amount = 50m, BalanceBefore = 500m, BalanceAfter = 450m, Description = "March overdue charge adjustment.", EducationAccountId = 6, CreatedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
                new EducationCreditTransaction { Id = 8, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000008"), Type = EducationCreditTransactionType.OutstandingAutoDeduction, Direction = EducationCreditTransactionDirection.Debit, Amount = 70m, BalanceBefore = 70m, BalanceAfter = 0m, Description = "April overdue charge adjustment.", EducationAccountId = 8, CreatedAt = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc) },
                new EducationCreditTransaction { Id = 9, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000009"), Type = EducationCreditTransactionType.CourseFeePayment, Direction = EducationCreditTransactionDirection.Debit, Amount = 180m, BalanceBefore = 1400m, BalanceAfter = 1220m, Description = "Course fee payment for Sustainability Science Workshop.", EducationAccountId = 4, CreatedAt = new DateTime(2026, 1, 23, 0, 0, 0, DateTimeKind.Utc) },
                new EducationCreditTransaction { Id = 10, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000010"), Type = EducationCreditTransactionType.CourseFeePayment, Direction = EducationCreditTransactionDirection.Debit, Amount = 180m, BalanceBefore = 1500m, BalanceAfter = 1320m, Description = "Course fee payment for Digital Media Production.", EducationAccountId = 5, CreatedAt = new DateTime(2026, 1, 24, 0, 0, 0, DateTimeKind.Utc) });

            return modelBuilder;
        }
    }
}
