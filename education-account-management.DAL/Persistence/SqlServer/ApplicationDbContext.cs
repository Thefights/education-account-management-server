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

        public DbSet<AuthAccount> AuthAccount { get; set; }

        public DbSet<SsoIdentity> SsoIdentity { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<AdminProfile> AdminProfile { get; set; }

        public DbSet<EducationAccount> EducationAccount { get; set; }

        public DbSet<School> School { get; set; }

        public DbSet<Course> Course { get; set; }

        public DbSet<CourseFee> CourseFee { get; set; }

        public DbSet<Enrollment> Enrollment { get; set; }

        public DbSet<Charge> Charge { get; set; }

        public DbSet<Payment> Payment { get; set; }

        public DbSet<PaymentAllocation> PaymentAllocation { get; set; }

        public DbSet<TopupRule> TopupRule { get; set; }

        public DbSet<TopupRuleCondition> TopupRuleCondition { get; set; }

        public DbSet<TopupBatch> TopupBatch { get; set; }

        public DbSet<TopupBatchTarget> TopupBatchTarget { get; set; }

        public DbSet<AdhocTopupBatch> AdhocTopupBatch { get; set; }

        public DbSet<AdhocTopupBatchTarget> AdhocTopupBatchTarget { get; set; }

        public DbSet<EducationCreditTransaction> EducationCreditTransaction { get; set; }

        public DbSet<TopupBatchTargetTransaction> TopupBatchTargetTransaction { get; set; }

        public DbSet<AdhocTopupBatchTargetTransaction> AdhocTopupBatchTargetTransaction { get; set; }

        public DbSet<AuditLog> AuditLog { get; set; }

        public DbSet<OutboxMessage> OutboxMessage { get; set; }

        public DbSet<AiAssistantSetting> AiAssistantSetting { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SoftDeleteQueryFilter.ConfigureQueryFilter(modelBuilder);
            DeleteBehaviorConfigurator.ConfigureNavigationDeleteBehaviors(modelBuilder);
            ConfigureSchema(modelBuilder);
            ModelIndexConfigurator.ConfigureIndexes(modelBuilder);
            UniqueIndexConfigurator.ConfigureUniqueIndexes(modelBuilder);
            SeedDataConfigurator.ConfigureSeedData(modelBuilder);
        }

        private static void ConfigureSchema(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(user => user.AuthAccount)
                .WithOne()
                .HasForeignKey<User>(user => user.AuthAccountId);

            modelBuilder.Entity<AdminProfile>()
                .HasOne(adminProfile => adminProfile.User)
                .WithOne(user => user.AdminProfile)
                .HasForeignKey<AdminProfile>(adminProfile => adminProfile.UserId);

            modelBuilder.Entity<Payment>()
                .HasOne(payment => payment.EducationCreditTransaction)
                .WithOne(transaction => transaction.Payment)
                .HasForeignKey<Payment>(payment => payment.EducationCreditTransactionId);

            modelBuilder.Entity<AiAssistantSetting>()
                .ToTable(table => table.HasCheckConstraint(
                    "CK_AiAssistantSetting_Singleton",
                    "[Id] = 1"));

            modelBuilder.Entity<EducationAccount>()
                .HasOne(educationAccount => educationAccount.Citizen)
                .WithOne(citizen => citizen.EducationAccount)
                .HasForeignKey<EducationAccount>(educationAccount => educationAccount.CitizenId);

            modelBuilder.Entity<TopupBatchTargetTransaction>()
                .HasOne(link => link.TopupBatchTarget)
                .WithOne(target => target.TopupBatchTargetTransaction)
                .HasForeignKey<TopupBatchTargetTransaction>(link => link.TopupBatchTargetId);

            modelBuilder.Entity<TopupBatchTargetTransaction>()
                .HasOne(link => link.EducationCreditTransaction)
                .WithOne(transaction => transaction.TopupBatchTargetTransaction)
                .HasForeignKey<TopupBatchTargetTransaction>(link => link.EducationCreditTransactionId);

            modelBuilder.Entity<AdhocTopupBatchTargetTransaction>()
                .HasOne(link => link.AdhocTopupBatchTarget)
                .WithOne(target => target.AdhocTopupBatchTargetTransaction)
                .HasForeignKey<AdhocTopupBatchTargetTransaction>(link => link.AdhocTopupBatchTargetId);

            modelBuilder.Entity<AdhocTopupBatchTargetTransaction>()
                .HasOne(link => link.EducationCreditTransaction)
                .WithOne(transaction => transaction.AdhocTopupBatchTargetTransaction)
                .HasForeignKey<AdhocTopupBatchTargetTransaction>(link => link.EducationCreditTransactionId);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await ForeignKeyValidator.ValidateForeignKeysAsync(this, cancellationToken);
            var now = DateTime.UtcNow;
            ApplyAuditFields(now);
            await SoftDeleteCascadeHandler.ApplySoftDeleteCascadeAsync(this, cancellationToken);
            ApplyAuditFields(now);

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditFields(DateTime now)
        {
            var currentUserId = _auditUserContext.CurrentUserId;

            foreach (var entry in ChangeTracker.Entries<AuditEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = currentUserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = currentUserId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = now;
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = currentUserId;
                        break;
                }
            }
        }
    }
}
