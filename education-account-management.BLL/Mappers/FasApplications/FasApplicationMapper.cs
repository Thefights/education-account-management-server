using DTOs.FasApplications;
using Models;
using Riok.Mapperly.Abstractions;

namespace Mappers.FasApplications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class FasApplicationMapper
    {
        [MapProperty(nameof(FasApplication.Id), nameof(FasApplicationItemDTO.Id))]
        [MapProperty($"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}", nameof(FasApplicationItemDTO.ApplicantNo))]
        [MapProperty($"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.AccountNumber)}", nameof(FasApplicationItemDTO.AccountName))]
        [MapProperty($"{nameof(FasApplication.FasScheme)}.{nameof(FasScheme.SchemeName)}", nameof(FasApplicationItemDTO.SchemeName))]
        [MapProperty(nameof(FasApplication.CreatedAt), nameof(FasApplicationItemDTO.SubmittedAt))]
        [MapProperty(nameof(FasApplication.Status), nameof(FasApplicationItemDTO.Status))]
        [MapProperty(nameof(FasApplication.ValidityEndDate), nameof(FasApplicationItemDTO.ValidityEndDate))]
        public partial FasApplicationItemDTO MapToListItemDTO(FasApplication model);

        public partial IQueryable<FasApplicationItemDTO> ProjectToListItemDTO(IQueryable<FasApplication> query);
    }
}
