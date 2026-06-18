using Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class EducationCreditTransactionSeedBuilder : ISeedBuilder
{
    public int Priority => 150;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<EducationCreditTransaction>().HasData(
            Enumerable.Range(1, 20).Select(id => new EducationCreditTransaction
            {
                Id = id,
                TransactionCode = SeedGuid(id),
                Type = id <= 10
                    ? id % 4 == 0 ? EducationCreditTransactionType.Adjustment : EducationCreditTransactionType.Topup
                    : EducationCreditTransactionType.Adjustment,
                Direction = EducationCreditTransactionDirection.Credit,
                Amount = id <= 10 ? 100m + id * 10m : 50m + (id - 10) * 5m,
                BalanceBefore = id <= 10 ? 1000m + id * 100m : 1200m + id * 50m,
                BalanceAfter = id <= 10 ? 1100m + id * 110m : 1250m + id * 55m,
                Description = id <= 10 ? $"Seed transaction {id:000}" : $"Seed adhoc transaction {id:000}",
                EducationAccountId = id <= 10 ? id : id - 10,
                CreatedAt = createdAt
            }).ToArray());

        modelBuilder.Entity<EducationCreditTransaction>().HasData(
            new EducationCreditTransaction { Id = 21, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000021"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 120m, BalanceBefore = 1100m, BalanceAfter = 980m, Description = "Payment transaction 001", EducationAccountId = 1, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 22, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000022"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 70m, BalanceBefore = 1200m, BalanceAfter = 1130m, Description = "Payment transaction 002", EducationAccountId = 2, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 23, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000023"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 140m, BalanceBefore = 1300m, BalanceAfter = 1160m, Description = "Payment transaction 003", EducationAccountId = 3, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 24, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000024"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 180m, BalanceBefore = 1400m, BalanceAfter = 1220m, Description = "Payment transaction 004", EducationAccountId = 4, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 25, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000025"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 180m, BalanceBefore = 1500m, BalanceAfter = 1320m, Description = "Payment transaction 005", EducationAccountId = 5, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 26, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000026"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 100m, BalanceBefore = 1600m, BalanceAfter = 1500m, Description = "Payment transaction 006", EducationAccountId = 6, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 27, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000027"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 200m, BalanceBefore = 1700m, BalanceAfter = 1500m, Description = "Payment transaction 007", EducationAccountId = 7, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 28, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000028"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 130m, BalanceBefore = 1800m, BalanceAfter = 1670m, Description = "Payment transaction 008", EducationAccountId = 8, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 29, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000029"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 250m, BalanceBefore = 1900m, BalanceAfter = 1650m, Description = "Payment transaction 009", EducationAccountId = 9, CreatedAt = createdAt },
            new EducationCreditTransaction { Id = 30, TransactionCode = Guid.Parse("00000000-0000-0000-0000-000000000030"), Type = EducationCreditTransactionType.CourseFee, Direction = EducationCreditTransactionDirection.Debit, Amount = 300m, BalanceBefore = 2000m, BalanceAfter = 1700m, Description = "Payment transaction 010", EducationAccountId = 10, CreatedAt = createdAt });

        return modelBuilder;
    }

    private static Guid SeedGuid(int id)
    {
        return Guid.Parse($"00000000-0000-0000-0000-{id:000000000000}");
    }

}
