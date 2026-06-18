using Models;

namespace Persistence.SqlServer.ModelConfigurations
{
    public static class ModelIndexConfigurator
    {
        public static void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Citizen>(entity =>
            {
                entity.HasIndex(citizen => citizen.DateOfBirth);
                entity.HasIndex(citizen => citizen.CitizenshipStatus);
                entity.HasIndex(citizen => citizen.Email);
            });

            modelBuilder.Entity<AuthAccount>(entity =>
            {
                entity.HasIndex(authAccount => authAccount.Status);
            });

            modelBuilder.Entity<SsoIdentity>(entity =>
            {
                entity.HasIndex(ssoIdentity => ssoIdentity.AuthAccountId);
                entity.HasIndex(ssoIdentity => ssoIdentity.Provider);
                entity.HasIndex(ssoIdentity => ssoIdentity.ProviderUserId);
                entity.HasIndex(ssoIdentity => new { ssoIdentity.Provider, ssoIdentity.ProviderUserId }).IsUnique();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasIndex(refreshToken => refreshToken.AuthAccountId);
                entity.HasIndex(refreshToken => refreshToken.ExpiresAt);
                entity.HasIndex(refreshToken => refreshToken.RevokedAt);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(user => user.AuthAccountId).IsUnique();
                entity.HasIndex(user => user.CitizenId);
                entity.HasIndex(user => user.Role);
            });

            modelBuilder.Entity<AdminProfile>(entity =>
            {
                entity.HasIndex(adminProfile => adminProfile.UserId).IsUnique();
                entity.HasIndex(adminProfile => adminProfile.SchoolId);
            });

            modelBuilder.Entity<EducationAccount>(entity =>
            {
                entity.HasIndex(educationAccount => educationAccount.CitizenId).IsUnique();
                entity.HasIndex(educationAccount => educationAccount.Status);
                entity.HasIndex(educationAccount => educationAccount.OpenedAt);
                entity.HasIndex(educationAccount => educationAccount.ClosedAt);
                entity.HasIndex(educationAccount => educationAccount.ExtendedUntil);
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.HasIndex(school => school.Status);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(course => course.SchoolId);
                entity.HasIndex(course => course.Status);
                entity.HasIndex(course => course.CourseName);
            });

            modelBuilder.Entity<CourseFee>(entity =>
            {
                entity.HasIndex(courseFee => courseFee.CourseId);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasIndex(enrollment => enrollment.CourseId);
                entity.HasIndex(enrollment => enrollment.EducationAccountId);
                entity.HasIndex(enrollment => new { enrollment.CourseId, enrollment.EducationAccountId }).IsUnique();
            });

            modelBuilder.Entity<Charge>(entity =>
            {
                entity.HasIndex(charge => charge.EnrollmentId).IsUnique();
                entity.HasIndex(charge => charge.Status);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasIndex(payment => payment.EducationCreditTransactionId).IsUnique();
                entity.HasIndex(payment => payment.PaymentMethod);
                entity.HasIndex(payment => payment.Status);
                entity.HasIndex(payment => payment.PaidAt);
                entity.HasIndex(payment => payment.ExternalReference);
            });

            modelBuilder.Entity<PaymentAllocation>(entity =>
            {
                entity.HasIndex(allocation => allocation.PaymentId);
                entity.HasIndex(allocation => allocation.ChargeId);
                entity.HasIndex(allocation => new { allocation.PaymentId, allocation.ChargeId }).IsUnique();
            });

            modelBuilder.Entity<TopupRule>(entity =>
            {
                entity.HasIndex(topupRule => topupRule.RuleName);
                entity.HasIndex(topupRule => topupRule.Status);
            });

            modelBuilder.Entity<TopupRuleCondition>(entity =>
            {
                entity.HasIndex(condition => condition.TopupRuleId);
                entity.HasIndex(condition => condition.Field);
                entity.HasIndex(condition => condition.Operator);
            });

            modelBuilder.Entity<TopupBatch>(entity =>
            {
                entity.HasIndex(batch => batch.TopupRuleId);
                entity.HasIndex(batch => batch.Status);
            });

            modelBuilder.Entity<TopupBatchTarget>(entity =>
            {
                entity.HasIndex(target => target.TopupBatchId);
                entity.HasIndex(target => target.EducationAccountId);
                entity.HasIndex(target => target.Status);
                entity.HasIndex(target => new { target.TopupBatchId, target.EducationAccountId }).IsUnique();
            });

            modelBuilder.Entity<AdhocTopupBatch>(entity =>
            {
                entity.HasIndex(batch => batch.Status);
            });

            modelBuilder.Entity<AdhocTopupBatchTarget>(entity =>
            {
                entity.HasIndex(target => target.AdhocTopupBatchId);
                entity.HasIndex(target => target.EducationAccountId);
                entity.HasIndex(target => target.Status);
                entity.HasIndex(target => new { target.AdhocTopupBatchId, target.EducationAccountId }).IsUnique();
            });

            modelBuilder.Entity<EducationCreditTransaction>(entity =>
            {
                entity.HasIndex(transaction => transaction.EducationAccountId);
                entity.HasIndex(transaction => transaction.Type);
                entity.HasIndex(transaction => transaction.Direction);
            });

            modelBuilder.Entity<TopupBatchTargetTransaction>(entity =>
            {
                entity.HasIndex(link => link.TopupBatchTargetId).IsUnique();
                entity.HasIndex(link => link.EducationCreditTransactionId).IsUnique();
            });

            modelBuilder.Entity<AdhocTopupBatchTargetTransaction>(entity =>
            {
                entity.HasIndex(link => link.AdhocTopupBatchTargetId).IsUnique();
                entity.HasIndex(link => link.EducationCreditTransactionId).IsUnique();
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(log => log.ActorUserId);
                entity.HasIndex(log => log.Category);
                entity.HasIndex(log => log.Action);
                entity.HasIndex(log => log.OccurredAt);
            });

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasIndex(message => message.Type);
                entity.HasIndex(message => message.Status);
                entity.HasIndex(message => message.OccurredAt);
            });

        }
    }
}
