using DTOs.FasApplications;
using Riok.Mapperly.Abstractions;

using Mappers.Base;

namespace Mappers.FasApplications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class FasApplicationMapper : IReadMapper<FasApplication, GetFasApplicationSchoolAdminDTO>
    {
        [MapProperty(nameof(FasApplication.Id), nameof(GetFasApplicationSchoolAdminDetailDTO.id))]
        [MapProperty(nameof(FasApplication.StudentAgeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.Age)}")]
        [MapProperty(nameof(FasApplication.StudentNationalitySnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.StudentNationality)}")]
        [MapProperty(nameof(FasApplication.GuardianNationalitySnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.GuardianNationality)}")]
        [MapProperty(nameof(FasApplication.GrossHouseholdIncomeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.GrossHouseholdIncome)}")]
        [MapProperty(nameof(FasApplication.HouseholdMemberCountSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.HouseholdMembers)}")]
        [MapProperty(nameof(FasApplication.PerCapitaIncomeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.PerCapitaIncome)}")]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.Scheme))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.SystemSuggestedTier))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.Status))]
        private partial GetFasApplicationSchoolAdminDetailDTO MapToDetailDTOInternal(FasApplication model);

        public GetFasApplicationSchoolAdminDetailDTO MapToDetailDTO(FasApplication model)
        {
            var target = MapToDetailDTOInternal(model);
            
            target.Status = model.Status.ToString();
            target.Scheme = new SchemeDetailsDTO
            {
                Id = model.FasSchemeId,
                SchemeName = model.FasScheme.SchemeName,
                Tiers = model.FasScheme.Tiers.Select(MapTierToDTO).ToList(),
                RequiredDocuments = model.FasScheme.RequiredDocuments.Select(rd =>
                {
                    var attachedDoc = model.Documents.FirstOrDefault(d => d.FasSchemeRequiredDocumentId == rd.Id);
                    return new ApplicationDocumentDTO
                    {
                        Id = rd.Id,
                        DocumentName = rd.DocumentName,
                        FileKey = attachedDoc?.FileKey
                    };
                }).ToList()
            };

            if (model.RecommendedTier != null)
            {
                target.SystemSuggestedTier = new SystemSuggestedTierDTO
                {
                    Id = model.RecommendedTier.Id,
                    TierName = model.RecommendedTier.TierName,
                    Reason = model.RecommendationReason ?? $"Matches {model.RecommendedTier.TierName} bracket."
                };
            }

            return target;
        }

        [MapProperty(nameof(FasSchemeTier.Id), nameof(TierDetailsDTO.Id))]
        [MapProperty(nameof(FasSchemeTier.TierName), nameof(TierDetailsDTO.TierName))]
        [MapProperty(nameof(FasSchemeTier.MaxPerCapitaIncome), nameof(TierDetailsDTO.MaxPerCapitaIncome))]
        [MapProperty(nameof(FasSchemeTier.SubsidyValue), nameof(TierDetailsDTO.SubsidyValue))]
        [MapProperty(nameof(FasSchemeTier.CourseFeeSubsidyValue), nameof(TierDetailsDTO.CourseFeeSubsidyValue))]
        [MapProperty(nameof(FasSchemeTier.MiscFeeSubsidyValue), nameof(TierDetailsDTO.MiscFeeSubsidyValue))]
        [MapperIgnoreSource(nameof(FasSchemeTier.FasScheme))]
        public partial TierDetailsDTO MapTierToDTO(FasSchemeTier tier);

        public partial GetFasApplicationSchoolAdminDTO MapToGetDTO(FasApplication model);
        public partial List<GetFasApplicationSchoolAdminDTO> MapToGetDTOList(List<FasApplication> models);
        public IQueryable<GetFasApplicationSchoolAdminDTO> ProjectToGetDTO(IQueryable<FasApplication> query)
        {
            return query.Select(a => new GetFasApplicationSchoolAdminDTO
            {
                Id = a.Id,
                ApplicationNumber = a.ApplicationNumber,
                AccountName = a.SchoolStudent.EducationAccount.Citizen.FullName,
                AccountNumber = a.SchoolStudent.EducationAccount.AccountNumber,
                SchemeName = a.FasScheme.SchemeName,
                SubmittedAt = a.CreatedAt,
                Status = a.Status.ToString()
            });
        }
    }
}
