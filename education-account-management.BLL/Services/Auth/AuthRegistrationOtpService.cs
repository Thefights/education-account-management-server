using DTOs.Auth;
using Interfaces.Audit;
using Interfaces.Auth;
using Security;
using System.Security.Cryptography;

namespace Services.Auth
{
    public class AuthRegistrationOtpService(
        IUnitOfWork unitOfWork,
        IAuthEmailService authEmailService,
        IAuditLogWriter auditLogWriter,
        IValidator<SendRegisterEmailOtpRequestDTO> sendRegisterEmailOtpValidator,
        IValidator<VerifyRegisterEmailOtpRequestDTO> verifyRegisterEmailOtpValidator)
        : IAuthRegistrationOtpService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthEmailService _authEmailService = authEmailService;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IValidator<SendRegisterEmailOtpRequestDTO> _sendRegisterEmailOtpValidator = sendRegisterEmailOtpValidator;
        private readonly IValidator<VerifyRegisterEmailOtpRequestDTO> _verifyRegisterEmailOtpValidator = verifyRegisterEmailOtpValidator;
        private readonly IGenericRepository<OtpVerification> _otpVerificationRepository = unitOfWork.Repository<OtpVerification>();

        private const int MaxFailedOtpAttempts = 5;
        private static readonly TimeSpan RegisterEmailOtpLifetime = TimeSpan.FromMinutes(5);

        public async Task<SendRegisterEmailOtpResponseDTO> SendEmailOtpAsync(
            SendRegisterEmailOtpRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            _sendRegisterEmailOtpValidator.Validate(request);

            var now = DateTime.UtcNow;
            var normalizedEmail = EmailWhitelistValueUtil.Normalize(request.Email);
            var activeOtps = await _otpVerificationRepository
                .Query(tracking: true)
                .Where(otp => otp.Target == normalizedEmail
                    && otp.Purpose == OtpVerificationPurpose.RegistrationEmailVerification
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
                Purpose = OtpVerificationPurpose.RegistrationEmailVerification,
                DeliveryMethod = OtpVerificationDeliveryMethod.Email,
                SessionId = Guid.NewGuid().ToString("N"),
                Target = normalizedEmail,
                OtpHash = TokenUtil.HashToken(code),
                FailedAttemptCount = 0,
                ExpiresAt = now.Add(RegisterEmailOtpLifetime)
            };

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    await _otpVerificationRepository.AddAsync(otpVerification, token);
                    await _auditLogWriter.LogAnonymousAsync(
                        AuditLogCategory.Authentication,
                        AuditLogAction.SendRegisterEmailOtp,
                        $"Email:{normalizedEmail}",
                        normalizedEmail,
                        normalizedEmail,
                        cancellationToken: token);

                    await _authEmailService.SendOtpEmailAsync(
                        normalizedEmail,
                        code,
                        otpVerification.ExpiresAt,
                        token);
                },
                cancellationToken);

            return new SendRegisterEmailOtpResponseDTO
            {
                SessionId = otpVerification.SessionId,
                ExpiresAt = otpVerification.ExpiresAt
            };
        }

        public async Task VerifyEmailOtpAsync(
            VerifyRegisterEmailOtpRequestDTO request,
            CancellationToken cancellationToken = default)
        {
            _verifyRegisterEmailOtpValidator.Validate(request);

            var now = DateTime.UtcNow;
            var normalizedEmail = EmailWhitelistValueUtil.Normalize(request.Email);
            var otpVerification = await this.GetRegistrationEmailOtpQuery(tracking: true)
                .FirstOrDefaultAsync(
                    otp => otp.SessionId == request.SessionId
                        && otp.Target == normalizedEmail,
                    cancellationToken);

            if (otpVerification == null
                || otpVerification.UsedAt != null
                || otpVerification.InvalidatedAt != null
                || otpVerification.ExpiresAt <= now)
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
            await _auditLogWriter.LogAnonymousAsync(
                AuditLogCategory.Authentication,
                AuditLogAction.VerifyRegisterEmailOtp,
                $"Email:{normalizedEmail}",
                normalizedEmail,
                normalizedEmail,
                cancellationToken: cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task EnsureEmailVerifiedAsync(
            string? email,
            string? sessionId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(sessionId))
            {
                throw new InvalidDataException("Email must be verified before registration.");
            }

            var normalizedEmail = EmailWhitelistValueUtil.Normalize(email);
            var otpVerification = await this.GetRegistrationEmailOtpQuery(tracking: false)
                .FirstOrDefaultAsync(
                    otp => otp.SessionId == sessionId.Trim()
                        && otp.Target == normalizedEmail,
                    cancellationToken);

            if (otpVerification == null
                || otpVerification.UsedAt == null
                || otpVerification.InvalidatedAt != null)
            {
                throw new InvalidDataException("Email must be verified before registration.");
            }
        }

        private IQueryable<OtpVerification> GetRegistrationEmailOtpQuery(bool tracking)
        {
            return _otpVerificationRepository
                .Query(tracking: tracking)
                .Where(otp => otp.Purpose == OtpVerificationPurpose.RegistrationEmailVerification
                    && otp.DeliveryMethod == OtpVerificationDeliveryMethod.Email);
        }
    }
}
