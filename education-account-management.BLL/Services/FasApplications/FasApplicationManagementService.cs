using DTOs.FasApplications;
using Interfaces.FasApplications;
using Mappers.FasApplications;
using Results;
using Services.Base;

namespace Services.FasApplications
{
    public class FasApplicationManagementService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        SchoolScopeResolver schoolScopeResolver,
        FasApplicationMapper mapper) : BaseGetService<FasApplication, GetFasApplicationSchoolAdminDTO>(unitOfWork, mapper), IFasApplicationManagementService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;
        private readonly FasApplicationMapper _mapper = mapper;

        public override async Task<PaginationResult<GetFasApplicationSchoolAdminDTO>> GetAllPaginatedAsync(
            FilterDTO filterDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterDTO);
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);

            var (total, items) = await _repository.GetProjectedPaginatedAsync(
                _mapper.ProjectToGetDTO,
                a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status != FasApplicationStatus.Draft,
                filterDTO.Filter,
                filterDTO.Search,
                filterDTO.SearchFields,
                filterDTO.SortExpression,
                filterDTO.Page,
                pageSize,
                _includes,
                cancellationToken);

            return new PaginationResult<GetFasApplicationSchoolAdminDTO>(total, pageSize, items);
        }

        public async Task<GetFasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var application = await _repository.Query()
                .Include(a => a.SchoolStudent)
                .Include(a => a.SchoolStudent.EducationAccount)
                .Include(a => a.SchoolStudent.EducationAccount.Citizen)
                .Include(a => a.RecommendedTier)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.Tiers)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.RequiredDocuments)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(
                    a => a.Id == applicationId &&
                        a.SchoolStudent.SchoolId == adminSchoolId &&
                        a.Status != FasApplicationStatus.Draft,
                    cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), applicationId);
            }

            return _mapper.MapToDetailDTO(application);
        }

        public async Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(dto.RejectionReason))
            {
                throw new ValidationFailureException(nameof(dto.RejectionReason), "Rejection reason is required.");
            }

            dto.RejectionReason = dto.RejectionReason.Trim();

            var application = await _repository
                .Query()
                .Include(a => a.FasScheme)
                .Include(a => a.SchoolStudent)
                .FirstOrDefaultAsync(
                    a => a.Id == id &&
                        a.SchoolStudent.SchoolId == schoolId &&
                        a.FasScheme.SchoolId == schoolId,
                    cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (application.Status != FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be rejected.");
            }

            application.Status = FasApplicationStatus.Rejected;
            application.RejectionReason = dto.RejectionReason;

            application.TryValidate();

            _repository.Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task ApproveAsync(int id, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var application = await _repository
                .Query()
                .Include(a => a.FasScheme)
                .Include(a => a.SchoolStudent)
                .FirstOrDefaultAsync(
                    a => a.Id == id &&
                        a.SchoolStudent.SchoolId == schoolId &&
                        a.FasScheme.SchoolId == schoolId,
                    cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (application.Status != FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be approved.");
            }

            if (!application.RecommendedTierId.HasValue)
            {
                throw new DataConflictException("Cannot approve application because no tier is eligible.");
            }

            var currentUserId = _currentUserService.UserId;
            var now = DateTime.UtcNow;

            application.Status = FasApplicationStatus.Approved;
            application.ApprovedAt = now;
            application.ApprovedByUserId = currentUserId;
            application.ApprovedTierId = application.RecommendedTierId;
            application.DurationInMonthsSnapshot = application.FasScheme.DurationInMonths;
            application.ValidityStartDate = now.Date;
            application.ValidityEndDate = application.FasScheme.DurationInMonths > 0
                ? application.ValidityStartDate.Value.AddMonths(application.FasScheme.DurationInMonths)
                : null;

            application.TryValidate();

            _repository.Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public override async Task<List<GetFasApplicationSchoolAdminDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                a => a.SchoolStudent.SchoolId == schoolId && a.Status != FasApplicationStatus.Draft,
                _includes,
                cancellationToken);
        }

        public override async Task<List<GetFasApplicationSchoolAdminDTO>> GetAllByIdsAsync(
            List<int> ids,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.GetProjectedAsync(
                _mapper.ProjectToGetDTO,
                a => ids.Contains(a.Id) &&
                    a.SchoolStudent.SchoolId == schoolId &&
                    a.Status != FasApplicationStatus.Draft,
                _includes,
                cancellationToken);
        }

        public override async Task<GetFasApplicationSchoolAdminDTO> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);
            return await _repository.FirstOrDefaultProjectedAsync(
                _mapper.ProjectToGetDTO,
                a => a.Id == id &&
                    a.SchoolStudent.SchoolId == schoolId &&
                    a.Status != FasApplicationStatus.Draft,
                _includes,
                cancellationToken)
                ?? throw new DataNotFoundException(typeof(FasApplication), id);
        }
    }
}