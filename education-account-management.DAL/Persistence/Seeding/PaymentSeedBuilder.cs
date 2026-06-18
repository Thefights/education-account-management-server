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
            new Payment { Id = 1, EducationCreditTransactionId = 21, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, TotalAmount = 120m, PaidAt = createdAt.AddDays(20), CreatedAt = createdAt },
            new Payment { Id = 2, EducationCreditTransactionId = 22, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, TotalAmount = 70m, PaidAt = createdAt.AddDays(21), CreatedAt = createdAt },
            new Payment { Id = 3, EducationCreditTransactionId = 23, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, TotalAmount = 140m, PaidAt = createdAt.AddDays(22), CreatedAt = createdAt },
            new Payment { Id = 4, EducationCreditTransactionId = 24, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, TotalAmount = 180m, PaidAt = createdAt.AddDays(23), CreatedAt = createdAt },
            new Payment { Id = 5, EducationCreditTransactionId = 25, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, TotalAmount = 180m, PaidAt = createdAt.AddDays(24), CreatedAt = createdAt },
            new Payment { Id = 6, EducationCreditTransactionId = 26, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, TotalAmount = 100m, PaidAt = createdAt.AddDays(25), ExternalReference = "PAY-ONLINE-0006", CreatedAt = createdAt },
            new Payment { Id = 7, EducationCreditTransactionId = 27, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, TotalAmount = 200m, PaidAt = createdAt.AddDays(26), ExternalReference = "PAY-ONLINE-0007", CreatedAt = createdAt },
            new Payment { Id = 8, EducationCreditTransactionId = 28, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, TotalAmount = 130m, PaidAt = createdAt.AddDays(27), ExternalReference = "PAY-ONLINE-0008", CreatedAt = createdAt },
            new Payment { Id = 9, EducationCreditTransactionId = 29, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, TotalAmount = 250m, PaidAt = createdAt.AddDays(28), ExternalReference = "PAY-ONLINE-0009", CreatedAt = createdAt },
            new Payment { Id = 10, EducationCreditTransactionId = 30, PaymentMethod = PaymentMethod.OnlinePayment, Status = PaymentStatus.Succeeded, TotalAmount = 300m, PaidAt = createdAt.AddDays(29), ExternalReference = "PAY-ONLINE-0010", CreatedAt = createdAt });

        return modelBuilder;
    }
}
