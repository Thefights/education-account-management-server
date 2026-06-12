using education_account_management.DAL.Models;
using Models;

namespace Persistence.SqlServer.ModelConfigurations
{
    public static class ModelIndexConfigurator
    {
        public static void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthAccount>(entity =>
            {
                entity.HasIndex(authAccount => authAccount.UserIdText).IsUnique();
                entity.HasIndex(authAccount => authAccount.Email);
                entity.HasIndex(authAccount => authAccount.Status);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(user => user.AuthAccountId).IsUnique();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasIndex(refreshToken => refreshToken.AuthAccountId);
                entity.HasIndex(refreshToken => refreshToken.TokenHash).IsUnique();
                entity.HasIndex(refreshToken => refreshToken.ExpiresAt);
                entity.HasIndex(refreshToken => refreshToken.RevokedAt);
                entity.HasIndex(refreshToken => refreshToken.ReplacedByRefreshTokenId);
            });

            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.HasIndex(passwordResetToken => passwordResetToken.AuthAccountId);
                entity.HasIndex(passwordResetToken => passwordResetToken.TokenHash).IsUnique();
                entity.HasIndex(passwordResetToken => passwordResetToken.ExpiresAt);
                entity.HasIndex(passwordResetToken => passwordResetToken.UsedAt);
            });

            modelBuilder.Entity<OtpVerification>(entity =>
            {
                entity.HasIndex(otpVerification => otpVerification.SessionId).IsUnique();
                entity.HasIndex(otpVerification => otpVerification.AuthAccountId);
                entity.HasIndex(otpVerification => otpVerification.Target);
                entity.HasIndex(otpVerification => otpVerification.Purpose);
                entity.HasIndex(otpVerification => otpVerification.DeliveryMethod);
                entity.HasIndex(otpVerification => otpVerification.ExpiresAt);
                entity.HasIndex(otpVerification => otpVerification.UsedAt);
                entity.HasIndex(otpVerification => otpVerification.InvalidatedAt);
            });

            modelBuilder.Entity<SocialLogin>(entity =>
            {
                entity.HasIndex(socialLogin => new { socialLogin.Provider, socialLogin.ProviderUserId }).IsUnique();
                entity.HasIndex(socialLogin => new { socialLogin.AuthAccountId, socialLogin.Provider }).IsUnique();
                entity.HasIndex(socialLogin => socialLogin.ProviderEmail);
            });

            modelBuilder.Entity<EmailWhitelist>(entity =>
            {
                entity.HasIndex(emailWhitelist => emailWhitelist.Value).IsUnique();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(product => product.Status);
                entity.HasIndex(product => product.Name);
            });

            modelBuilder.Entity<UserFavoriteProduct>(entity =>
            {
                entity.HasIndex(userFavoriteProduct => userFavoriteProduct.ProductId);
            });

            modelBuilder.Entity<UserProductAssignment>(entity =>
            {
                entity.HasIndex(userProductAssignment => userProductAssignment.ProductId);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(userRole => userRole.RoleId);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(auditLog => auditLog.ActorUserId);
                entity.HasIndex(auditLog => auditLog.ActorUserIdText);
                entity.HasIndex(auditLog => auditLog.Category);
                entity.HasIndex(auditLog => auditLog.Action);
                entity.HasIndex(auditLog => auditLog.CreatedAt);
                entity.HasIndex(auditLog => auditLog.Object);
                entity.HasIndex(auditLog => new { auditLog.Category, auditLog.Action, auditLog.CreatedAt });
            });
        }
    }
}
