using DTOs.Admin;
using DTOs.Csv;
using Interfaces.Admin;
using Interfaces.Audit;
using Mappers.Admin;
using Results;
using System.Linq.Expressions;
using Validators;


namespace Services.Admin
{
    public class AdminService(
        IUnitOfWork unitOfWork,
        AdminMapper mapper,
        IAuditLogWriter auditLogWriter,
        IManagementActionLogService managementActionLogService,
        ICurrentUserService currentUserService)
        : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AdminMapper _mapper = mapper;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly IManagementActionLogService _managementActionLogService = managementActionLogService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();
        private readonly IGenericRepository<SsoIdentity> _ssoIdentityRepository = unitOfWork.Repository<SsoIdentity>();
        private readonly IGenericRepository<AdminProfile> _adminProfileRepository = unitOfWork.Repository<AdminProfile>();
        private readonly IGenericRepository<School> _schoolRepository = unitOfWork.Repository<School>();
        private readonly IGenericRepository<UserStatusHistory> _userStatusHistoryRepository = unitOfWork.Repository<UserStatusHistory>();

        public async Task<GetAdminDTO> CreateAsync(
            CreateAdminDTO createDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);
            await ValidateRequestAsync(
                createDTO.Role,
                createDTO.SchoolId,
                createDTO.AzureObjectId,
                createDTO.Nric,
                cancellationToken);
            var staffCode = await BusinessCodeGenerator.GenerateUniqueAsync(
                BusinessCodeGenerator.StaffPrefix,
                (candidate, token) => _adminProfileRepository.AnyAsync(
                    profile => profile.StaffCode == candidate,
                    token),
                conflictMessage: "Unable to generate a unique staff code. Please retry.",
                cancellationToken: cancellationToken);

            var userId = await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var user = new User
                    {
                        Role = createDTO.Role,
                        CitizenId = null
                    };
                    user.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(_userRepository, user, cancellationToken: token);
                    await _userRepository.AddAsync(user, token);
                    await _unitOfWork.SaveChangeAsync(token);

                    var identity = new SsoIdentity
                    {
                        UserId = user.Id,
                        Provider = SsoProvider.AzureAD,
                        ProviderUserId = createDTO.AzureObjectId
                    };
                    identity.TryValidate();
                    await _ssoIdentityRepository.AddAsync(identity, token);

                    var profile = new AdminProfile
                    {
                        UserId = user.Id,
                        StaffCode = staffCode,
                        FullName = createDTO.FullName,
                        Nric = createDTO.Nric,
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
                        profile.Nric,
                        cancellationToken: token);

