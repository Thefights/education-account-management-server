namespace Persistence.SqlServer.Transactions
{
    public interface IUnitOfWorkTransaction
    {
        void OnRollback(Func<Task> action);
        void AfterCommit(Func<Task> action);
    }
}
