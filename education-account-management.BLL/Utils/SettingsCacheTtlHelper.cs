using education_account_management.BLL;

namespace Utils
{
    public static class SettingsCacheTtlHelper
    {
        public static TimeSpan GetSettingsTtl(AppConfiguration configuration)
        {
            return TimeSpan.FromMinutes(Math.Max(1, configuration.DataCacheConfig.SettingsTtlMinutes));
        }
    }
}
