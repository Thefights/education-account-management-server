using Enums;

namespace Authorization
{
    public static class RolePolicy
    {
        public const string Admin = nameof(UserRole.SystemAdmin);

        public const string AdminStaff =
            $"{nameof(UserRole.SystemAdmin)},{nameof(UserRole.FinanceAdmin)},{nameof(UserRole.CourseAdmin)}";
    }
}
