using DTOs.AdminManagement;
using Interfaces.AdminManagement;
using Interfaces.Audit;
using Results;
using System.Linq.Expressions;
using Validators;

namespace Services.AdminManagement
{
    public class AdminManagementService(
        IUnitOfWork unitOfWork,
        AdminManagementMapper mapper,
        IAuditLogWriter auditLogWriter)
        : IAdminManagementService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AdminManagementMapper _mapper = mapper;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<AuthAccount> _authAccountRepository = unitOfWork.Repository<AuthAccount>();
        private readonly IGenericRepository<SsoIdentity> _ssoIdentityRepository = unitOfWork.Repository<SsoIdentity>();
        private readonly IGenericRepository<AdminProfile> _adminProfileRepository = unitOfWork.Repository<AdminProfile>();
        private readonly IGenericRepository<School> _schoolRepository = unitOfWork.Repository<School>();

        public async Task<GetAdminManagementDTO> CreateAsync(
            CreateAdminManagementDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            await ValidateRequestAsync(createDTO.Role, createDTO.SchoolId, createDTO.AzureObjectId, cancellationToken);

            var userId = await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var authAccount = new AuthAccount
                    {
                        Status = createDTO.Status
                    };
                    authAccount.TryValidate();
                    await _authAccountRepository.AddAsync(authAccount, token);
                    await _unitOfWork.SaveChangeAsync(token);

                    var identity = new SsoIdentity
                    {
                        AuthAccountId = authAccount.Id,
                        Provider = SsoProvider.AzureAD,
                        ProviderUserId = createDTO.AzureObjectId
                    };
                    identity.TryValidate();
                    await _ssoIdentityRepository.AddAsync(identity, token);

                    var user = new User
                    {
                        AuthAccountId = authAccount.Id,
                        Role = createDTO.Role,
                        CitizenId = null
                    };
                    user.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(_userRepository, user, cancellationToken: token);
                    await _userRepository.AddAsync(user, token);
                    await _unitOfWork.SaveChangeAsync(token);

                    var profile = new AdminProfile
                    {
                        UserId = user.Id,
                        StaffCode = createDTO.StaffCode,
                        FullName = createDTO.FullName,
                        Email = createDTO.Email,
                        PhoneNumber = createDTO.PhoneNumber,
                        SchoolId = createDTO.SchoolId
                    };
                    profile.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(_adminProfileRepository, profile, cancellationToken: token);
                    await _adminProfileRepository.AddAsync(profile, token);

                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.AccountCreation,
                        "Admin account created",
                        System.Text.Json.JsonSerializer.Serialize(new { user.Id, user.Role, profile.StaffCode }),
                        cancellationToken: token);

