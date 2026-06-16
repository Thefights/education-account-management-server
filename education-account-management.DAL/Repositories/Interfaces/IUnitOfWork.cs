using Common;
using Persistence.SqlServer.Transactions;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : Entity;

        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);

        Task ExecuteInTransactionAsync(
            Func<IUnitOfWorkTransaction, CancellationToken, Task> action,
            CancellationToken cancellationToken = default);

        Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<IUnitOfWorkTransaction, CancellationToken, Task<TResult>> action,
            CancellationToken cancellationToken = default);
    }
}