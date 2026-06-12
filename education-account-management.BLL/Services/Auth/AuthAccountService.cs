using DTOs.Auth;
using Interfaces.Audit;
using Interfaces.Auth;
using Interfaces.Storage;

namespace Services.Auth
{
    public class AuthAccountService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        AuthAccountMapper authAccountMapper,
        IUploadService uploadService,
        IAuditLogWriter auditLogWriter)
        : IAuthAccountService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly AuthAccountMapper _authAccountMapper = authAccountMapper;
        private readonly IUploadService _uploadService = uploadService;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IGenericRepository<AuthAccount> _authAccountRepository = unitOfWork.Repository<AuthAccount>();
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();

        public async Task<GetAuthAccountProfileDTO> GetCurrentAuthAccountAsync(CancellationToken cancellationToken = default)
        {
            return await _authAccountRepository.FirstOrDefaultProjectedAsync(
                    _authAccountMapper.ProjectToGetProfileDTO,
                    authAccount => authAccount.Id == _currentUserService.AuthId,
                    cancellationToken: cancellationToken)
                ?? throw new DataNotFoundException(typeof(AuthAccount), _currentUserService.AuthId);
        }

        public async Task<GetAuthAccountProfileDTO> UpdateCurrentAuthAccountAsync(
            UpdateAuthAccountProfileDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    var authAccount = await _authAccountRepository
                        .Query(tracking: true)
                        .Include(account => account.User)
                        .FirstOrDefaultAsync(account => account.Id == _currentUserService.AuthId, token)
                        ?? throw new DataNotFoundException(typeof(AuthAccount), _currentUserService.AuthId);

                    var oldImageUrl = authAccount.User.ImageUrl;
                    var uploadedImageUrl = await ImageUploadHelper.UploadIfPresentAsync(
                        _uploadService,
                        updateDTO,
                        nameof(User),
                        token);
                    ImageTransactionHookHelper.RegisterUploadedImageRollback(
                        transaction,
                        _uploadService,
                        uploadedImageUrl);
                    ImageTransactionHookHelper.RegisterOldImageDeleteAfterCommit(
                        transaction,
                        _uploadService,
                        oldImageUrl,
                        uploadedImageUrl);

                    updateDTO.UserIdText = updateDTO.UserIdText?.Trim() ?? authAccount.UserIdText;
                    _authAccountMapper.MapFromUpdateProfileDTO(updateDTO, authAccount);

                    if (!string.IsNullOrWhiteSpace(uploadedImageUrl))
                    {
                        authAccount.User.ImageUrl = uploadedImageUrl;
                    }

                    authAccount.TryValidate();
                    authAccount.User.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(
                        _userRepository,
                        authAccount.User,
                        authAccount.User.Id,
                        token);
                    await UniqueConstraintValidator.ValidateAsync(
                        _authAccountRepository,
                        authAccount,
                        authAccount.Id,
                        token);

                    _authAccountRepository.Update(authAccount);
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.AccountManagement,
                        AuditLogAction.UpdateAccount,
                        $"AuthAccount:{authAccount.Id}:{authAccount.UserIdText}",
                        token);
                },
                cancellationToken);

            return await GetCurrentAuthAccountAsync(cancellationToken);
        }
    }
}
