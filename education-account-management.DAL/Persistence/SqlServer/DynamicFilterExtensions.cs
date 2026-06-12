using System.Collections;
using System.Linq.Dynamic.Core;

namespace Persistence.SqlServer
{
    public static class DynamicFilterExtensions
    {
        public static IQueryable<T> ApplyFiltering<T>(
            this IQueryable<T> query,
            string? filter)
            where T : Entity
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return query;
            }

            foreach (var condition in filter.Split(
                         ',',
                         StringSplitOptions.RemoveEmptyEntries |
                         StringSplitOptions.TrimEntries))
            {
                query = ApplyCondition(query, condition);
            }

            return query;
        }

        private static IQueryable<T> ApplyCondition<T>(
            IQueryable<T> query,
            string condition)
            where T : Entity
        {
            var tokens = condition.Split(
                ' ',
                3,
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

            if (tokens.Length < 3)
            {
                return query;
            }

            var field = tokens[0];
            var operation = tokens[1];
            var rawValue = tokens[2];

            var ignoreCase = rawValue.EndsWith(
                "/i",
                StringComparison.OrdinalIgnoreCase);

            var value = ignoreCase
                ? rawValue[..^2]
                : rawValue;

            var queryField = ignoreCase
                ? $"{field}.ToLower()"
                : field;

            var queryValue = ignoreCase
                ? value.ToLowerInvariant()
                : value;

            if (operation is "in" or "!in")
            {
                return ApplyInCondition(
                    query,
                    field,
                    rawValue,
                    operation == "!in");
            }

            var convertedValue = FilterValueConverter.ConvertFilterValue<T>(
                field,
                value);

            return TryBuildCollectionFilter(
                field,
                operation,
                queryValue,
                ignoreCase,
                out var collectionPredicate,
                out var useParameter)
                ? useParameter
                    ? DynamicQueryableExtensions.Where(
                        query,
                        collectionPredicate,
                        queryValue)
                    : DynamicQueryableExtensions.Where(
                        query,
                        collectionPredicate)
                : operation switch
                {
                    "=" => DynamicQueryableExtensions.Where(
                        query,
                        $"{queryField} == @0",
                        ignoreCase ? queryValue : convertedValue),

                    "!=" => DynamicQueryableExtensions.Where(
                        query,
                        $"{queryField} != @0",
                        ignoreCase ? queryValue : convertedValue),

                    ">" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} > @0",
                        convertedValue),

                    "<" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} < @0",
                        convertedValue),

                    ">=" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} >= @0",
                        convertedValue),

                    "<=" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} <= @0",
                        convertedValue),

                    "=*" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} != null && {queryField}.Contains(@0)",
                        queryValue),

                    "!*" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} == null || !{queryField}.Contains(@0)",
                        queryValue),

                    "^" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} != null && {queryField}.StartsWith(@0)",
                        queryValue),

                    "!^" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} == null || !{queryField}.StartsWith(@0)",
                        queryValue),

                    "$" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} != null && {queryField}.EndsWith(@0)",
                        queryValue),

                    "!$" => DynamicQueryableExtensions.Where(
                        query,
                        $"{field} == null || !{queryField}.EndsWith(@0)",
                        queryValue),

                    _ => query
                };
        }

        private static IQueryable<T> ApplyInCondition<T>(
            IQueryable<T> query,
            string field,
            string rawValue,
            bool negate)
            where T : Entity
        {
            var valueType = FilterValueConverter.ResolvePropertyType(
                typeof(T),
                field);

            if (valueType == null)
            {
                return query;
            }

            var values = rawValue
                .Split(
                    '|',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries)
                .Select(value => FilterValueConverter.ConvertFilterValue(
                    valueType,
                    value))
                .Where(value => value != null)
                .ToList();

            if (values.Count == 0)
            {
                return query;
            }

            var listType = typeof(List<>).MakeGenericType(valueType);
            var typedValues = (IList)Activator.CreateInstance(listType)!;

            foreach (var value in values)
            {
                typedValues.Add(value);
            }

            var predicate = negate
                ? $"!@0.Contains({field})"
                : $"@0.Contains({field})";

            return DynamicQueryableExtensions.Where(
                query,
                predicate,
                typedValues);
        }

        private static bool TryBuildCollectionFilter(
            string field,
            string operation,
            string queryValue,
            bool ignoreCase,
            out string predicate,
            out bool useParameter)
        {
            predicate = string.Empty;
            useParameter = true;

            const string collectionMarker = "[].";

            var markerIndex = field.IndexOf(
                collectionMarker,
                StringComparison.Ordinal);

            if (markerIndex < 0)
            {
                return false;
            }

            var collectionField = field[..markerIndex];

            var itemField = field[
                (markerIndex + collectionMarker.Length)..];

            if (string.IsNullOrWhiteSpace(collectionField) ||
                string.IsNullOrWhiteSpace(itemField))
            {
                return false;
            }

            var queryField = ignoreCase
                ? $"{itemField}.ToLower()"
                : itemField;

            var equalityValue = "@0";

            if (!ignoreCase &&
                FilterValueConverter.IsSafeNumericLiteral(queryValue))
            {
                equalityValue = queryValue;
                useParameter = false;
            }

            predicate = operation switch
            {
                "=" => $"{collectionField}.Any({queryField} == {equalityValue})",

                "!=" => $"!{collectionField}.Any({queryField} == {equalityValue})",

                "=*" => $"{collectionField}.Any({itemField} != null && {queryField}.Contains(@0))",

                "!*" => $"!{collectionField}.Any({itemField} != null && {queryField}.Contains(@0))",

                "^" => $"{collectionField}.Any({itemField} != null && {queryField}.StartsWith(@0))",

                "!^" => $"!{collectionField}.Any({itemField} != null && {queryField}.StartsWith(@0))",

                "$" => $"{collectionField}.Any({itemField} != null && {queryField}.EndsWith(@0))",

                "!$" => $"!{collectionField}.Any({itemField} != null && {queryField}.EndsWith(@0))",

                _ => string.Empty
            };

            return !string.IsNullOrWhiteSpace(predicate);
        }
    }
}