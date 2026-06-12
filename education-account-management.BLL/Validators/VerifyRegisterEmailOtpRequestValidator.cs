using DTOs.Auth;
using EntityAnnotations.RegExAttributes;
using System.Text.RegularExpressions;

namespace Validators
{
    public partial class VerifyRegisterEmailOtpRequestValidator : IValidator<VerifyRegisterEmailOtpRequestDTO>
    {
        public void Validate(VerifyRegisterEmailOtpRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (string.IsNullOrWhiteSpace(entity.SessionId))
            {
                throw new InvalidDataException("Email verification session ID cannot be null, empty, or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(entity.Email))
            {
                throw new InvalidDataException("Email cannot be null, empty, or whitespace.");
            }

            if (!IsValidEmail(entity.Email))
            {
                throw new InvalidDataException("Email must be valid.");
            }

            if (string.IsNullOrWhiteSpace(entity.OtpCode))
            {
                throw new InvalidDataException("OTP code cannot be null, empty, or whitespace.");
            }

            if (!OtpCodeRegex().IsMatch(entity.OtpCode))
            {
                throw new InvalidDataException("OTP code must be exactly 6 digits.");
            }
        }

        [GeneratedRegex(@"^\d{6}$")]
        private static partial Regex OtpCodeRegex();

        private static bool IsValidEmail(string email)
        {
            var normalizedEmail = EmailWhitelistValueUtil.Normalize(email);
            return new EmailValidatorAttribute().IsValid(normalizedEmail);
        }
    }
}
