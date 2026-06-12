using DTOs.Auth;
using System.Text.RegularExpressions;

namespace Validators
{
    public partial class VerifyMfaOtpRequestValidator : IValidator<VerifyMfaOtpRequestDTO>
    {
        public void Validate(VerifyMfaOtpRequestDTO entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (string.IsNullOrWhiteSpace(entity.SessionId))
            {
                throw new InvalidDataException("MFA session ID cannot be null, empty, or whitespace.");
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
    }
}
