using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationCreditTransactionSeedBuilder : ISeedBuilder
    {
        public int Priority => 130;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var transactions = new List<EducationCreditTransaction>();
            
            // Generate initial Topups for all accounts (1-50)
            for (int i = 1; i <= 50; i++)
            {
                transactions.Add(new EducationCreditTransaction
                {
                    Id = 100 + i, // Offset topup IDs to avoid collision with payment debits
                    TransactionCode = Guid.Parse($"00000000-0000-0000-0000-{(100 + i):D12}"),
                    Type = EducationCreditTransactionType.Topup,
                    Direction = EducationCreditTransactionDirection.Credit,
                    Amount = 1000m,
                    BalanceBefore = 0m,
                    BalanceAfter = 1000m,
                    Description = "Initial system top-up",
                    EducationAccountId = i,
                    CreatedAt = createdAt
                });
            }

            // Generate Debits for successful payments using EducationBalance (i % 2 != 0 and i % 3 == 0)
            for (int i = 1; i <= 50; i++)
            {
                if (i % 3 == 0 && i % 2 != 0) 
                {
                    decimal courseFee = 125m + (i * 5m);
                    decimal miscFee = 23m;
                    decimal gst = Math.Round((courseFee + miscFee) * 0.09m, 2);
                    decimal grossAmount = courseFee + miscFee + gst;
                    decimal subsidyAmount = i % 4 == 0 ? 30m : 0m;
                    decimal netAmount = grossAmount - subsidyAmount;
                    
                    transactions.Add(new EducationCreditTransaction
                    {
                        Id = i,
                        TransactionCode = Guid.Parse($"00000000-0000-0000-0000-{i:D12}"),
                        Type = EducationCreditTransactionType.CourseFeePayment,
                        Direction = EducationCreditTransactionDirection.Debit,
                        Amount = netAmount,
                        BalanceBefore = 1000m,
                        BalanceAfter = 1000m - netAmount,
                        Description = "Course fee payment deduction",
                        EducationAccountId = i,
                        CreatedAt = createdAt.AddDays(2)
                    });
                }
            }

            // Sterling Quach (Id 1) additional debit
            transactions.Add(new EducationCreditTransaction { Id = 51, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000051"), Type = EducationCreditTransactionType.CourseFeePayment, Direction = EducationCreditTransactionDirection.Debit, Amount = 185m, BalanceBefore = 1000m, BalanceAfter = 815m, Description = "Course fee payment for Creative Thinking Cohort 51", EducationAccountId = 1, CreatedAt = createdAt.AddDays(30) });

            transactions.Add(new EducationCreditTransaction { Id = 203, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000203"), Type = EducationCreditTransactionType.Topup, Direction = EducationCreditTransactionDirection.Credit, Amount = 105m, BalanceBefore = 1000m, BalanceAfter = 1105m, Description = "Manual Account Adjustment", EducationAccountId = 3, CreatedAt = createdAt.AddDays(3) });
            transactions.Add(new EducationCreditTransaction { Id = 207, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000207"), Type = EducationCreditTransactionType.Topup, Direction = EducationCreditTransactionDirection.Credit, Amount = 145m, BalanceBefore = 1000m, BalanceAfter = 1145m, Description = "STEM Enrichment Credit", EducationAccountId = 7, CreatedAt = createdAt.AddDays(7) });

            modelBuilder.Entity<EducationCreditTransaction>().HasData(transactions);
            return modelBuilder;
        }
    }
}
