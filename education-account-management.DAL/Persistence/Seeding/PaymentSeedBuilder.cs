using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class PaymentSeedBuilder : ISeedBuilder
    {
        public int Priority => 160;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;

            modelBuilder.Entity<Payment>().HasData(
                new Payment { Id = 1, EducationCreditTransactionId = 5, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(2), CitizenNricSnapshot = SingaporeNricUtil.Generate(2), CitizenFullNameSnapshot = "Amelia Tan", TotalAmount = 70m, PaidAt = new DateTime(2026, 1, 21, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new Payment { Id = 2, EducationCreditTransactionId = 6, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(3), CitizenNricSnapshot = SingaporeNricUtil.Generate(3), CitizenFullNameSnapshot = "Marcus Lim", TotalAmount = 140m, PaidAt = new DateTime(2026, 1, 22, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new Payment { Id = 3, EducationCreditTransactionId = 9, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(4), CitizenNricSnapshot = SingaporeNricUtil.Generate(4), CitizenFullNameSnapshot = "Priya Nair", TotalAmount = 180m, PaidAt = new DateTime(2026, 1, 23, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new Payment { Id = 4, EducationCreditTransactionId = 10, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(5), CitizenNricSnapshot = SingaporeNricUtil.Generate(5), CitizenFullNameSnapshot = "Ethan Koh", TotalAmount = 180m, PaidAt = new DateTime(2026, 1, 24, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new Payment { Id = 5, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(1), CitizenNricSnapshot = SingaporeNricUtil.Generate(1), CitizenFullNameSnapshot = "Sterling Quach", TotalAmount = 120m, PaidAt = new DateTime(2026, 1, 25, 0, 0, 0, DateTimeKind.Utc), ExternalReference = "PAY-ONLINE-0005", CreatedAt = createdAt },
                new Payment { Id = 6, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(6), CitizenNricSnapshot = SingaporeNricUtil.Generate(6), CitizenFullNameSnapshot = "Hannah Lee", TotalAmount = 100m, PaidAt = new DateTime(2026, 1, 26, 0, 0, 0, DateTimeKind.Utc), ExternalReference = "PAY-ONLINE-0006", CreatedAt = createdAt },
                new Payment { Id = 7, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(7), CitizenNricSnapshot = SingaporeNricUtil.Generate(7), CitizenFullNameSnapshot = "Daniel Wong", TotalAmount = 200m, PaidAt = new DateTime(2026, 1, 27, 0, 0, 0, DateTimeKind.Utc), ExternalReference = "PAY-ONLINE-0007", CreatedAt = createdAt },
                new Payment { Id = 8, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(8), CitizenNricSnapshot = SingaporeNricUtil.Generate(8), CitizenFullNameSnapshot = "Sofia Chen", TotalAmount = 130m, PaidAt = new DateTime(2026, 1, 28, 0, 0, 0, DateTimeKind.Utc), ExternalReference = "PAY-ONLINE-0008", CreatedAt = createdAt },
                new Payment { Id = 9, EducationCreditTransactionId = 7, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(6), CitizenNricSnapshot = SingaporeNricUtil.Generate(6), CitizenFullNameSnapshot = "Hannah Lee", TotalAmount = 50m, PaidAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt },
                new Payment { Id = 10, EducationCreditTransactionId = 8, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedAccountNumberUtil.Generate(8), CitizenNricSnapshot = SingaporeNricUtil.Generate(8), CitizenFullNameSnapshot = "Sofia Chen", TotalAmount = 70m, PaidAt = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = createdAt });

            return modelBuilder;
        }
    }
}
