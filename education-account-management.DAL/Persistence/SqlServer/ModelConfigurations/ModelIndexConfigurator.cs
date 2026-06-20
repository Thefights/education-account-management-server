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

            modelBuilder.Entity<SsoIdentity>(entity =>
            {
                entity.HasIndex(ssoIdentity => ssoIdentity.UserId);
                entity.HasIndex(ssoIdentity => ssoIdentity.Provider);
                entity.HasIndex(ssoIdentity => ssoIdentity.ProviderUserId);
                entity.HasIndex(ssoIdentity => new { ssoIdentity.Provider, ssoIdentity.ProviderUserId }).IsUnique();
                entity.HasIndex(ssoIdentity => new { ssoIdentity.UserId, ssoIdentity.Provider }).IsUnique();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasIndex(refreshToken => refreshToken.UserId);
                entity.HasIndex(refreshToken => refreshToken.ExpiresAt);
                entity.HasIndex(refreshToken => refreshToken.RevokedAt);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(user => user.CitizenId).IsUnique();
                entity.HasIndex(user => user.Role);
                entity.HasIndex(user => user.Status);
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

            modelBuilder.Entity<TopupSchedule>(entity =>
            {
                entity.HasIndex(s => s.TopupRuleId).IsUnique();
                entity.HasIndex(s => s.Status);
                entity.HasIndex(s => s.NextExecutionAt);
            });

            modelBuilder.Entity<TopupExecution>(entity =>
            {
                entity.HasIndex(e => e.SourceType);
                entity.HasIndex(e => e.TopupRuleId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ExecutionCode).IsUnique();
                entity.HasIndex(e => e.IdempotencyKey).IsUnique();
            });

            modelBuilder.Entity<TopupExecutionTarget>(entity =>
            {
                entity.HasIndex(t => t.TopupExecutionId);
                entity.HasIndex(t => t.EducationAccountId);
                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => new { t.TopupExecutionId, t.AccountNumber }).IsUnique();
                entity.HasIndex(t => t.EducationCreditTransactionId).IsUnique();
            });

            modelBuilder.Entity<TopupSystemApplication>(entity =>
            {
                entity.HasIndex(a => a.TopupRuleId);
                entity.HasIndex(a => a.EducationAccountId);
                entity.HasIndex(a => new { a.TopupRuleId, a.EducationAccountId }).IsUnique();
                entity.HasIndex(a => a.TopupExecutionTargetId).IsUnique();
            });

            modelBuilder.Entity<EducationCreditTransaction>(entity =>
            {
                entity.HasIndex(transaction => transaction.EducationAccountId);
                entity.HasIndex(transaction => transaction.Type);
                entity.HasIndex(transaction => transaction.Direction);
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
