using DTOs.Auth;

namespace Validators
{
    public class RegisterRequestValidator : IValidator<RegisterRequestDTO>
    {
        public void Validate(RegisterRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (string.IsNullOrWhiteSpace(entity.Password))
                throw new InvalidDataException("Password cannot be null, empty, or whitespace.");

            if (entity.Password != entity.ConfirmPassword)
                throw new InvalidDataException("Confirm password must match password.");

            if (string.IsNullOrWhiteSpace(entity.EmailVerificationSessionId))
                throw new InvalidDataException("Email verification session ID cannot be null, empty, or whitespace.");
        }
    }
}
