using DTOs.EducationAccounts;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class EducationAccountMapper
        : ICrudMapper<EducationAccount, CreateEducationAccountDTO, GetEducationAccountDTO, UpdateEducationAccountDTO>
    {
        [MapProperty(nameof(EducationAccount.Id), nameof(GetEducationAccountDTO.Id))]
        [MapProperty(nameof(EducationAccount.AccountNumber), nameof(GetEducationAccountDTO.AccountNumber))]
        [MapProperty("Citizen.Nric", nameof(GetEducationAccountDTO.Nric))]
        [MapProperty("Citizen.FullName", nameof(GetEducationAccountDTO.Name))]
        [MapProperty("Citizen.DateOfBirth", nameof(GetEducationAccountDTO.DateOfBirth))]
        [MapProperty("Citizen.Email", nameof(GetEducationAccountDTO.Email))]
        [MapProperty("Citizen.PhoneNumber", nameof(GetEducationAccountDTO.PhoneNumber))]
        [MapProperty("Citizen.ResidentialAddress", nameof(GetEducationAccountDTO.ResidentialAddress))]
        [MapProperty("Citizen.MailingAddress", nameof(GetEducationAccountDTO.MailingAddress))]
        [MapProperty(nameof(EducationAccount.Status), nameof(GetEducationAccountDTO.Status))]
        [MapProperty(nameof(EducationAccount.EducationCreditBalance), nameof(GetEducationAccountDTO.Balance))]
        [MapProperty(nameof(EducationAccount.OpenedAt), nameof(GetEducationAccountDTO.CreatedDate))]
        public partial GetEducationAccountDTO MapToGetDTO(EducationAccount model);

        public partial List<GetEducationAccountDTO> MapToGetDTOList(List<EducationAccount> models);

        public partial IQueryable<GetEducationAccountDTO> ProjectToGetDTO(IQueryable<EducationAccount> query);

        [MapProperty(nameof(CreateEducationAccountDTO.GeneratedAccountNumber), nameof(EducationAccount.AccountNumber))]
        [MapProperty(nameof(CreateEducationAccountDTO.ResolvedCitizenId), nameof(EducationAccount.CitizenId))]
        public partial EducationAccount MapFromCreateDTO(CreateEducationAccountDTO dto);

        public partial void MapFromUpdateDTO(UpdateEducationAccountDTO dto, EducationAccount model);
    }
}
