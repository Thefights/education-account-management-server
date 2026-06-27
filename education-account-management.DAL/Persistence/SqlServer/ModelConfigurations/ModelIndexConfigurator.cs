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
                entity.HasIndex(school => school.SchoolName);
                entity.HasIndex(school => school.Email);
            });

            modelBuilder.Entity<SchoolStudent>(entity =>
            {
                entity.HasIndex(student => student.SchoolId);
                entity.HasIndex(student => student.EducationAccountId).IsUnique();
                entity.HasIndex(student => student.Status);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(course => course.SchoolId);
                entity.HasIndex(course => course.Status);
                entity.HasIndex(course => course.CourseName);
                entity.HasIndex(course => course.FasApplicationDueDate);
                entity.HasIndex(course => course.EnrollmentDeadline);
                entity.HasIndex(course => course.StartDate);
                entity.HasIndex(course => course.EndDate);
                entity.HasIndex(course => new { course.SchoolId, course.CourseCode }).IsUnique();
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasIndex(enrollment => enrollment.CourseId);
                entity.HasIndex(enrollment => enrollment.SchoolStudentId);
                entity.HasIndex(enrollment => enrollment.CitizenNricSnapshot);
                entity.HasIndex(enrollment => enrollment.AccountNumberSnapshot);
                entity.HasIndex(enrollment => new { enrollment.CourseId, enrollment.SchoolStudentId }).IsUnique();
            });

            modelBuilder.Entity<Charge>(entity =>
            {
                entity.HasIndex(charge => charge.EnrollmentId).IsUnique();
                entity.HasIndex(charge => charge.AppliedFasApplicationId);
                entity.HasIndex(charge => charge.Status);
            });

            modelBuilder.Entity<ChargeInstallment>(entity =>
            {
                entity.HasIndex(installment => installment.ChargeId);
                entity.HasIndex(installment => installment.Status);
                entity.HasIndex(installment => installment.DueDate);
                entity.HasIndex(installment => new { installment.ChargeId, installment.InstallmentNumber }).IsUnique();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasIndex(payment => payment.EducationCreditTransactionId).IsUnique();
                entity.HasIndex(payment => payment.PaymentMethod);
                entity.HasIndex(payment => payment.Status);
                entity.HasIndex(payment => payment.PaidAt);
                entity.HasIndex(payment => payment.AccountNumberSnapshot);
                entity.HasIndex(payment => payment.CitizenNricSnapshot);
                entity.HasIndex(payment => payment.ExternalReference).IsUnique();
            });

            modelBuilder.Entity<PaymentAllocation>(entity =>
            {
                entity.HasIndex(allocation => allocation.PaymentId);
                entity.HasIndex(allocation => allocation.ChargeId);
                entity.HasIndex(allocation => allocation.ChargeInstallmentId);
                entity.HasIndex(allocation => new { allocation.PaymentId, allocation.ChargeId }).IsUnique();
            });

            modelBuilder.Entity<FasScheme>(entity =>
            {
                entity.HasIndex(scheme => scheme.SchoolId);
                entity.HasIndex(scheme => scheme.Status);
                entity.HasIndex(scheme => scheme.SchemeCode).IsUnique();
                entity.HasIndex(scheme => scheme.SchemeName);
            });

            modelBuilder.Entity<FasSchemeConditionGroup>(entity =>
            {
                entity.HasIndex(group => group.FasSchemeId);
                entity.HasIndex(group => group.ParentGroupId);
                entity.HasIndex(group => group.FasSchemeId)
                    .IsUnique()
                    .HasFilter($"[{nameof(FasSchemeConditionGroup.ParentGroupId)}] IS NULL");
            });

            modelBuilder.Entity<FasSchemeCondition>(entity =>
            {
                entity.HasIndex(condition => condition.GroupId);
                entity.HasIndex(condition => condition.Field);
            });

            modelBuilder.Entity<FasSchemeTier>(entity =>
            {
                entity.HasIndex(tier => tier.FasSchemeId);
                entity.HasIndex(tier => new { tier.FasSchemeId, tier.TierName }).IsUnique();
            });

            modelBuilder.Entity<FasSchemeRequiredDocument>(entity =>
            {
                entity.HasIndex(document => document.FasSchemeId);
                entity.HasIndex(document => new { document.FasSchemeId, document.DocumentName }).IsUnique();
            });

            modelBuilder.Entity<FasSchemeCourse>(entity =>
            {
                entity.HasIndex(schemeCourse => schemeCourse.FasSchemeId);
                entity.HasIndex(schemeCourse => schemeCourse.CourseId);
                entity.HasIndex(schemeCourse => new { schemeCourse.FasSchemeId, schemeCourse.CourseId }).IsUnique();
            });

            modelBuilder.Entity<FasApplication>(entity =>
            {
                entity.HasIndex(application => application.FasSchemeId);
                entity.HasIndex(application => application.SchoolStudentId);
                entity.HasIndex(application => application.Status);
                entity.HasIndex(application => application.ApplicationNumber).IsUnique();
                entity.HasIndex(application => application.ValidityEndDate);
            });

            modelBuilder.Entity<FasApplicationDocument>(entity =>
            {
                entity.HasIndex(document => document.FasApplicationId);
                entity.HasIndex(document => document.FasSchemeRequiredDocumentId);
            });

            modelBuilder.Entity<FasTierOverrideHistory>(entity =>
            {
                entity.HasIndex(history => history.FasApplicationId);
                entity.HasIndex(history => history.ModifiedByUserId);
                entity.HasIndex(history => history.ModifiedAt);
            });

            modelBuilder.Entity<SystemTopup>(entity =>
            {
                entity.HasIndex(topup => topup.Status);
            });

            modelBuilder.Entity<SystemTopupConditionGroup>(entity =>
            {
                entity.HasIndex(group => group.SystemTopupId);
                entity.HasIndex(group => group.ParentGroupId);
                entity.HasIndex(group => group.SystemTopupId)
                    .IsUnique()
                    .HasFilter($"[{nameof(SystemTopupConditionGroup.ParentGroupId)}] IS NULL");
            });

            modelBuilder.Entity<SystemTopupCondition>(entity =>
            {
                entity.HasIndex(condition => condition.GroupId);
                entity.HasIndex(condition => condition.Field);
            });

            modelBuilder.Entity<ScheduleTopUp>(entity =>
            {
                entity.HasIndex(schedule => schedule.Status);
                entity.HasIndex(schedule => schedule.NextExecutionAt);
            });

            modelBuilder.Entity<ScheduleTopUpConditionGroup>(entity =>
            {
                entity.HasIndex(group => group.ScheduleTopUpId);
                entity.HasIndex(group => group.ParentGroupId);
                entity.HasIndex(group => group.ScheduleTopUpId)
                    .IsUnique()
                    .HasFilter($"[{nameof(ScheduleTopUpConditionGroup.ParentGroupId)}] IS NULL");
            });

            modelBuilder.Entity<ScheduleTopUpCondition>(entity =>
            {
                entity.HasIndex(condition => condition.GroupId);
                entity.HasIndex(condition => condition.Field);
            });

            modelBuilder.Entity<TopupExecution>(entity =>
            {
                entity.HasIndex(e => e.SourceType);
                entity.HasIndex(e => e.SystemTopupId);
                entity.HasIndex(e => e.ScheduleTopUpId);
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
                entity.HasIndex(a => a.SystemTopupId);
                entity.HasIndex(a => a.EducationAccountId);
                entity.HasIndex(a => new { a.SystemTopupId, a.EducationAccountId }).IsUnique();
                entity.HasIndex(a => a.TopupExecutionTargetId).IsUnique();
            });

            modelBuilder.Entity<EducationCreditTransaction>(entity =>
            {
                entity.HasIndex(transaction => transaction.EducationAccountId);
                entity.HasIndex(transaction => transaction.Type);
                entity.HasIndex(transaction => transaction.Direction);
                entity.HasIndex(transaction => transaction.CreatedAt);
            });

            modelBuilder.Entity<EducationAccountStatusHistory>(entity =>
            {
                entity.HasIndex(history => history.EducationAccountId);
                entity.HasIndex(history => history.ChangedByUserId);
                entity.HasIndex(history => history.ChangedAt);
            });

            modelBuilder.Entity<UserStatusHistory>(entity =>
            {
                entity.HasIndex(history => history.UserId);
                entity.HasIndex(history => history.ChangedByUserId);
                entity.HasIndex(history => history.ChangedAt);
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
