namespace Repositories.Interfaces
{
    public interface IAuditUserContext
    {
        int? CurrentUserId { get; }
    }
}
