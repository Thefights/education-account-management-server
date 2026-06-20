using Models;

namespace Persistence.SqlServer.ModelConfigurations;

public static class ModelRelationshipConfigurator
{
    public static void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminProfile>()
            .HasOne(profile => profile.User)
            .WithOne(user => user.AdminProfile)
            .HasForeignKey<AdminProfile>(profile => profile.UserId);

        modelBuilder.Entity<User>()
            .HasOne(user => user.Citizen)
            .WithOne(citizen => citizen.User)
            .HasForeignKey<User>(user => user.CitizenId);

        modelBuilder.Entity<EducationAccount>()
            .HasOne(account => account.Citizen)
            .WithOne(citizen => citizen.EducationAccount)
            .HasForeignKey<EducationAccount>(account => account.CitizenId);

        modelBuilder.Entity<Payment>()
            .HasOne(payment => payment.EducationCreditTransaction)
            .WithOne(transaction => transaction.Payment)
            .HasForeignKey<Payment>(payment => payment.EducationCreditTransactionId);

        modelBuilder.Entity<TopupExecutionTarget>()
            .HasOne(target => target.EducationCreditTransaction)
            .WithOne(transaction => transaction.TopupExecutionTarget)
            .HasForeignKey<TopupExecutionTarget>(target => target.EducationCreditTransactionId);
    }
}