                    return user.Id;
                },
                cancellationToken);

            return await GetByIdAsync(userId, cancellationToken);
        }

        public async Task<GetAdminManagementDTO> UpdateAsync(
            int userId,
            UpdateAdminManagementDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            if (updateDTO.Role != UserRole.SchoolAdmin)
            {
                updateDTO.SchoolId = null;
            }
            await ValidateRequestAsync(updateDTO.Role, updateDTO.SchoolId, updateDTO.AzureObjectId, cancellationToken, userId);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var user = await _userRepository.Query(tracking: true)
                        .Include(entity => entity.AuthAccount)
                            .ThenInclude(account => account.SsoIdentities)
                        .Include(entity => entity.AdminProfile)
                        .FirstOrDefaultAsync(entity => entity.Id == userId, token)
                        ?? throw new DataNotFoundException("Admin", userId);

                    if (!IsAdminRole(user.Role) || user.AdminProfile == null)
                    {
                        throw new DataNotFoundException("Admin", userId);
                    }

                    var identity = user.AuthAccount.SsoIdentities
                        .SingleOrDefault(item => item.Provider == SsoProvider.AzureAD)
                        ?? throw new DataNotFoundException("Azure AD identity was not found for this admin.");
                    user.Role = updateDTO.Role;
                    user.AuthAccount.Status = updateDTO.Status;
                    identity.ProviderUserId = updateDTO.AzureObjectId;
                    user.AdminProfile.StaffCode = updateDTO.StaffCode;
                    user.AdminProfile.FullName = updateDTO.FullName;
                    user.AdminProfile.Email = updateDTO.Email;
                    user.AdminProfile.PhoneNumber = updateDTO.PhoneNumber;
                    user.AdminProfile.SchoolId = updateDTO.Role == UserRole.SchoolAdmin
                        ? updateDTO.SchoolId
                        : null;

                    user.TryValidate();
                    user.AuthAccount.TryValidate();
                    identity.TryValidate();
                    user.AdminProfile.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(_userRepository, user, user.Id, token);
                    await UniqueConstraintValidator.ValidateAsync(
                        _adminProfileRepository,
                        user.AdminProfile,
                        user.AdminProfile.Id,
                        token);

                    await _auditLogWriter.LogAsync(
                        updateDTO.Status == AuthAccountStatus.Active
                            ? AuditLogCategory.Security
                            : AuditLogCategory.StatusChange,
                        "Admin account updated",
                        System.Text.Json.JsonSerializer.Serialize(new { user.Id, user.Role, user.AuthAccount.Status }),
                        cancellationToken: token);
                },
                cancellationToken);

            return await GetByIdAsync(userId, cancellationToken);
        }

        public async Task<PaginationResult<GetAdminManagementDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);
            var (total, items) = await _userRepository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                IsAdmin,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                cancellationToken: cancellationToken);

            return new PaginationResult<GetAdminManagementDTO>(total, pageSize, items);
        }

        public async Task<List<GetAdminManagementDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                IsAdmin,
                cancellationToken: cancellationToken);
        }

        public async Task<List<GetAdminManagementDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                user => ids.Contains(user.Id) && user.AdminProfile != null &&
                    (user.Role == UserRole.SystemAdmin || user.Role == UserRole.FinanceAdmin || user.Role == UserRole.SchoolAdmin),
                cancellationToken: cancellationToken);
        }

        public async Task<GetAdminManagementDTO> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.FirstOrDefaultProjectedAsync(
                    _mapper.ProjectToGetDTO,
                    user => user.Id == id && user.AdminProfile != null &&
                        (user.Role == UserRole.SystemAdmin || user.Role == UserRole.FinanceAdmin || user.Role == UserRole.SchoolAdmin),
                    cancellationToken: cancellationToken)
                ?? throw new DataNotFoundException("Admin", id);
        }

        private async Task ValidateRequestAsync(
            UserRole role,
            int? schoolId,
            string azureObjectId,
            CancellationToken cancellationToken,
            int? excludedUserId = null)
        {
            if (!IsAdminRole(role))
            {
                throw new ValidationFailureException(nameof(role), "Role must be an admin role.");
            }

            if (role == UserRole.SchoolAdmin && !schoolId.HasValue)
            {
                throw new ValidationFailureException(nameof(schoolId), "SchoolId is required for SchoolAdmin.");
            }

            if (role != UserRole.SchoolAdmin && schoolId.HasValue)
            {
                throw new ValidationFailureException(nameof(schoolId), "SchoolId is only allowed for SchoolAdmin.");
            }

            if (schoolId.HasValue && !await _schoolRepository.AnyAsync(school => school.Id == schoolId.Value, cancellationToken))
            {
                throw new DataNotFoundException(typeof(School), schoolId.Value);
            }

            var identityExists = await _ssoIdentityRepository.Query()
                .AnyAsync(
                    identity => identity.Provider == SsoProvider.AzureAD
                        && identity.ProviderUserId == azureObjectId
                        && (!excludedUserId.HasValue
                            || identity.AuthAccountId != _userRepository.Query()
                                .Where(user => user.Id == excludedUserId.Value)
                                .Select(user => user.AuthAccountId)
                                .FirstOrDefault()),
                    cancellationToken);
            if (identityExists)
            {
                throw new DataConflictException("Azure ObjectId already exists.");
            }
        }

        private static readonly Expression<Func<User, bool>> IsAdmin =
            user => user.AdminProfile != null &&
                (user.Role == UserRole.SystemAdmin ||
                 user.Role == UserRole.FinanceAdmin ||
                 user.Role == UserRole.SchoolAdmin);

        private static bool IsAdminRole(UserRole role)
        {
            return role is UserRole.SystemAdmin or UserRole.FinanceAdmin or UserRole.SchoolAdmin;
        }
    }
}
