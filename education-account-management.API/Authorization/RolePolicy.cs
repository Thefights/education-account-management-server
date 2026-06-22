using Enums;

namespace Authorization
{
    public static class RolePolicy
    {
        public const string SystemAdmin = nameof(UserRole.SystemAdmin);
        public const string AccountHolder = nameof(UserRole.AccountHolder);
        public const string FinanceAdmin = nameof(UserRole.FinanceAdmin);
        public const string SchoolAdmin = nameof(UserRole.SchoolAdmin);

    }
}
