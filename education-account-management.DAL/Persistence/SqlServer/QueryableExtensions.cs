using Common;
using System.Linq.Dynamic.Core;

namespace Persistence.SqlServer
{
    internal static class QueryableExtensions
    {
        public static IQueryable<T> ApplyIncludes<T>(
            this IQueryable<T> query,
            string[]? includes)
            where T : Entity
        {
            if (includes == null)
            {
                return query;
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public static IQueryable<T> ApplyPaging<T>(
            this IQueryable<T> query,
            int page,
            int pageSize)
            where T : Entity
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public static IQueryable<T> ApplyOrdering<T>(
            this IQueryable<T> query,
            string? order)
            where T : Entity
        {
            return string.IsNullOrWhiteSpace(order)
                ? query
                : DynamicQueryableExtensions.OrderBy(query, order);
        }

        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string? search,
            string[]? searchFields)
            where T : Entity
        {
            if (string.IsNullOrWhiteSpace(search) ||
                searchFields == null ||
                searchFields.Length == 0)
            {
                return query;
            }

            var normalizedSearch = search.Trim().ToLowerInvariant();

            var conditions = searchFields
                .Where(field => !string.IsNullOrWhiteSpace(field))
                .Select(field => $"{field} != null && {field}.ToLower().Contains(@0)")
                .ToList();

            return conditions.Count == 0
                ? query
                : DynamicQueryableExtensions.Where(
                    query,
                    string.Join(" || ", conditions),
                    normalizedSearch);
        }
    }
}