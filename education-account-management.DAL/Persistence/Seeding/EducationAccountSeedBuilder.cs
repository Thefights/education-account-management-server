using Models;
using Persistence.Seeding.Constants;

namespace Persistence.Seeding
{
    public sealed class EducationAccountSeedBuilder : ISeedBuilder
    {
        public int Priority => 20;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var createdAt = SeedDataConstants.CreatedAt;
            var accounts = new List<EducationAccount>();

            for (int i = 1; i <= 50; i++)
            {
                decimal totalCredit = 1000m;
                if (i == 3) totalCredit += 105m;
                if (i == 7) totalCredit += 145m;

                decimal totalDebit = 0m;
                
                if (i % 3 == 0 && i % 2 != 0)
                {
                    decimal courseFee = 125m + (i * 5m);
                    decimal miscFee = 23m;
                    decimal gst = Math.Round((courseFee + miscFee) * 0.09m, 2);
                    decimal grossAmount = courseFee + miscFee + gst;
                    decimal subsidyAmount = i % 4 == 0 ? 30m : 0m;
                    totalDebit = grossAmount - subsidyAmount;
                }
                
                if (i == 1) 
                {
                    totalDebit += 185m; // Account 1 has extra payment
                }
                
                decimal availableBalance = totalCredit - totalDebit;

                accounts.Add(new EducationAccount
                {
                    Id = i,
                    AccountNumber = SeedBusinessCodeUtil.Generate(BusinessCodeGenerator.EducationAccountPrefix, i),
                    EducationCreditBalance = availableBalance,
                    Status = EducationAccountStatus.Active,
                    CitizenId = i,
                    CreatedAt = createdAt,
                    OpenedAt = createdAt
                });
            }

            modelBuilder.Entity<EducationAccount>().HasData(accounts);
            return modelBuilder;
        }
    }
}
