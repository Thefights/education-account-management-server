using Enums;

namespace Infrastructure.Interface
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        UserRole Role { get; }

        string UserName { get; }
    }
}
