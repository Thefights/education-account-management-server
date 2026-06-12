namespace Infrastructure.Interface
{
    public interface ICurrentTokenService
    {
        string? AccessToken { get; }
    }
}
