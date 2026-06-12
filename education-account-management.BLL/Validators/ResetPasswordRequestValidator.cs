using DTOs.Auth;
using System.Text.RegularExpressions;

namespace Validators
{
    public partial class ResetPasswordRequestValidator() : IValidator<ResetPasswordRequestDTO>
    {
        public void Validate(ResetPasswordRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (string.IsNullOrWhiteSpace(entity.Token))
                throw new InvalidDataException("Reset token cannot be null, empty, or whitespace.");

            if (string.IsNullOrWhiteSpace(entity.NewPassword))
                throw new InvalidDataException("New password cannot be null, empty, or whitespace.");

            if (entity.NewPassword != entity.ConfirmPassword)
                throw new InvalidDataException("Confirm password must match new password.");

            if (!PlainPasswordRegex().IsMatch(entity.NewPassword))
                throw new InvalidDataException("Password must be at least 8 characters, include upper/lowercase, number and special character.");
        }

        [GeneratedRegex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        private static partial Regex PlainPasswordRegex();
    }
}
