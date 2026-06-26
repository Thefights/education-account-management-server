using DTOs.FasApplications;
using Enums;
using Exceptions;
using Interfaces.Base;
using Interfaces.FasApplications;
using Microsoft.EntityFrameworkCore;
using Models;
using Utils;

namespace Services.FasApplications
{
    public class ManagementFasApplicationService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        SchoolScopeResolver schoolScopeResolver) : IManagementFasApplicationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly SchoolScopeResolver _schoolScopeResolver = schoolScopeResolver;

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

            _unitOfWork.Repository<FasApplication>().Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
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

            if (application.Status != FasApplicationStatus.Pending)
            {
                throw new DataConflictException("Only pending applications can be rejected.");
            }

            if (string.IsNullOrWhiteSpace(dto.RejectionReason))
            {
                throw new DataConflictException("Rejection reason is required.");
            }

            application.Status = FasApplicationStatus.Rejected;
            application.RejectionReason = dto.RejectionReason;

            application.TryValidate();

            _unitOfWork.Repository<FasApplication>().Update(application);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
