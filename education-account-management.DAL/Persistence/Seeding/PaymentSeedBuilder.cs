using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding;

public sealed class PaymentSeedBuilder : ISeedBuilder
{
    public int Priority => 160;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var createdAt = SeedDataConstants.CreatedAt;

        modelBuilder.Entity<Payment>().HasData(
            new Payment { Id = 1, EducationCreditTransactionId = 21, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000001", CitizenNricSnapshot = "S0000001A", CitizenFullNameSnapshot = "Citizen 001", TotalAmount = 120m, PaidAt = createdAt.AddDays(20), CreatedAt = createdAt },
            new Payment { Id = 2, EducationCreditTransactionId = 22, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000002", CitizenNricSnapshot = "S0000002A", CitizenFullNameSnapshot = "Citizen 002", TotalAmount = 70m, PaidAt = createdAt.AddDays(21), CreatedAt = createdAt },
            new Payment { Id = 3, EducationCreditTransactionId = 23, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000003", CitizenNricSnapshot = "S0000003A", CitizenFullNameSnapshot = "Citizen 003", TotalAmount = 140m, PaidAt = createdAt.AddDays(22), CreatedAt = createdAt },
            new Payment { Id = 4, EducationCreditTransactionId = 24, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000004", CitizenNricSnapshot = "S0000004A", CitizenFullNameSnapshot = "Citizen 004", TotalAmount = 180m, PaidAt = createdAt.AddDays(23), CreatedAt = createdAt },
            new Payment { Id = 5, EducationCreditTransactionId = 25, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000005", CitizenNricSnapshot = "S0000005A", CitizenFullNameSnapshot = "Citizen 005", TotalAmount = 180m, PaidAt = createdAt.AddDays(24), CreatedAt = createdAt },
            new Payment { Id = 6, EducationCreditTransactionId = 26, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000006", CitizenNricSnapshot = "S0000006A", CitizenFullNameSnapshot = "Citizen 006", TotalAmount = 100m, PaidAt = createdAt.AddDays(25), ExternalReference = "PAY-ONLINE-0006", CreatedAt = createdAt },
            new Payment { Id = 7, EducationCreditTransactionId = 27, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000007", CitizenNricSnapshot = "S0000007A", CitizenFullNameSnapshot = "Citizen 007", TotalAmount = 200m, PaidAt = createdAt.AddDays(26), ExternalReference = "PAY-ONLINE-0007", CreatedAt = createdAt },
            new Payment { Id = 8, EducationCreditTransactionId = 28, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000008", CitizenNricSnapshot = "S0000008A", CitizenFullNameSnapshot = "Citizen 008", TotalAmount = 130m, PaidAt = createdAt.AddDays(27), ExternalReference = "PAY-ONLINE-0008", CreatedAt = createdAt },
            new Payment { Id = 9, EducationCreditTransactionId = 29, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000009", CitizenNricSnapshot = "S0000009A", CitizenFullNameSnapshot = "Citizen 009", TotalAmount = 250m, PaidAt = createdAt.AddDays(28), ExternalReference = "PAY-ONLINE-0009", CreatedAt = createdAt },
            new Payment { Id = 10, EducationCreditTransactionId = 30, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = "EA000000000000000010", CitizenNricSnapshot = "S0000010A", CitizenFullNameSnapshot = "Citizen 010", TotalAmount = 300m, PaidAt = createdAt.AddDays(29), ExternalReference = "PAY-ONLINE-0010", CreatedAt = createdAt });

        return modelBuilder;
    }
}