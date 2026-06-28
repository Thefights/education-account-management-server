using System.Collections;
using System.Reflection;
using Utils;

namespace Filters.Base
{
    public class FilterDTO
    {
        private static readonly IReadOnlyDictionary<string, string> DefaultSortFields =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["id"] = "Id"
            };

        public int Page { get; set; } = 1;

        public virtual int PageSize { get; set; } = 10;

        public virtual string Sort { get; set; } = "id desc";

        public virtual IReadOnlyDictionary<string, string> SortFields => DefaultSortFields;

        public string? Search { get; set; }

        public string? Filter => BuildFilter();

        public virtual string[] SearchFields => BuildSearchFields();

        public string SortExpression => BuildSortExpression();

        protected virtual string? BuildFilter()
        {
            List<string> filters = [];

            foreach (var prop in GetType().GetProperties())
            {
                var attribute = prop.GetCustomAttribute<FilterFieldAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                var value = prop.GetValue(this);
                if (value == null || IsDefaultValue(value))
                {
                    continue;
                }

                var field = string.IsNullOrWhiteSpace(attribute.TargetField) ? prop.Name : attribute.TargetField;

                string formattedValue;
                switch (value)
                {
                    case string strVal:
                        if (string.IsNullOrWhiteSpace(strVal))
                        {
                            continue;
                        }

                        strVal = strVal.Trim();
                        prop.SetValue(this, strVal);
                        formattedValue = attribute.Operation is FilterOperationEnum.Contains
                                    or FilterOperationEnum.NotContains
                                    ? $"{strVal}/i"
                                    : strVal;
                        break;
                    case DateTime dateVal:
                        formattedValue = dateVal.ToString("o");
                        break;
                    case IEnumerable enumerable when value is not string:
                        var values = enumerable
                            .Cast<object?>()
                            .Where(item => item != null)
                            .Select(item => item!.ToString())
                            .Where(item => !string.IsNullOrWhiteSpace(item))
                            .ToList();
                        if (values.Count == 0)
                        {
                            continue;
                        }

                        formattedValue = string.Join("|", values);
                        break;
                    default:
                        formattedValue = value.ToString()!;
                        break;
                }

                filters.Add($"{field} {attribute.Operation.GetEnumDisplayName()} {formattedValue}");
            }

            return filters.Count != 0 ? string.Join(",", filters) : null;
        }

        private string BuildSortExpression()
        {
            if (string.IsNullOrWhiteSpace(Sort))
            {
                return string.Empty;
            }

            var sortExpressions = new List<string>();
            foreach (var term in Sort.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var tokens = term.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (tokens.Length is < 1 or > 2)
                {
                    throw new InvalidDataException($"Invalid sort expression '{term}'.");
                }

                var alias = tokens[0];
                if (!SortFields.TryGetValue(alias, out var field))
                {
                    throw new InvalidDataException($"Sorting by '{alias}' is not allowed.");
                }

                var direction = tokens.Length == 2 ? tokens[1] : "asc";
                if (!IsAllowedSortDirection(direction))
                {
                    throw new InvalidDataException($"Sort direction '{direction}' is not allowed.");
                }

                sortExpressions.Add($"{field} {direction.ToLowerInvariant()}");
            }

            return string.Join(", ", sortExpressions);
        }

        private bool IsDefaultValue(object value)
        {
            var type = value.GetType();
            return value.Equals(type.IsValueType ? Activator.CreateInstance(type) : null);
        }

        private static bool IsAllowedSortDirection(string direction)
        {
            return string.Equals(direction, "asc", StringComparison.OrdinalIgnoreCase)
                || string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
        }

        private string[] BuildSearchFields()
        {
            return [.. GetType()
                .GetProperties()
                .SelectMany(prop => prop.GetCustomAttributes<SearchFieldAttribute>())
                .Select(attribute => attribute.TargetField)
                .Where(field => !string.IsNullOrWhiteSpace(field))
                .Distinct(StringComparer.OrdinalIgnoreCase)];
        }
    }
}
