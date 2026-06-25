using Models;
using Persistence.SqlServer.ModelConfigurations;
using Repositories.Interfaces;

namespace Persistence.SqlServer
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IAuditUserContext auditUserContext) : DbContext(options)
    {
        private readonly IAuditUserContext _auditUserContext = auditUserContext;

        public DbSet<Citizen> Citizen { get; set; }

        public DbSet<SsoIdentity> SsoIdentity { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<AdminProfile> AdminProfile { get; set; }

        public DbSet<EducationAccount> EducationAccount { get; set; }

        public DbSet<School> School { get; set; }

        public DbSet<SchoolStudent> SchoolStudent { get; set; }

        public DbSet<Course> Course { get; set; }

        public DbSet<Enrollment> Enrollment { get; set; }

        public DbSet<Charge> Charge { get; set; }

        public DbSet<ChargeInstallment> ChargeInstallment { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<PaymentAllocation> PaymentAllocation { get; set; }

        public DbSet<FasScheme> FasScheme { get; set; }

        public DbSet<FasSchemeConditionGroup> FasSchemeConditionGroup { get; set; }

        public DbSet<FasSchemeCondition> FasSchemeCondition { get; set; }

        public DbSet<FasSchemeTier> FasSchemeTier { get; set; }

        public DbSet<FasSchemeRequiredDocument> FasSchemeRequiredDocument { get; set; }

        public DbSet<FasSchemeCourse> FasSchemeCourse { get; set; }

        public DbSet<FasApplication> FasApplication { get; set; }

        public DbSet<FasApplicationDocument> FasApplicationDocument { get; set; }

        public DbSet<FasTierOverrideHistory> FasTierOverrideHistory { get; set; }

        public DbSet<OutstandingDeductionRun> OutstandingDeductionRun { get; set; }

        public DbSet<OutstandingDeductionTarget> OutstandingDeductionTarget { get; set; }

        public DbSet<SystemTopup> SystemTopup { get; set; }

        public DbSet<SystemTopupConditionGroup> SystemTopupConditionGroup { get; set; }

        public DbSet<SystemTopupCondition> SystemTopupCondition { get; set; }

        public DbSet<ScheduleTopUp> ScheduleTopUp { get; set; }

        public DbSet<ScheduleTopUpConditionGroup> ScheduleTopUpConditionGroup { get; set; }

        public DbSet<ScheduleTopUpCondition> ScheduleTopUpCondition { get; set; }

        public DbSet<EducationAccountSweepReport> EducationAccountSweepReports { get; set; }

        public DbSet<EducationAccountSweepTarget> EducationAccountSweepTargets { get; set; }

        public DbSet<TopupExecution> TopupExecution { get; set; }

        public DbSet<TopupExecutionTarget> TopupExecutionTarget { get; set; }

        public DbSet<TopupSystemApplication> TopupSystemApplication { get; set; }

        public DbSet<EducationCreditTransaction> EducationCreditTransaction { get; set; }

        public DbSet<EducationAccountStatusHistory> EducationAccountStatusHistory { get; set; }

        public DbSet<UserStatusHistory> UserStatusHistory { get; set; }

        public DbSet<AuditLog> AuditLog { get; set; }

        public DbSet<OutboxMessage> OutboxMessage { get; set; }

        public DbSet<AiAssistantSetting> AiAssistantSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SoftDeleteQueryFilter.ConfigureQueryFilter(modelBuilder);
            DeleteBehaviorConfigurator.ConfigureNavigationDeleteBehaviors(modelBuilder);
            ModelRelationshipConfigurator.ConfigureRelationships(modelBuilder);
            CheckConstraintConfigurator.ConfigureCheckConstraints(modelBuilder);
            ModelIndexConfigurator.ConfigureIndexes(modelBuilder);
            UniqueIndexConfigurator.ConfigureUniqueIndexes(modelBuilder);
            SeedDataConfigurator.ConfigureSeedData(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await ForeignKeyValidator.ValidateForeignKeysAsync(this, cancellationToken);
            var now = DateTime.UtcNow;
            AuditEntityChangeTracker.Apply(ChangeTracker, _auditUserContext.CurrentUserId, now);
            await SoftDeleteCascadeHandler.ApplySoftDeleteCascadeAsync(this, cancellationToken);
            AuditEntityChangeTracker.Apply(ChangeTracker, _auditUserContext.CurrentUserId, now);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
