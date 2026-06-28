using DTOs.FasApplications;
using Filters.FasApplications;
using Interfaces.FasApplications;
using Mappers.FasApplications;
using Results;
using System.Linq.Expressions;
using Models;
using Exceptions;
using Interfaces.Auth;
using Microsoft.EntityFrameworkCore;
using Services.Auth;
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
        private readonly IGenericRepository<FasApplication> _fasApplicationRepository = unitOfWork.Repository<FasApplication>();

        public override async Task<PaginationResult<GetFasApplicationSchoolAdminDTO>> GetAllPaginatedAsync(Filters.Base.FilterDTO filterDTO, CancellationToken cancellationToken = default)
        {
            var request = (FasApplicationFilterDTO)filterDTO;
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            if (request.Status is { } status)
            {
                return await base.GetAllPaginatedAsync(
                    request,
                    a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == status,
                    cancellationToken);
            }

            return await base.GetAllPaginatedAsync(
                request,
                a => a.SchoolStudent.SchoolId == adminSchoolId,
                cancellationToken);
        }

        public async Task<GetFasApplicationSchoolAdminDetailDTO> GetApplicationDetailsAsync(int applicationId, CancellationToken cancellationToken = default)
        {
            var adminSchoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var application = await _fasApplicationRepository.Query()
                .Include(a => a.SchoolStudent)
                .Include(a => a.SchoolStudent.EducationAccount)
                .Include(a => a.SchoolStudent.EducationAccount.Citizen)
                .Include(a => a.RecommendedTier)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.Tiers)
                .Include(a => a.FasScheme)
                    .ThenInclude(s => s.RequiredDocuments)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.Id == applicationId && a.SchoolStudent.SchoolId == adminSchoolId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException($"Application {applicationId} not found.");
            }

            return _mapper.MapToDetailDTO(application);
        }

        public async Task RejectAsync(int id, RejectFasApplicationDTO dto, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var application = await _unitOfWork.Repository<FasApplication>()
            .Query()
            .Include(a => a.FasScheme)
            .FirstOrDefaultAsync(a => a.Id == id && a.FasScheme.SchoolId == schoolId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (application.Status != Enums.FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be rejected.");
            }

            application.Status = Enums.FasApplicationStatus.Rejected;
            application.RejectionReason = dto.RejectionReason;

            application.TryValidate();

            _unitOfWork.Repository<FasApplication>().Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        public async Task ApproveAsync(int id, CancellationToken cancellationToken = default)
        {
            var schoolId = await _schoolScopeResolver.GetSchoolIdAsync(cancellationToken);

            var application = await _unitOfWork.Repository<FasApplication>()
           .Query()
           .Include(a => a.FasScheme)
           .Include(a => a.SchoolStudent)
           .FirstOrDefaultAsync(a => a.Id == id && a.FasScheme.SchoolId == schoolId, cancellationToken);

            if (application == null)
            {
                throw new DataNotFoundException(typeof(FasApplication), id);
            }

            if (application.Status != Enums.FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be approved.");
            }

            if (!application.RecommendedTierId.HasValue)
            {
                throw new DataConflictException("Cannot approve application because no tier is eligible.");
            }

            var currentUserId = _currentUserService.UserId;
            var now = DateTime.UtcNow;

            application.Status = Enums.FasApplicationStatus.Approved;
            application.ApprovedAt = now;
            application.ApprovedByUserId = currentUserId;
            application.ApprovedTierId = application.RecommendedTierId;
            application.DurationInMonthsSnapshot = application.FasScheme.DurationInMonths;
            application.ValidityStartDate = now.Date;
            application.ValidityEndDate = application.FasScheme.DurationInMonths > 0
                ? application.ValidityStartDate.Value.AddMonths(application.FasScheme.DurationInMonths)
                : null;

            application.TryValidate();

            _unitOfWork.Repository<FasApplication>().Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        // ==========================================
        // UNUSED API IMPLEMENTATIONS FROM BASE CLASS
        // ==========================================

        public override Task<List<GetFasApplicationSchoolAdminDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<List<GetFasApplicationSchoolAdminDTO>> GetAllByIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<GetFasApplicationSchoolAdminDTO> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
