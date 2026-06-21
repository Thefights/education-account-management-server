using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : Entity
    {
        IQueryable<T> Query(bool tracking = false, bool ignoreQueryFilters = false);

        Task<List<TResult>> GetProjectedAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> projection, Expression<Func<T, bool>>? filter = null, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<TResult?> FirstOrDefaultProjectedAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> projection, Expression<Func<T, bool>>? filter = null, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<List<TResult>> GetProjectedFilteredAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> projection, string? filter, string? search, string[]? searchFields, string order, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<(int Count, List<TResult> Items)> GetProjectedPaginatedAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> projection, string? filter, string? search, string[]? searchFields, string order, int page, int pageSize, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<(int Count, List<TResult> Items)> GetProjectedPaginatedAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> projection, Expression<Func<T, bool>>? filter, string order, int page, int pageSize, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<(int Count, List<TResult> Items)> GetProjectedPaginatedAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> projection, Expression<Func<T, bool>>? filterExpr, string? filterStr, string order, int page, int pageSize, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<(int Count, List<TResult> Items)> GetProjectedPaginatedAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> projection, Expression<Func<T, bool>>? filterExpr, string? filterStr, string? search, string[]? searchFields, string order, int page, int pageSize, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<List<T>> GetByIdsAsync(List<int> ids, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<List<T>> GetTrackedByIdsAsync(List<int> ids, string[]? includes = null, CancellationToken cancellationToken = default);

        Task<T?> GetTrackedByIdAsync(int id, string[]? includes = null, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);

        void Remove(T entity);
        void RemoveRange(List<T> entities);

        T Update(T entity);
        void UpdateRange(List<T> entities);

        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    }
}
