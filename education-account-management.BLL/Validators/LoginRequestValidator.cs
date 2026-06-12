using DTOs.Auth;

namespace Validators
{
    public class LoginRequestValidator : IValidator<LoginRequestDTO>
    {
        public void Validate(LoginRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (string.IsNullOrWhiteSpace(entity.UserId))
                throw new InvalidDataException("User ID cannot be null, empty, or whitespace.");

            if (string.IsNullOrWhiteSpace(entity.Password))
                throw new InvalidDataException("Password cannot be null, empty, or whitespace.");
        }
    }
}
