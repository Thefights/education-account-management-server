using DTOs.Auth;
using Interfaces.Auth;
using Security;
using System.Security.Cryptography;

namespace Services.Auth
{
    public class AuthMfaService(
        IUnitOfWork unitOfWork,
        IValidator<ResendMfaOtpRequestDTO> resendMfaOtpValidator,
        IValidator<VerifyMfaOtpRequestDTO> verifyMfaOtpValidator)
        : IAuthMfaService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IValidator<ResendMfaOtpRequestDTO> _resendMfaOtpValidator = resendMfaOtpValidator;
        private readonly IValidator<VerifyMfaOtpRequestDTO> _verifyMfaOtpValidator = verifyMfaOtpValidator;
        private readonly IGenericRepository<OtpVerification> _otpVerificationRepository = unitOfWork.Repository<OtpVerification>();

        private const int MaxFailedOtpAttempts = 5;
        private static readonly TimeSpan MfaOtpLifetime = TimeSpan.FromMinutes(3);

        public async Task<LoginMfaOtpDTO> CreateLoginMfaOtpAsync(
            AuthAccount authAccount,
            DateTime now,
            CancellationToken cancellationToken = default)
        {
            var activeOtps = await _otpVerificationRepository
                .Query(tracking: true)
                .Where(otp => otp.AuthAccountId == authAccount.Id
                    && otp.Purpose == OtpVerificationPurpose.LoginMfaEmail
                    && otp.DeliveryMethod == OtpVerificationDeliveryMethod.Email
                    && otp.UsedAt == null
                    && otp.InvalidatedAt == null
                    && otp.ExpiresAt > now)
                .ToListAsync(cancellationToken);

            foreach (var activeOtp in activeOtps)
            {
                activeOtp.InvalidatedAt = now;
            }

            var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            var otpVerification = new OtpVerification
            {
                AuthAccountId = authAccount.Id,
                Purpose = OtpVerificationPurpose.LoginMfaEmail,
                DeliveryMethod = OtpVerificationDeliveryMethod.Email,
                SessionId = Guid.NewGuid().ToString("N"),
                Target = authAccount.Email,
                OtpHash = TokenUtil.HashToken(code),
                FailedAttemptCount = 0,
                ExpiresAt = now.Add(MfaOtpLifetime)
            };

            await _otpVerificationRepository.AddAsync(otpVerification, cancellationToken);

            return new LoginMfaOtpDTO
            {
                Code = code,
                SessionId = otpVerification.SessionId,
                Target = otpVerification.Target,
                ExpiresAt = otpVerification.ExpiresAt
            };
        }

        public async Task<LoginMfaOtpDTO> ResendLoginMfaOtpAsync(
            ResendMfaOtpRequestDTO request,
            DateTime now,
            CancellationToken cancellationToken = default)
        {
            _resendMfaOtpValidator.Validate(request);

            var otpVerification = await _otpVerificationRepository
                .Query(tracking: true)
                .Include(otp => otp.AuthAccount)
                .ThenInclude(authAccount => authAccount!.User)
                .FirstOrDefaultAsync(
                    otp => otp.SessionId == request.SessionId!.Trim()
                        && otp.Purpose == OtpVerificationPurpose.LoginMfaEmail
                        && otp.DeliveryMethod == OtpVerificationDeliveryMethod.Email,
                    cancellationToken);

            if (otpVerification == null
                || otpVerification.UsedAt != null
                || otpVerification.InvalidatedAt != null
                || otpVerification.AuthAccount == null)
            {
                throw new UnauthorizedAccessException("Invalid or expired MFA session.");
            }

            if (otpVerification.AuthAccount.Status == AuthAccountStatus.Inactive
                || otpVerification.AuthAccount.IsDeleted
                || otpVerification.AuthAccount.User.IsDeleted)
            {
                throw new UnauthorizedAccessException("Invalid or expired MFA session.");
            }

            otpVerification.InvalidatedAt = now;
            return await this.CreateLoginMfaOtpAsync(
                otpVerification.AuthAccount,
                now,
                cancellationToken);
        }

        public async Task<OtpVerification> VerifyLoginMfaOtpAsync(
            VerifyMfaOtpRequestDTO request,
            DateTime now,
            CancellationToken cancellationToken = default)
        {
            _verifyMfaOtpValidator.Validate(request);

            var otpVerification = await _otpVerificationRepository
                .Query(tracking: true)
                .Include(otp => otp.AuthAccount)
                .ThenInclude(authAccount => authAccount!.User)
                .ThenInclude(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .FirstOrDefaultAsync(
                    otp => otp.SessionId == request.SessionId
                        && otp.Purpose == OtpVerificationPurpose.LoginMfaEmail
                        && otp.DeliveryMethod == OtpVerificationDeliveryMethod.Email,
                    cancellationToken);

            if (otpVerification == null
                || otpVerification.UsedAt != null
                || otpVerification.InvalidatedAt != null
                || otpVerification.ExpiresAt <= now
                || otpVerification.AuthAccount == null)
            {
                throw new UnauthorizedAccessException("Invalid or expired OTP code.");
            }

            if (otpVerification.AuthAccount.Status == AuthAccountStatus.Inactive
                || otpVerification.AuthAccount.IsDeleted
                || otpVerification.AuthAccount.User.IsDeleted)
            {
                throw new UnauthorizedAccessException("Invalid or expired OTP code.");
            }

            if (TokenUtil.HashToken(request.OtpCode!) != otpVerification.OtpHash)
            {
                otpVerification.FailedAttemptCount++;
                if (otpVerification.FailedAttemptCount >= MaxFailedOtpAttempts)
                {
                    otpVerification.InvalidatedAt = now;
                }

                await _unitOfWork.SaveChangeAsync(cancellationToken);
                throw new UnauthorizedAccessException("Invalid or expired OTP code.");
            }

            otpVerification.UsedAt = now;
            return otpVerification;
        }
    }
}