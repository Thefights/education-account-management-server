using DTOs.Email;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class EmailWhitelistMapper
    {
        public partial GetEmailWhitelistDTO MapToGetDTO(EmailWhitelist model);

        public partial List<GetEmailWhitelistDTO> MapToGetDTOList(List<EmailWhitelist> models);

        public partial IQueryable<GetEmailWhitelistDTO> ProjectToGetDTO(IQueryable<EmailWhitelist> query);
    }
}
