using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ChargeInstallmentSeedBuilder : ISeedBuilder
    {
        public int Priority => 120;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var installments = new List<ChargeInstallment>();
            
            for (int i = 1; i <= 50; i++)
            {
                decimal courseFee = 125m + (i * 5m);
                decimal miscFee = 23m;
                decimal gst = Math.Round((courseFee + miscFee) * 0.09m, 2);
                decimal grossAmount = courseFee + miscFee + gst;
                decimal subsidyAmount = i % 4 == 0 ? 30m : 0m;
                decimal netAmount = grossAmount - subsidyAmount;
                
                var status = ChargeInstallmentStatus.PendingPayment;
                if (i % 3 == 0) status = ChargeInstallmentStatus.Paid;
                else if (i % 3 == 2) status = ChargeInstallmentStatus.Overdue;
                
                DateTime? becameOverdueAt = status == ChargeInstallmentStatus.Overdue ? new DateTime(2026, 6, (i % 28) + 1, 0, 0, 0, DateTimeKind.Utc) : null;
                
                installments.Add(new ChargeInstallment
                {
                    Id = i,
                    ChargeId = i,
                    InstallmentNumber = 1,
                    Status = status,
                    DueDate = new DateTime(2026, 5, (i % 28) + 1, 0, 0, 0, DateTimeKind.Utc),
                    Amount = netAmount,
                    CreatedAt = createdAt,
                    BecameOverdueAt = becameOverdueAt
                });
            }
            
            // Sterling Quach special installment coverage (ChargeId=51, NetAmount=1110)
            installments.Add(new ChargeInstallment { Id = 51, ChargeId = 51, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt });
            installments.Add(new ChargeInstallment { Id = 52, ChargeId = 51, InstallmentNumber = 2, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc) });
            installments.Add(new ChargeInstallment { Id = 53, ChargeId = 51, InstallmentNumber = 3, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt });
            installments.Add(new ChargeInstallment { Id = 54, ChargeId = 51, InstallmentNumber = 4, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt });
            installments.Add(new ChargeInstallment { Id = 55, ChargeId = 51, InstallmentNumber = 5, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt });
            installments.Add(new ChargeInstallment { Id = 56, ChargeId = 51, InstallmentNumber = 6, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt });

            modelBuilder.Entity<ChargeInstallment>().HasData(installments);
            return modelBuilder;
        }
    }
}
