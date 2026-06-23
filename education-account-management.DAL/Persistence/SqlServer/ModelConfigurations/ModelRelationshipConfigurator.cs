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

        modelBuilder.Entity<SchoolStudent>()
            .HasOne(student => student.EducationAccount)
            .WithOne(account => account.SchoolStudent)
            .HasForeignKey<SchoolStudent>(student => student.EducationAccountId);

        modelBuilder.Entity<Payment>()
            .HasOne(payment => payment.EducationCreditTransaction)
            .WithOne(transaction => transaction.Payment)
            .HasForeignKey<Payment>(payment => payment.EducationCreditTransactionId);

        modelBuilder.Entity<TopupExecutionTarget>()
            .HasOne(target => target.EducationCreditTransaction)
            .WithOne(transaction => transaction.TopupExecutionTarget)
            .HasForeignKey<TopupExecutionTarget>(target => target.EducationCreditTransactionId);

        modelBuilder.Entity<SystemTopupConditionGroup>()
            .HasOne(group => group.ParentGroup)
            .WithMany(group => group.ChildGroups)
            .HasForeignKey(group => group.ParentGroupId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ScheduleTopUpConditionGroup>()
            .HasOne(group => group.ParentGroup)
            .WithMany(group => group.ChildGroups)
            .HasForeignKey(group => group.ParentGroupId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OutstandingDeductionTarget>()
            .HasOne(target => target.EducationCreditTransaction)
            .WithOne(transaction => transaction.OutstandingDeductionTarget)
            .HasForeignKey<OutstandingDeductionTarget>(target => target.EducationCreditTransactionId);

        modelBuilder.Entity<OutstandingDeductionTarget>()
            .HasOne(target => target.Payment)
            .WithOne(payment => payment.OutstandingDeductionTarget)
            .HasForeignKey<OutstandingDeductionTarget>(target => target.PaymentId);

        modelBuilder.Entity<EducationAccountStatusHistory>()
            .HasOne(history => history.ChangedByUser)
            .WithMany()
            .HasForeignKey(history => history.ChangedByUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserStatusHistory>()
            .HasOne(history => history.User)
            .WithMany(user => user.StatusHistories)
            .HasForeignKey(history => history.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserStatusHistory>()
            .HasOne(history => history.ChangedByUser)
            .WithMany()
            .HasForeignKey(history => history.ChangedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
