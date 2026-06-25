using DTOs.FasApplications;
using Models;
using Riok.Mapperly.Abstractions;

namespace Mappers.FasApplications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class FasApplicationMapper
    {
        [MapProperty(nameof(FasApplication.ApplicationNumber), nameof(FasApplicationListItemDTO.Id))]
        [MapProperty($"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.Citizen)}.{nameof(Citizen.FullName)}", nameof(FasApplicationListItemDTO.ApplicantName))]
        [MapProperty($"{nameof(FasApplication.SchoolStudent)}.{nameof(SchoolStudent.EducationAccount)}.{nameof(EducationAccount.AccountNumber)}", nameof(FasApplicationListItemDTO.AccountNumber))]
        [MapProperty($"{nameof(FasApplication.FasScheme)}.{nameof(FasScheme.SchemeName)}", nameof(FasApplicationListItemDTO.SchemeName))]
        [MapProperty(nameof(FasApplication.CreatedAt), nameof(FasApplicationListItemDTO.SubmittedAt))]
        [MapProperty(nameof(FasApplication.Status), nameof(FasApplicationListItemDTO.Status))]
        [MapProperty(nameof(FasApplication.ValidityEndDate), nameof(FasApplicationListItemDTO.ValidityEndDate))]
        public partial FasApplicationListItemDTO MapToListItemDTO(FasApplication model);

        public partial IQueryable<FasApplicationListItemDTO> ProjectToListItemDTO(IQueryable<FasApplication> query);
    }
}
