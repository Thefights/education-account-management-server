namespace Infrastructure.Interface
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        int AuthId { get; }

        RoleEnum Role { get; }
    }
}
