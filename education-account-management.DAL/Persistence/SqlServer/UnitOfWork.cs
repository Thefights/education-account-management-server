using Common;
using education_account_management.DAL.Persistence.SqlServer.Transactions;
using Microsoft.Extensions.Logging;
using Persistence.SqlServer.Transactions;
using Repositories.Interfaces;

namespace Persistence.SqlServer
{
    public class UnitOfWork(
        DbContext dbContext,
        ILogger<UnitOfWork> logger) : IUnitOfWork
    {
        private readonly DbContext _dbContext = dbContext;
        private readonly ILogger<UnitOfWork> _logger = logger;
        private readonly Dictionary<Type, dynamic> _repositories = [];

        #region Repository
        public IGenericRepository<T> Repository<T>() where T : Entity
        {
            var entityType = typeof(T);

            if (_repositories.TryGetValue(entityType, out dynamic? repository))
            {
                return repository;
            }

            var newRepository = Activator.CreateInstance(
                typeof(GenericRepository<>).MakeGenericType(typeof(T)),
                _dbContext
            );

            if (newRepository == null)
            {
                throw new NullReferenceException("Repository should not be null");
            }

            _repositories.Add(entityType, newRepository);

            return (IGenericRepository<T>)newRepository;
        }
        #endregion

        #region Save Changes
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Transaction
        public async Task ExecuteInTransactionAsync(
            Func<IUnitOfWorkTransaction, CancellationToken, Task> action,
            CancellationToken cancellationToken = default)
        {
            await ExecuteInTransactionAsync<object?>(
                async (transaction, token) =>
                {
                    await action(transaction, token);
                    return null;
                },
                cancellationToken);
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<IUnitOfWorkTransaction, CancellationToken, Task<TResult>> action,
            CancellationToken cancellationToken = default)
        {
            var transactionContext = new UnitOfWorkTransaction(_logger);
            await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            TResult result;
            try
            {
                result = await action(transactionContext, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await dbTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                try
                {
                    await dbTransaction.RollbackAsync(CancellationToken.None);
                }
                finally
                {
                    await transactionContext.ExecuteRollbackAsync();
                }

                throw;
            }

            await transactionContext.ExecuteAfterCommitAsync();
            return result;
        }
        #endregion
    }
}
