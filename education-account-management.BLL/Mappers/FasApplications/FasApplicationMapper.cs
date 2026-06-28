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
        [MapProperty(nameof(FasApplication.FasSchemeId), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.Scheme)}.{nameof(SchemeDetailsDTO.Id)}")]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.Scheme))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.SystemSuggestedTier))]
        public partial GetFasApplicationSchoolAdminDetailDTO MapToDetailDTO(FasApplication model);

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
