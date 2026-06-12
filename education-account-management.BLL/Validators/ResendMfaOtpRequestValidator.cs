using DTOs.Auth;

namespace Validators
{
    public class ResendMfaOtpRequestValidator : IValidator<ResendMfaOtpRequestDTO>
    {
        public void Validate(ResendMfaOtpRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (string.IsNullOrWhiteSpace(entity.SessionId))
            {
                throw new InvalidDataException("MFA session ID cannot be null, empty, or whitespace.");
            }
        }
    }
}
