namespace Utils
{
    public static class EnumUtil
    {
        public static string GetEnumDisplayName(this Enum enumValue)
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            if (member == null)
            {
                return enumValue.ToString();
            }

            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        public static bool TryParseDefined<TEnum>(string? value, out TEnum parsed) where TEnum : struct, Enum
        {
            return TryParse(value, out parsed) && Enum.IsDefined(parsed);
        }

        private static bool TryParse<TEnum>(string? value, out TEnum parsed) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                parsed = default;
                return false;
            }

            return Enum.TryParse(value.Trim(), ignoreCase: true, out parsed);
        }
    }
}
