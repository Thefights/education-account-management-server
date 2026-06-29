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

            modelBuilder.Entity<ChargeInstallment>().HasData(
                new ChargeInstallment { Id = 1, ChargeId = 1, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 156m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 2, ChargeId = 2, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 2, 0, 0, 0, DateTimeKind.Utc), Amount = 162m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 3, ChargeId = 3, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc), Amount = 168m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 3, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 4, ChargeId = 4, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 4, 0, 0, 0, DateTimeKind.Utc), Amount = 174m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 5, ChargeId = 5, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc), Amount = 180m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 6, ChargeId = 6, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc), Amount = 186m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 6, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 7, ChargeId = 7, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 7, 0, 0, 0, DateTimeKind.Utc), Amount = 192m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 8, ChargeId = 8, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc), Amount = 198m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 9, ChargeId = 9, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc), Amount = 204m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 9, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 10, ChargeId = 10, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc), Amount = 210m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 11, ChargeId = 11, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 11, 0, 0, 0, DateTimeKind.Utc), Amount = 216m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 12, ChargeId = 12, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc), Amount = 222m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 12, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 13, ChargeId = 13, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 13, 0, 0, 0, DateTimeKind.Utc), Amount = 228m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 14, ChargeId = 14, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 14, 0, 0, 0, DateTimeKind.Utc), Amount = 234m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 15, ChargeId = 15, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc), Amount = 240m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 16, ChargeId = 16, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 16, 0, 0, 0, DateTimeKind.Utc), Amount = 246m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 17, ChargeId = 17, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc), Amount = 252m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 18, ChargeId = 18, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc), Amount = 258m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 19, ChargeId = 19, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 19, 0, 0, 0, DateTimeKind.Utc), Amount = 264m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 20, ChargeId = 20, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), Amount = 270m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 21, ChargeId = 21, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 21, 0, 0, 0, DateTimeKind.Utc), Amount = 276m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 22, ChargeId = 22, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 22, 0, 0, 0, DateTimeKind.Utc), Amount = 282m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 23, ChargeId = 23, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 23, 0, 0, 0, DateTimeKind.Utc), Amount = 288m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 24, ChargeId = 24, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 24, 0, 0, 0, DateTimeKind.Utc), Amount = 294m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 25, ChargeId = 25, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 25, 0, 0, 0, DateTimeKind.Utc), Amount = 300m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 26, ChargeId = 26, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 26, 0, 0, 0, DateTimeKind.Utc), Amount = 306m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 27, ChargeId = 27, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 27, 0, 0, 0, DateTimeKind.Utc), Amount = 312m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 28, ChargeId = 28, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 28, 0, 0, 0, DateTimeKind.Utc), Amount = 318m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 29, ChargeId = 29, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 324m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 30, ChargeId = 30, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 2, 0, 0, 0, DateTimeKind.Utc), Amount = 330m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 2, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 31, ChargeId = 31, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc), Amount = 336m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 32, ChargeId = 32, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 4, 0, 0, 0, DateTimeKind.Utc), Amount = 342m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 33, ChargeId = 33, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc), Amount = 348m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 34, ChargeId = 34, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc), Amount = 354m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 35, ChargeId = 35, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 7, 0, 0, 0, DateTimeKind.Utc), Amount = 360m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 36, ChargeId = 36, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 8, 0, 0, 0, DateTimeKind.Utc), Amount = 366m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 8, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 37, ChargeId = 37, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc), Amount = 372m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 38, ChargeId = 38, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc), Amount = 378m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 39, ChargeId = 39, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 11, 0, 0, 0, DateTimeKind.Utc), Amount = 384m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 40, ChargeId = 40, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc), Amount = 390m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 41, ChargeId = 41, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 13, 0, 0, 0, DateTimeKind.Utc), Amount = 396m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 42, ChargeId = 42, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 14, 0, 0, 0, DateTimeKind.Utc), Amount = 402m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 14, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 43, ChargeId = 43, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc), Amount = 408m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 44, ChargeId = 44, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 16, 0, 0, 0, DateTimeKind.Utc), Amount = 414m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 45, ChargeId = 45, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc), Amount = 420m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 17, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 46, ChargeId = 46, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc), Amount = 426m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 47, ChargeId = 47, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 19, 0, 0, 0, DateTimeKind.Utc), Amount = 432m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 48, ChargeId = 48, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc), Amount = 438m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 49, ChargeId = 49, InstallmentNumber = 1, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 5, 21, 0, 0, 0, DateTimeKind.Utc), Amount = 444m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 50, ChargeId = 50, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 5, 22, 0, 0, 0, DateTimeKind.Utc), Amount = 450m, CreatedAt = createdAt },
                // Sterling Quach installments on one charge for full status coverage
                new ChargeInstallment { Id = 51, ChargeId = 51, InstallmentNumber = 1, Status = ChargeInstallmentStatus.Paid, DueDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 52, ChargeId = 51, InstallmentNumber = 2, Status = ChargeInstallmentStatus.Overdue, DueDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt, BecameOverdueAt = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc) },
                new ChargeInstallment { Id = 53, ChargeId = 51, InstallmentNumber = 3, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 54, ChargeId = 51, InstallmentNumber = 4, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 55, ChargeId = 51, InstallmentNumber = 5, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt },
                new ChargeInstallment { Id = 56, ChargeId = 51, InstallmentNumber = 6, Status = ChargeInstallmentStatus.PendingPayment, DueDate = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc), Amount = 185m, CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