                    return user.Id;
                },
                cancellationToken);

            return await GetByIdAsync(userId, cancellationToken);
        }

        public async Task<GetAdminDTO> UpdateAsync(
            int userId,
            UpdateAdminDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);
            if (updateDTO.Role != UserRole.SchoolAdmin)
            {
                updateDTO.SchoolId = null;
            }
            await ValidateRequestAsync(
                updateDTO.Role,
                updateDTO.SchoolId,
                updateDTO.AzureObjectId,
                updateDTO.Nric,
                cancellationToken,
                userId);

            await _unitOfWork.ExecuteInTransactionAsync(
                async (_, token) =>
                {
                    var user = await _userRepository.Query(tracking: true)
                        .Include(entity => entity.SsoIdentities)
                        .Include(entity => entity.AdminProfile)
                        .FirstOrDefaultAsync(entity => entity.Id == userId, token)
                        ?? throw new DataNotFoundException("Admin", userId);

                    if (!IsAdminRole(user.Role) || user.AdminProfile == null)
                    {
                        throw new DataNotFoundException("Admin", userId);
                    }

                    var identity = user.SsoIdentities
                        .SingleOrDefault(item => item.Provider == SsoProvider.AzureAD)
                        ?? throw new DataNotFoundException("Azure AD identity was not found for this admin.");
                    user.Role = updateDTO.Role;
                    identity.ProviderUserId = updateDTO.AzureObjectId;
                    user.AdminProfile.FullName = updateDTO.FullName;
                    user.AdminProfile.Nric = updateDTO.Nric;
                    user.AdminProfile.Email = updateDTO.Email;
                    user.AdminProfile.PhoneNumber = updateDTO.PhoneNumber;
                    user.AdminProfile.SchoolId = updateDTO.Role == UserRole.SchoolAdmin
                        ? updateDTO.SchoolId
                        : null;

                    user.TryValidate();
                    identity.TryValidate();
                    user.AdminProfile.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(_userRepository, user, user.Id, token);
                    await UniqueConstraintValidator.ValidateAsync(
                        _adminProfileRepository,
                        user.AdminProfile,
                        user.AdminProfile.Id,
                        token);

                    await _auditLogWriter.LogAsync(
                        AuditLogCategory.Security,
                        "Admin account updated",
                        user.AdminProfile.Nric,
                        cancellationToken: token);
                },
                cancellationToken);

            return await GetByIdAsync(userId, cancellationToken);
        }

        public async Task<PaginationResult<GetAdminDTO>> GetAllPaginatedAsync(
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

            return new PaginationResult<GetAdminDTO>(total, pageSize, items);
        }

        public async Task<List<GetAdminDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                IsAdmin,
                cancellationToken: cancellationToken);
        }

        public async Task<List<GetAdminDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                user => ids.Contains(user.Id) && user.AdminProfile != null &&
                    (user.Role == UserRole.SystemAdmin || user.Role == UserRole.FinanceAdmin || user.Role == UserRole.SchoolAdmin),
                cancellationToken: cancellationToken);
        }

        public async Task<GetAdminDTO> GetByIdAsync(
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
            string nric,
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
                        && (!excludedUserId.HasValue || identity.UserId != excludedUserId.Value),
                    cancellationToken);
            if (identityExists)
            {
                throw new DataConflictException("Azure ObjectId already exists.");
            }

            var normalizedNric = nric.Trim().ToUpperInvariant();
            var nricExists = await _adminProfileRepository.Query()
                .AnyAsync(
                    profile => profile.Nric.ToUpper() == normalizedNric
                        && (!excludedUserId.HasValue || profile.UserId != excludedUserId.Value),
                    cancellationToken);
            if (nricExists)
            {
                throw new DataConflictException("NRIC already exists.");
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

        public async Task UpdateAdminsStatusAsync(BatchUpdateAdminStatusDTO dto, CancellationToken cancellationToken = default)
        {
            var batchId = Guid.NewGuid();
            var users = await _userRepository.GetByIdsAsync(dto.Ids, cancellationToken: cancellationToken);
            if (users.Count != dto.Ids.Distinct().Count())
                throw new ValidationFailureException(nameof(dto.Ids), "One or more admins do not exist.");

            await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
            {
                foreach (var user in users)
                {
                    var oldStatus = user.Status;
                    var newStatus = (UserStatus)dto.Status;
                    user.Status = newStatus;
                    if (oldStatus != newStatus)
                    {
                        var history = new UserStatusHistory
                        {
                            UserId = user.Id,
                            PreviousStatus = oldStatus,
                            NewStatus = newStatus,
                            Reason = dto.Reason,
                            ChangedAt = DateTime.UtcNow,
                            ChangedByUserId = _currentUserService.CurrentUserId
                        };
                        history.TryValidate();
                        await _userStatusHistoryRepository.AddAsync(history, token);
                    }
                    await _managementActionLogService.LogAsync(
                        batchId,
                        "Admin",
                        user.Id,
                        newStatus == Enums.UserStatus.Active ? "Activate" : "Deactivate",
                        dto.Reason,
                        oldStatus.ToString(),
                        newStatus.ToString(),
                        cancellationToken: token);
                }
                _userRepository.UpdateRange(users);
            }, cancellationToken);
        }

        public async Task<BatchImportResultDTO> ImportAsync(Microsoft.AspNetCore.Http.IFormFile file, CancellationToken cancellationToken = default)
        {
            var fileErrors = CsvImportHelper.ValidateFile(file);
            if (fileErrors.Count != 0)
                CsvImportHelper.ThrowIfImportFailed(0, fileErrors);

            var rows = CsvImportHelper.ReadRows<CreateAdminDTO>(file);
            if (rows.Errors.Count != 0)
                CsvImportHelper.ThrowIfImportFailed(rows.Total, rows.Errors);

            if (rows.Items.Count == 0)
                CsvImportHelper.ThrowIfImportFailed(0, [DTOs.Csv.BatchImportErrorDTO.Create(0, "File", "CSV file must contain at least one data row.")]);

            var errors = new List<DTOs.Csv.BatchImportErrorDTO>();
            var successCount = 0;

            foreach (var item in rows.Items)
            {
                try
                {
                    await CreateAsync(item.Row, cancellationToken);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add(BatchImportErrorDTO.Create(item.RowNumber, "Row", ex.Message, item.Row.Nric));
                }
            }

            if (errors.Count != 0)
            {
                CsvImportHelper.ThrowIfImportFailed(rows.Total, errors, successCount);
            }

            return new BatchImportResultDTO
            {
                Total = rows.Total,
                Succeeded = successCount,
                Failed = 0,
                Errors = []
            };
        }
    }
}
