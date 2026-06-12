using DTOs.Email;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class EmailWhitelistSettingMapper :
        IReadMapper<EmailWhitelistSetting, GetEmailWhitelistSettingDTO>,
        IUpdateMapper<UpdateEmailWhitelistSettingDTO, EmailWhitelistSetting>
    {
        public partial GetEmailWhitelistSettingDTO MapToGetDTO(EmailWhitelistSetting model);

        public partial List<GetEmailWhitelistSettingDTO> MapToGetDTOList(List<EmailWhitelistSetting> models);

        public partial void MapFromUpdateDTO(UpdateEmailWhitelistSettingDTO updateDTO, EmailWhitelistSetting model);

        public partial IQueryable<GetEmailWhitelistSettingDTO> ProjectToGetDTO(IQueryable<EmailWhitelistSetting> query);
    }
}
