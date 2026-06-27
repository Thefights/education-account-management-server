using DTOs.FasApplications;
using Riok.Mapperly.Abstractions;

namespace Mappers.FasApplications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class FasApplicationMapper
    {
        [MapProperty(nameof(FasApplication.Id), nameof(FasApplicationSchoolAdminDetailDTO.id))]
        [MapProperty(nameof(FasApplication.StudentAgeSnapshot), $"{nameof(FasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.Age)}")]
        [MapProperty(nameof(FasApplication.StudentNationalitySnapshot), $"{nameof(FasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.StudentNationality)}")]
        [MapProperty(nameof(FasApplication.GuardianNationalitySnapshot), $"{nameof(FasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.GuardianNationality)}")]
        [MapProperty(nameof(FasApplication.GrossHouseholdIncomeSnapshot), $"{nameof(FasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.GrossHouseholdIncome)}")]
        [MapProperty(nameof(FasApplication.HouseholdMemberCountSnapshot), $"{nameof(FasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.HouseholdMembers)}")]
        [MapProperty(nameof(FasApplication.PerCapitaIncomeSnapshot), $"{nameof(FasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.PerCapitaIncome)}")]
        [MapProperty(nameof(FasApplication.FasSchemeId), $"{nameof(FasApplicationSchoolAdminDetailDTO.Scheme)}.{nameof(SchemeDetailsDTO.Id)}")]
        [MapperIgnoreTarget(nameof(FasApplicationSchoolAdminDetailDTO.Status))]
        [MapperIgnoreTarget(nameof(FasApplicationSchoolAdminDetailDTO.Scheme))]
        [MapperIgnoreTarget(nameof(FasApplicationSchoolAdminDetailDTO.SystemSuggestedTier))]
        public partial FasApplicationSchoolAdminDetailDTO MapToDetailDTO(FasApplication model);

        [MapProperty(nameof(FasSchemeTier.Id), nameof(TierDetailsDTO.Id))]
        [MapProperty(nameof(FasSchemeTier.TierName), nameof(TierDetailsDTO.TierName))]
        [MapProperty(nameof(FasSchemeTier.MaxPerCapitaIncome), nameof(TierDetailsDTO.MaxPerCapitaIncome))]
        [MapperIgnoreSource(nameof(FasSchemeTier.FasScheme))]
        [MapperIgnoreTarget(nameof(TierDetailsDTO.ConditionText))]
        [MapperIgnoreTarget(nameof(TierDetailsDTO.SubsidyDescription))]
        public partial TierDetailsDTO MapTierToDTO(FasSchemeTier tier);
    }
}
