using Enums;

namespace Authorization
{
    public static class RolePolicy
    {
        public const string Admin = nameof(RoleEnum.Admin);
        public const string AdminOrTenantUser = $"{nameof(RoleEnum.Admin)},{nameof(RoleEnum.TenantUser)}";
    }
}
