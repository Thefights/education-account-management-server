using Common;
using System.Collections;

namespace Persistence.SqlServer
{
    public static class FilterValueConverter
    {
        public static object ConvertFilterValue<T>(
            string field,
            string value)
            where T : Entity
        {
            var type = ResolvePropertyType(typeof(T), field);

            return type == null
                ? value
                : ConvertFilterValue(type, value) ?? value;
        }

        public static object? ConvertFilterValue(
            Type type,
            string value)
        {
            var targetType = Nullable.GetUnderlyingType(type) ?? type;

            if (targetType.IsEnum)
            {
                return Enum.Parse(
                    targetType,
                    value,
                    ignoreCase: true);
            }

            if (targetType == typeof(DateTime))
            {
                return DateTime.Parse(
                    value,
                    null,
                    System.Globalization.DateTimeStyles.RoundtripKind);
            }

            if (targetType == typeof(Guid))
            {
                return Guid.Parse(value);
            }

            return targetType == typeof(string)
                ? value
                : Convert.ChangeType(
                    value,
                    targetType,
                    System.Globalization.CultureInfo.InvariantCulture);
        }

        public static Type? ResolvePropertyType(
            Type rootType,
            string field)
        {
            const string collectionMarker = "[].";

            var normalizedField = field.Replace(
                collectionMarker,
                ".",
                StringComparison.Ordinal);

            var currentType = rootType;

            foreach (var segment in normalizedField.Split(
                         '.',
                         StringSplitOptions.RemoveEmptyEntries |
                         StringSplitOptions.TrimEntries))
            {
                var property = currentType.GetProperty(
                    segment,
                    BindingFlags.IgnoreCase |
                    BindingFlags.Instance |
                    BindingFlags.Public);

                if (property == null)
                {
                    return null;
                }

                currentType = property.PropertyType;

                if (currentType != typeof(string) &&
                    currentType.IsGenericType &&
                    typeof(IEnumerable).IsAssignableFrom(currentType))
                {
                    currentType = currentType.GetGenericArguments()[0];
                }
            }

            return currentType;
        }

        public static bool IsSafeNumericLiteral(string value)
        {
            return value.All(char.IsDigit);
        }
    }
}