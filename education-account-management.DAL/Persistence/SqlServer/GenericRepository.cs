using Common;
using Repositories.Interfaces;
using System.Linq.Expressions;

namespace Persistence.SqlServer
{
    public class GenericRepository<T>(DbContext dbContext) : IGenericRepository<T>
        where T : Entity
    {
        protected readonly DbContext _dbContext = dbContext;

        #region Get All

        public IQueryable<T> Query(bool tracking = false, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return tracking ? query : query.AsNoTracking();
        }

        public async Task<List<TResult>> GetProjectedAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> projection,
            Expression<Func<T, bool>>? filter = null,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(filter, includes);
            return await projection(query).ToListAsync(cancellationToken);
        }

        public async Task<TResult?> FirstOrDefaultProjectedAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> projection,
            Expression<Func<T, bool>>? filter = null,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(filter, includes);
            return await projection(query).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TResult>> GetProjectedFilteredAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> projection,
            string? filter,
            string? search,
            string[]? searchFields,
            string order,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(includes: includes);

            query = query
                .ApplyFiltering(filter)
                .ApplySearch(search, searchFields)
                .ApplyOrdering(order);

            return await projection(query).ToListAsync(cancellationToken);
        }

        public async Task<List<T>> GetTrackedByIdsAsync(
            List<int> ids,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            if (ids.Count == 0 || !typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return [];
            }

            IQueryable<T> query = BuildTrackedQuery(includes: includes);

            if (ids.Count == 1)
            {
                int id = ids[0];

                var entity = await query.FirstOrDefaultAsync(
                    e => (e as BaseEntity)!.Id == id,
                    cancellationToken);

                return entity != null ? [entity] : [];
            }

            return await query
                .Where(e => ids.Contains((e as BaseEntity)!.Id))
                .ToListAsync(cancellationToken);
        }

        #endregion

        #region Get All Paginated

        public async Task<(int Count, List<TResult> Items)> GetProjectedPaginatedAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> projection,
            string? filter,
            string? search,
            string[]? searchFields,
            string order,
            int page,
            int pageSize,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = BuildQuery(includes: includes);
            var total = await query.CountAsync(cancellationToken);

            try
            {
                query = query
                    .ApplyFiltering(filter)
                    .ApplySearch(search, searchFields)
                    .ApplyOrdering(order);

                total = await query.CountAsync(cancellationToken);
            }
            finally
            {
                query = query.ApplyPaging(page, pageSize);
            }

            return (total, await projection(query).ToListAsync(cancellationToken));
        }

        public async Task<(int Count, List<TResult> Items)> GetProjectedPaginatedAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> projection,
            Expression<Func<T, bool>>? filter,
            string order,
            int page,
            int pageSize,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = BuildQuery(filter, includes);
            var total = await query.CountAsync(cancellationToken);

            try
            {
                query = query.ApplyOrdering(order);
            }
            finally
            {
                query = query.ApplyPaging(page, pageSize);
            }

            return (total, await projection(query).ToListAsync(cancellationToken));
        }

        public async Task<(int Count, List<TResult> Items)> GetProjectedPaginatedAsync<TResult>(
            Func<IQueryable<T>, IQueryable<TResult>> projection,
            Expression<Func<T, bool>>? filterExpr,
            string? filterStr,
            string order,
            int page,
            int pageSize,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = BuildQuery(filterExpr, includes);
            var total = await query.CountAsync(cancellationToken);

            try
            {
                query = query
                    .ApplyFiltering(filterStr)
                    .ApplyOrdering(order);

                total = await query.CountAsync(cancellationToken);
            }
            finally
            {
                query = query.ApplyPaging(page, pageSize);
            }

            return (total, await projection(query).ToListAsync(cancellationToken));
        }

        #endregion

        #region Get One

        public async Task<T?> GetTrackedByIdAsync(
            int id,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            if (!typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return null;
            }

            if (includes == null || includes.Length == 0)
            {
                return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
            }

            IQueryable<T> query = _dbContext.Set<T>();
            query = query.ApplyIncludes(includes);

            return await query.FirstOrDefaultAsync(
                entity => (entity as BaseEntity)!.Id == id,
                cancellationToken);
        }

        #endregion

        #region Add

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        public async Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        #endregion

        #region Remove

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        #endregion

        #region Update

        public T Update(T entity)
        {
            var result = _dbContext.Set<T>().Update(entity);
            return result.Entity;
        }

        public void UpdateRange(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
        }

        #endregion

        #region Others

        public async Task<bool> AnyAsync(
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().AnyAsync(filter ?? (_ => true), cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().CountAsync(filter ?? (_ => true), cancellationToken);
        }

        #endregion

        private IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? filter = null,
            string[]? includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            query = query.ApplyIncludes(includes);

            return query
                .Where(filter ?? (_ => true))
                .AsNoTracking();
        }

        private IQueryable<T> BuildTrackedQuery(
            Expression<Func<T, bool>>? filter = null,
            string[]? includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            query = query.ApplyIncludes(includes);

            return query.Where(filter ?? (_ => true));
        }
    }
}