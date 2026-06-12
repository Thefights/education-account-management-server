namespace Interfaces.Email
{
    public interface IEmailWhitelistPolicyService
    {
        Task<bool> CanSendAsync(string email, CancellationToken cancellationToken = default);
    }
}
