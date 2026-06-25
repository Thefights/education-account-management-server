using Enums;

namespace Infrastructure.Interface
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        int? CurrentUserId { get; }

        UserRole Role { get; }

        string UserName { get; }
    }
}
