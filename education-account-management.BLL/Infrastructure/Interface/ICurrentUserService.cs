using Enums;

namespace Infrastructure.Interface
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        int AuthId { get; }

        UserRole Role { get; }
    }
}
