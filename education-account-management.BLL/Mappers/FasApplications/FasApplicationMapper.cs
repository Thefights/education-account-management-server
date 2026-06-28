using DTOs.FasApplications;
using Riok.Mapperly.Abstractions;

namespace Mappers.FasApplications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class FasApplicationMapper
    {
        [MapProperty(nameof(FasApplication.Id), nameof(GetFasApplicationSchoolAdminDetailDTO.id))]
        [MapProperty(nameof(FasApplication.StudentAgeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.Age)}")]
        [MapProperty(nameof(FasApplication.StudentNationalitySnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.StudentNationality)}")]
        [MapProperty(nameof(FasApplication.GuardianNationalitySnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.GuardianNationality)}")]
        [MapProperty(nameof(FasApplication.GrossHouseholdIncomeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.GrossHouseholdIncome)}")]
        [MapProperty(nameof(FasApplication.HouseholdMemberCountSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.HouseholdMembers)}")]
        [MapProperty(nameof(FasApplication.PerCapitaIncomeSnapshot), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.StudentProfile)}.{nameof(StudentProfileDTO.PerCapitaIncome)}")]
        [MapProperty(nameof(FasApplication.FasSchemeId), $"{nameof(GetFasApplicationSchoolAdminDetailDTO.Scheme)}.{nameof(SchemeDetailsDTO.Id)}")]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.Status))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.Scheme))]
        [MapperIgnoreTarget(nameof(GetFasApplicationSchoolAdminDetailDTO.SystemSuggestedTier))]
        public partial GetFasApplicationSchoolAdminDetailDTO MapToDetailDTO(FasApplication model);

        [MapProperty(nameof(FasSchemeTier.Id), nameof(TierDetailsDTO.Id))]
        [MapProperty(nameof(FasSchemeTier.TierName), nameof(TierDetailsDTO.TierName))]
        [MapProperty(nameof(FasSchemeTier.MaxPerCapitaIncome), nameof(TierDetailsDTO.MaxPerCapitaIncome))]
        [MapperIgnoreSource(nameof(FasSchemeTier.FasScheme))]
        [MapperIgnoreTarget(nameof(TierDetailsDTO.ConditionText))]
        [MapperIgnoreTarget(nameof(TierDetailsDTO.SubsidyDescription))]
        public partial TierDetailsDTO MapTierToDTO(FasSchemeTier tier);
    }
}
