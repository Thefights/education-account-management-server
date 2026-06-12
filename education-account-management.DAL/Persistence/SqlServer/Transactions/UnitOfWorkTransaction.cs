using Microsoft.Extensions.Logging;

namespace Persistence.SqlServer.Transactions
{
    internal sealed class UnitOfWorkTransaction(ILogger logger) : IUnitOfWorkTransaction
    {
        private readonly ILogger _logger = logger;
        private readonly List<Func<Task>> _rollbackActions = [];
        private readonly List<Func<Task>> _afterCommitActions = [];

        public void OnRollback(Func<Task> action)
        {
            _rollbackActions.Add(action);
        }

        public void AfterCommit(Func<Task> action)
        {
            _afterCommitActions.Add(action);
        }

        public async Task ExecuteRollbackAsync()
        {
            foreach (var action in _rollbackActions)
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Transaction rollback hook failed.");
                }
            }
        }

        public async Task ExecuteAfterCommitAsync()
        {
            foreach (var action in _afterCommitActions)
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Transaction after-commit hook failed.");
                }
            }
        }
    }
}
