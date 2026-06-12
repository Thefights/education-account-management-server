using DTOs.Auth;

namespace Validators
{
    public class SocialLoginRequestValidator : IValidator<SocialLoginRequestDTO>
    {
        public void Validate(SocialLoginRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (!Enum.IsDefined(entity.Provider))
            {
                throw new InvalidDataException("Social login provider must be valid.");
            }

            if (string.IsNullOrWhiteSpace(entity.ProviderToken))
            {
                throw new InvalidDataException("Provider token cannot be null, empty, or whitespace.");
            }
        }
    }
}
