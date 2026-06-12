using DTOs.Auth;
using EntityAnnotations.RegExAttributes;

namespace Validators
{
    public class SendRegisterEmailOtpRequestValidator : IValidator<SendRegisterEmailOtpRequestDTO>
    {
        public void Validate(SendRegisterEmailOtpRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (string.IsNullOrWhiteSpace(entity.Email))
            {
                throw new InvalidDataException("Email cannot be null, empty, or whitespace.");
            }

            if (!IsValidEmail(entity.Email))
            {
                throw new InvalidDataException("Email must be valid.");
            }
        }

        private static bool IsValidEmail(string email)
        {
            var normalizedEmail = EmailWhitelistValueUtil.Normalize(email);
            return new EmailValidatorAttribute().IsValid(normalizedEmail);
        }
    }
}
