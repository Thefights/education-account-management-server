using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class PaymentSeedBuilder : ISeedBuilder
    {
        public int Priority => 140;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var payments = new List<Payment>();
            
            string[] names = { "Sterling Quach", "Amelia Tan", "Marcus Lim", "Priya Nair", "Ethan Koh", "Hannah Lee", "Daniel Wong", "Sofia Chen", "Lucas Nguyen", "Maya Rahman", "Noah Teo", "Aisha Fernandez", "Ryan Chua", "Chloe Goh", "Irfan Hassan", "Natalie Seah", "Alina Ang", "Benjamin Bala", "Clara Chew", "Darius Das", "Elena Eng", "Farhan Foo", "Grace Gan", "Haruto Ho", "Isabelle Ismail", "Jasper Jeyaratnam", "Keira Kwek", "Leon Lim", "Mei Lin Mohamed", "Nathan Ng", "Olivia Ong", "Pranav Pillai", "Qistina Quek", "Rafael Rao", "Selina Sim", "Terence Tan", "Umairah Uddin", "Victor Vasquez", "Wen Jie Wong", "Xavier Xu", "Yasmin Yeo", "Zachary Zainal", "Adeline Ang", "Brandon Bala", "Celeste Chew", "Damien Das", "Evelyn Eng", "Faris Foo", "Giselle Gan", "Haziq Ho" };

            for (int i = 1; i <= 50; i++)
            {
                decimal courseFee = 125m + (i * 5m);
                decimal miscFee = 23m;
                decimal gst = Math.Round((courseFee + miscFee) * 0.09m, 2);
                decimal grossAmount = courseFee + miscFee + gst;
                decimal subsidyAmount = i % 4 == 0 ? 30m : 0m;
                decimal netAmount = grossAmount - subsidyAmount;
                
                var status = PaymentStatus.Pending;
                if (i % 3 == 0) status = PaymentStatus.Succeeded;
                else if (i % 3 == 2) status = PaymentStatus.Failed;
                
                DateTime? paidAt = status == PaymentStatus.Succeeded ? new DateTime(2026, 6, (i % 28) + 1, 0, 0, 0, DateTimeKind.Utc) : null;
                string? externalRef = i % 2 == 0 ? $"PAY-EXT-000{i:D2}" : null;
                
                payments.Add(new Payment
                {
                    Id = i,
                    EducationCreditTransactionId = (i % 2 != 0 && status == PaymentStatus.Succeeded) ? i : null,
                    PaymentMethod = i % 2 == 0 ? PaymentMethod.OnlinePayment : PaymentMethod.EducationBalance,
                    Status = status,
                    AccountNumberSnapshot = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, i),
                    CitizenNricSnapshot = SingaporeNricUtil.Generate(i),
                    CitizenFullNameSnapshot = names[i - 1],
                    TotalAmount = netAmount,
                    CreatedAt = createdAt,
                    PaidAt = paidAt,
                    ExternalReference = externalRef
                });
            }

            payments.Add(new Payment { Id = 51, EducationCreditTransactionId = 51, PaymentMethod = PaymentMethod.EducationBalance, Status = PaymentStatus.Succeeded, AccountNumberSnapshot = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, 1), CitizenNricSnapshot = SingaporeNricUtil.Generate(1), CitizenFullNameSnapshot = "Sterling Quach", TotalAmount = 185m, CreatedAt = createdAt.AddDays(30), PaidAt = createdAt.AddDays(30) });

            modelBuilder.Entity<Payment>().HasData(payments);
            return modelBuilder;
        }
    }
}
