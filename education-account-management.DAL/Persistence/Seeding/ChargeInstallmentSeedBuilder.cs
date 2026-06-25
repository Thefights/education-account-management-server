using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class ChargeInstallmentSeedBuilder : ISeedBuilder
    {
        public int Priority => 135;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<ChargeInstallment>().HasData(
                new ChargeInstallment { Id = 1, ChargeId = 1, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 120m, PaidAmount = 120m, RemainingAmount = 0m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 2, ChargeId = 2, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Unpaid, DueDate = new DateTime(2026, 8, 2, 0, 0, 0, DateTimeKind.Utc), Amount = 70m, PaidAmount = 0m, RemainingAmount = 70m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 3, ChargeId = 3, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Unpaid, DueDate = new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), Amount = 80m, PaidAmount = 0m, RemainingAmount = 80m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 4, ChargeId = 4, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc), Amount = 180m, PaidAmount = 180m, RemainingAmount = 0m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 5, ChargeId = 5, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Unpaid, DueDate = new DateTime(2026, 8, 5, 0, 0, 0, DateTimeKind.Utc), Amount = 100m, PaidAmount = 0m, RemainingAmount = 100m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 6, ChargeId = 6, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc), Amount = 110m, PaidAmount = 0m, RemainingAmount = 110m, BecameOverdueAt = new DateTime(2026, 5, 7, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new ChargeInstallment { Id = 7, ChargeId = 7, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Unpaid, DueDate = new DateTime(2026, 5, 7, 0, 0, 0, DateTimeKind.Utc), Amount = 120m, PaidAmount = 0m, RemainingAmount = 120m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 8, ChargeId = 8, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc), Amount = 130m, PaidAmount = 0m, RemainingAmount = 130m, BecameOverdueAt = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new ChargeInstallment { Id = 9, ChargeId = 9, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 2, 9, 0, 0, 0, DateTimeKind.Utc), Amount = 140m, PaidAmount = 0m, RemainingAmount = 140m, BecameOverdueAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new ChargeInstallment { Id = 10, ChargeId = 10, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc), Amount = 150m, PaidAmount = 0m, RemainingAmount = 150m, BecameOverdueAt = new DateTime(2026, 2, 11, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}

