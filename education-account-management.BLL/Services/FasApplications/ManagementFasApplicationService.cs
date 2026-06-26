using DTOs.FasApplications;
using Enums;
using Interfaces.FasApplications.Management;
using Mappers.FasApplications;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Exceptions;

namespace Services.FasApplications
{
    public class ManagementFasApplicationService(IUnitOfWork unitOfWork, FasApplicationMapper mapper) : IManagementFasApplicationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly FasApplicationMapper _mapper = mapper;
        private readonly IGenericRepository<FasApplication> _fasApplicationRepository = unitOfWork.Repository<FasApplication>();

        public async Task<FasApplicationQueueResponseDTO> GetApplicationQueueAsync(GetFasApplicationListRequestDTO request, int adminSchoolId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow.AddHours(8);
            
            // 1. Build Base Filter for Admin's School
            Expression<Func<FasApplication, bool>> baseFilter = a => a.SchoolStudent.SchoolId == adminSchoolId;

            // 2. Count for each tab
            var pendingCount = await _fasApplicationRepository.CountAsync(
                a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Pending, 
                cancellationToken);
                
            var approvedCount = await _fasApplicationRepository.CountAsync(
                a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Approved && (a.ValidityEndDate == null || a.ValidityEndDate >= now), 
                cancellationToken);
                
            var expiredCount = await _fasApplicationRepository.CountAsync(
                a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Approved && a.ValidityEndDate < now, 
                cancellationToken);
                
            var rejectedCount = await _fasApplicationRepository.CountAsync(
                a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Rejected, 
                cancellationToken);

            // 3. Status Filter based on Request
            var requestedStatus = request.Status?.ToLower();
            Expression<Func<FasApplication, bool>> dbFilter = requestedStatus switch
            {
                "approved" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Approved && (a.ValidityEndDate == null || a.ValidityEndDate >= now),
                "expired" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Approved && a.ValidityEndDate < now,
                "rejected" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Rejected,
                "pending" => a => a.SchoolStudent.SchoolId == adminSchoolId && a.Status == FasApplicationStatus.Pending,
                _ => a => a.SchoolStudent.SchoolId == adminSchoolId
            };

            // 4. Search Filter
            string[] searchFields = [nameof(FasApplication.ApplicationNumber), "SchoolStudent.EducationAccount.AccountNumber", "SchoolStudent.EducationAccount.Citizen.FullName", "FasScheme.SchemeName"];
            
            // 5. Sort Order
            string order = string.IsNullOrWhiteSpace(request.Sort) ? "CreatedAt asc" : request.Sort;

            // 6. Pagination & Projection
            var result = await _fasApplicationRepository.GetProjectedPaginatedAsync(
                _mapper.ProjectToListItemDTO,
                dbFilter,
                null,
                request.Search,
                searchFields,
                order,
                request.Page,
                request.PageSize,
                null,
                cancellationToken
            );

            // Override mapped status to properly label 'expired' or just lowercase
            foreach (var item in result.Items)
            {
                if (string.Equals(item.Status, "Approved", StringComparison.OrdinalIgnoreCase) && item.ValidityEndDate < now)
                {
                    item.Status = "expired";
                }
                else
                {
                    item.Status = item.Status.ToLower();
                }
            }

            return new FasApplicationQueueResponseDTO
            {
                Collection = result.Items,
                Counts = new FasApplicationCountsDTO
                {
                    Pending = pendingCount,
                    Approved = approvedCount,
                    Expired = expiredCount,
                    Rejected = rejectedCount
                },
                TotalCount = result.Count,
                TotalPages = (int)Math.Ceiling(result.Count / (double)request.PageSize)
            };
        }

        public async Task<FasApplicationDetailsDTO> GetApplicationDetailsAsync(int applicationId, int adminSchoolId, CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.Repository<FasApplication>();
            
            var application = await repo.Query()
                .Include(a => a.SchoolStudent)
                .Include(a => a.SchoolStudent.EducationAccount)
                .Include(a => a.SchoolStudent.EducationAccount.Citizen)
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

            // Determine status string (including 'expired')
            var now = DateTime.UtcNow;
            var statusStr = application.Status.ToString().ToLower();
            if (application.Status == FasApplicationStatus.Approved && application.ValidityEndDate < now)
            {
                statusStr = "expired";
            }

            var isPercent = application.FasScheme.SubsidyType == FasSubsidyType.Percent;
            var symbol = isPercent ? "%" : " S$";

            // Map DTO
            var dto = new FasApplicationDetailsDTO
            {
                ApplicationId = application.ApplicationNumber,
                Status = statusStr,
                StudentProfile = new StudentProfileDTO
                {
                    Age = application.StudentAgeSnapshot,
                    StudentNationality = application.StudentNationalitySnapshot,
                    GuardianNationality = application.GuardianNationalitySnapshot,
                    GrossHouseholdIncome = application.GrossHouseholdIncomeSnapshot,
                    HouseholdMembers = application.HouseholdMemberCountSnapshot,
                    PerCapitaIncome = application.PerCapitaIncomeSnapshot
                },
                Scheme = new SchemeDetailsDTO
                {
                    Id = application.FasSchemeId,
                    SchemeName = application.FasScheme.SchemeName,
                    Tiers = application.FasScheme.Tiers.Select(t => new TierDetailsDTO
                    {
                        Id = t.Id,
                        TierName = t.TierName,
                        ConditionText = t.MaxPerCapitaIncome.HasValue ? $"PCI <= {t.MaxPerCapitaIncome.Value:0.00}" : "No limit",
                        SubsidyDescription = application.FasScheme.IsPerComponent 
                            ? $"Course: {t.CourseFeeSubsidyValue:0.00}{symbol}, Misc: {t.MiscFeeSubsidyValue:0.00}{symbol}"
                            : $"{t.SubsidyValue:0.00}{symbol}",
                        MaxPerCapitaIncome = t.MaxPerCapitaIncome
                    }).ToList(),
                    RequiredDocuments = application.FasScheme.RequiredDocuments.Select(rd => 
                    {
                        var attachedDoc = application.Documents.FirstOrDefault(d => d.FasSchemeRequiredDocumentId == rd.Id);
                        return new ApplicationDocumentDTO
                        {
                            Id = rd.Id,
                            DocumentName = rd.DocumentName,
                            Status = attachedDoc != null ? "attached" : "missing",
                            FileUrl = attachedDoc?.FileKey // returning FileKey as FileUrl
                        };
                    }).ToList()
                }
            };

            // Calculate Recommended Tier
            //citizen-only -> Tier 1 if Citizen": For a citizenship grant scheme, the MaxPerCapitaIncome field on the tier in the database is set to null
            var eligibleTier = application.FasScheme.Tiers
                .Where(t => t.MaxPerCapitaIncome == null || application.PerCapitaIncomeSnapshot <= t.MaxPerCapitaIncome)
                .OrderBy(t => t.MaxPerCapitaIncome ?? decimal.MaxValue)
                .FirstOrDefault();

            if (eligibleTier != null)
            {
                dto.SystemSuggestedTier = new SystemSuggestedTierDTO
                {
                    Id = eligibleTier.Id,
                    TierName = eligibleTier.TierName,
                    Reason = $"Calculated PCI (${application.PerCapitaIncomeSnapshot:0.00}) matches {eligibleTier.TierName} bracket."
                };
            }

            return dto;
        }
    }
}
