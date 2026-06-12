using Models;
using Persistence.SqlServer.ModelConfigurations;

namespace Persistence.SqlServer
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<AuthAccount> AuthAccount { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DbSet<OtpVerification> OtpVerification { get; set; }

        public DbSet<MfaSetting> MfaSetting { get; set; }

        public DbSet<PasswordResetToken> PasswordResetToken { get; set; }

        public DbSet<SocialLogin> SocialLogin { get; set; }

        public DbSet<UserFavoriteProduct> UserFavoriteProduct { get; set; }

        public DbSet<UserProductAssignment> UserProductAssignment { get; set; }

        public DbSet<EmailWhitelistSetting> EmailWhitelistSetting { get; set; }

        public DbSet<EmailWhitelist> EmailWhitelist { get; set; }

        public DbSet<AuditLog> AuditLog { get; set; }

        public DbSet<OutboxMessage> OutboxMessage { get; set; }

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
                .WithOne(authAccount => authAccount.User)
                .HasForeignKey<User>(user => user.AuthAccountId);

            modelBuilder.Entity<UserRole>()
                .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            modelBuilder.Entity<UserFavoriteProduct>()
                .HasKey(userFavoriteProduct => new { userFavoriteProduct.UserId, userFavoriteProduct.ProductId });

            modelBuilder.Entity<UserProductAssignment>()
                .HasKey(userProductAssignment => new { userProductAssignment.UserId, userProductAssignment.ProductId });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await ForeignKeyValidator.ValidateForeignKeysAsync(this, cancellationToken);
            ApplyAuditFields();
            await SoftDeleteCascadeHandler.ApplySoftDeleteCascadeAsync(this, cancellationToken);

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditFields()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreationDate = now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModificationDate = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletionDate = now;
                        break;
                }
            }
        }
    }
}
