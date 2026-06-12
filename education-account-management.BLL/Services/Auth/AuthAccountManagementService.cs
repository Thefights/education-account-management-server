using DTOs.Auth;
using DTOs.Csv;
using Interfaces.Audit;
using Interfaces.Auth;
using Interfaces.Storage;
using Security;
using Services.Base;
using Services.Csv;

namespace Services.Auth
{
    public class AuthAccountManagementService(
        IUnitOfWork unitOfWork,
        AuthAccountMapper authAccountMapper,
        IUploadService uploadService,
        IAuthEmailService authEmailService,
        IAuditLogWriter auditLogWriter,
        AuthAccountCsvImportService authAccountCsvImportService)
        : BaseService<AuthAccount, CreateAuthAccountDTO, GetAuthAccountDTO, UpdateAuthAccountDTO>(
            unitOfWork,
            authAccountMapper,
            uploadService,
            AuthAccountIncludes),
            IAuthAccountManagementService
    {
        private static readonly string[] AuthAccountIncludes = [nameof(AuthAccount.User)];

        private static readonly TimeSpan PasswordResetTokenLifetime = TimeSpan.FromHours(1);

        private readonly AuthAccountMapper _authAccountMapper = authAccountMapper;
        private readonly AuthAccountCsvImportService _authAccountCsvImportService = authAccountCsvImportService;

        private readonly IUploadService _authAccountUploadService = uploadService;
        private readonly IAuthEmailService _authEmailService = authEmailService;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;

        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<Role> _roleRepository = unitOfWork.Repository<Role>();

        public override async Task<GetAuthAccountDTO> CreateAsync(
            CreateAuthAccountDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            createDTO.TryValidate();

            await EnsureRolesExistAsync(createDTO.RoleIds, cancellationToken);

            var generatedPassword = PasswordHashUtil.GenerateTemporaryPassword();
            var resetToken = TokenUtil.GenerateRefreshToken();
            var resetTokenExpiresAt = DateTime.UtcNow.Add(PasswordResetTokenLifetime);

            var authAccount = new AuthAccount
            {
                UserIdText = createDTO.UserIdText!,
                Email = createDTO.Email!,
                PasswordHash = PasswordHashUtil.Hash(generatedPassword),
                FailedLoginCount = 0,
                User = new User
                {
                    FullName = createDTO.FullName!,
                    PhoneNumber = createDTO.PhoneNumber,
                    Gender = createDTO.Gender,
                    UserRoles = createDTO.RoleIds
                        .Distinct()
                        .Select(roleId => new UserRole { RoleId = roleId })
                        .ToList()
                },
                PasswordResetTokens =
                [
                    new PasswordResetToken
                    {
                        TokenHash = TokenUtil.HashToken(resetToken),
                        ExpiresAt = resetTokenExpiresAt
                    }
                ]
            };

            authAccount.TryValidate();
            authAccount.User.TryValidate();
            await UniqueConstraintValidator.ValidateAsync(_repository, authAccount, cancellationToken: cancellationToken);
            await UniqueConstraintValidator.ValidateAsync(_userRepository, authAccount.User, cancellationToken: cancellationToken);

            var authAccountId = await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    var avatarUrl = await ImageUploadHelper.UploadIfPresentAsync(
                        _authAccountUploadService,
                        createDTO,
                        nameof(User),
                        token);
                    ImageTransactionHookHelper.RegisterUploadedImageRollback(
                        transaction,
                        _authAccountUploadService,
                        avatarUrl);
                    if (!string.IsNullOrWhiteSpace(avatarUrl))
                    {
                        authAccount.User.ImageUrl = avatarUrl;
                        authAccount.User.TryValidate();
                    }

                    var resultEntity = await _repository.AddAsync(authAccount, token);
                    await _unitOfWork.SaveChangeAsync(token);

                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.AccountManagement,
                        AuditLogAction.CreateAccount,
                        $"AuthAccount:{resultEntity.Id}:{resultEntity.UserIdText}",
                        token);

                    await _authEmailService.SendPasswordResetEmailAsync(
                        resultEntity.Email,
                        resetToken,
                        resetTokenExpiresAt,
                        token);

                    return resultEntity.Id;
                },
                cancellationToken);

            return await GetByIdAsync(authAccountId, cancellationToken);
        }

        public async Task<BatchImportResultDTO> BatchImportAsync(
            IFormFile file,
            bool sendEmail = true,
            CancellationToken cancellationToken = default)
        {
            var result = await _authAccountCsvImportService.ImportAsync(file, sendEmail, cancellationToken);
            if (result.Succeeded > 0)
            {
                await _auditLogWriter.LogAsync(
                    AuditLogCategory.AccountManagement,
                    AuditLogAction.ImportAccounts,
                    $"AuthAccounts:{result.Succeeded}",
                    cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }

            return result;
        }

        public override async Task<GetAuthAccountDTO> UpdateAsync(
            int id,
            UpdateAuthAccountDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            updateDTO.TryValidate();

            await EnsureRolesExistAsync(updateDTO.RoleIds, cancellationToken);

            var authAccount = await _repository
                .Query(tracking: true)
                .Include(account => account.User)
                .ThenInclude(user => user.UserRoles)
                .FirstOrDefaultAsync(account => account.Id == id, cancellationToken)
                ?? throw new DataNotFoundException(typeof(AuthAccount), id);

            var oldImageUrl = authAccount.User.ImageUrl;

            _authAccountMapper.MapFromUpdateDTO(updateDTO, authAccount);
            authAccount.User.FullName = updateDTO.FullName!;
            authAccount.User.PhoneNumber = updateDTO.PhoneNumber;
            authAccount.User.Gender = updateDTO.Gender;
            ReplaceUserRoles(authAccount.User, updateDTO.RoleIds);

            authAccount.TryValidate();
            authAccount.User.TryValidate();
            await UniqueConstraintValidator.ValidateAsync(_repository, authAccount, authAccount.Id, cancellationToken);
            await UniqueConstraintValidator.ValidateAsync(_userRepository, authAccount.User, authAccount.User.Id, cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    var uploadedImageUrl = await ImageUploadHelper.UploadIfPresentAsync(
                        _authAccountUploadService,
                        updateDTO,
                        nameof(User),
                        token);
                    ImageTransactionHookHelper.RegisterUploadedImageRollback(
                        transaction,
                        _authAccountUploadService,
                        uploadedImageUrl);
                    ImageTransactionHookHelper.RegisterOldImageDeleteAfterCommit(
                        transaction,
                        _authAccountUploadService,
                        oldImageUrl,
                        uploadedImageUrl);
                    if (!string.IsNullOrWhiteSpace(uploadedImageUrl))
                    {
                        authAccount.User.ImageUrl = uploadedImageUrl;
                        authAccount.User.TryValidate();
                    }

                    _repository.Update(authAccount);
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.AccountManagement,
                        AuditLogAction.UpdateAccount,
                        $"AuthAccount:{authAccount.Id}:{authAccount.UserIdText}",
                        token);
                },
                cancellationToken);

            return await GetByIdAsync(authAccount.Id, cancellationToken);
        }

        public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var authAccount = await _repository
                .Query(tracking: true)
                .Include(account => account.User)
                .FirstOrDefaultAsync(account => account.Id == id, cancellationToken)
                ?? throw new DataNotFoundException(typeof(AuthAccount), id);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    ImageTransactionHookHelper.RegisterImageDeleteAfterCommit(
                        transaction,
                        _authAccountUploadService,
                        authAccount.User.ImageUrl);

                    _repository.Remove(authAccount);
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.AccountManagement,
                        AuditLogAction.DeleteAccount,
                        $"AuthAccount:{authAccount.Id}:{authAccount.UserIdText}",
                        token);
                },
                cancellationToken);
        }

        public override async Task DeleteSelectedIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            var authAccounts = await _repository
                .Query(tracking: true)
                .Include(account => account.User)
                .Where(account => ids.Contains(account.Id))
                .ToListAsync(cancellationToken);

            if (authAccounts.Count != ids.Count)
            {
                var foundIds = authAccounts.Select(authAccount => authAccount.Id).ToHashSet();
                var firstNotFoundId = ids.FirstOrDefault(id => !foundIds.Contains(id));
                throw new DataNotFoundException(typeof(AuthAccount), firstNotFoundId);
            }

            await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    foreach (var imageUrl in authAccounts.Select(authAccount => authAccount.User.ImageUrl))
                    {
                        ImageTransactionHookHelper.RegisterImageDeleteAfterCommit(
                            transaction,
                            _authAccountUploadService,
                            imageUrl);
                    }

                    _repository.RemoveRange(authAccounts);
                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.AccountManagement,
                        AuditLogAction.DeleteAccounts,
                        $"AuthAccounts:{authAccounts.Count}",
                        token);
                },
                cancellationToken);
        }

        public async Task<List<GetAuthAccountDTO>> UpdateAuthAccountStatusAsync(
            UpdateAuthAccountsStatusDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            updateDTO.TryValidate();

            var authAccountIds = updateDTO.AuthAccountIds.Distinct().ToList();
            await EnsureAuthAccountsExistAsync(authAccountIds, cancellationToken);

            var authAccounts = await _repository.GetTrackedByIdsAsync(authAccountIds, cancellationToken: cancellationToken);

            foreach (var authAccount in authAccounts)
            {
                authAccount.Status = updateDTO.Status;
            }

            if (authAccounts.Count == 1)
            {
                _repository.Update(authAccounts.Single());
            }
            else
            {
                _repository.UpdateRange(authAccounts);
            }

            await _auditLogWriter.LogAsync(
                AuditLogCategory.AccountManagement,
                AuditLogAction.UpdateAccountStatus,
                $"AuthAccounts:{authAccounts.Count}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return await GetAllByIdsAsync(authAccountIds, cancellationToken);
        }

        public async Task<List<GetAuthAccountDTO>> UnlockAuthAccountsAsync(
            UnlockAuthAccountsDTO unlockDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(unlockDTO);
            unlockDTO.TryValidate();

            var authAccountIds = unlockDTO.AuthAccountIds.Distinct().ToList();
            await EnsureAuthAccountsExistAsync(authAccountIds, cancellationToken);

            var authAccounts = await _repository.GetTrackedByIdsAsync(
                authAccountIds,
                cancellationToken: cancellationToken);

            foreach (var authAccount in authAccounts)
            {
                authAccount.FailedLoginCount = 0;
                authAccount.LockedUntil = null;
            }

            if (authAccounts.Count == 1)
            {
                _repository.Update(authAccounts.Single());
            }
            else
            {
                _repository.UpdateRange(authAccounts);
            }

            await _auditLogWriter.LogAsync(
                AuditLogCategory.AccountManagement,
                AuditLogAction.UnlockAccount,
                $"AuthAccounts:{authAccounts.Count}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return await GetAllByIdsAsync(authAccountIds, cancellationToken);
        }

        private async Task EnsureRolesExistAsync(List<int> roleIds, CancellationToken cancellationToken)
        {
            var distinctRoleIds = roleIds.Distinct().ToList();
            if (distinctRoleIds.Count == 0)
            {
                throw new InvalidDataException("At least one role is required.");
            }

            var existingRoleCount = await _roleRepository.CountAsync(
                role => distinctRoleIds.Contains(role.Id),
                cancellationToken);
            if (existingRoleCount != distinctRoleIds.Count)
            {
                throw new DataNotFoundException("One or more roles were not found.");
            }
        }

        private static void ReplaceUserRoles(User user, List<int> roleIds)
        {
            var targetRoleIds = roleIds.Distinct().ToHashSet();

            var removedRoles = user.UserRoles
                .Where(userRole => !targetRoleIds.Contains(userRole.RoleId))
                .ToList();
            foreach (var removedRole in removedRoles)
            {
                user.UserRoles.Remove(removedRole);
            }

            var existingRoleIds = user.UserRoles.Select(userRole => userRole.RoleId).ToHashSet();
            foreach (var roleId in targetRoleIds.Where(roleId => !existingRoleIds.Contains(roleId)))
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                });
            }
        }

        private async Task EnsureAuthAccountsExistAsync(
            List<int> authAccountIds,
            CancellationToken cancellationToken)
        {
            if (authAccountIds.Count == 0)
            {
                throw new InvalidDataException("At least one auth account is required.");
            }

            var existingAuthAccountCount = await _repository.CountAsync(
                authAccount => authAccountIds.Contains(authAccount.Id),
                cancellationToken);
            if (existingAuthAccountCount != authAccountIds.Count)
            {
                throw new DataNotFoundException("One or more auth accounts were not found.");
            }
        }
    }
}
