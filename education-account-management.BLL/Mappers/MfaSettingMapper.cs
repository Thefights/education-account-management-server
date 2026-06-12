using DTOs.Email;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class MfaSettingMapper :
        IReadMapper<MfaSetting, GetMfaSettingDTO>,
        IUpdateMapper<UpdateMfaSettingDTO, MfaSetting>
    {
        public partial GetMfaSettingDTO MapToGetDTO(MfaSetting model);

        public partial List<GetMfaSettingDTO> MapToGetDTOList(List<MfaSetting> models);

        public partial void MapFromUpdateDTO(UpdateMfaSettingDTO updateDTO, MfaSetting model);

        public partial IQueryable<GetMfaSettingDTO> ProjectToGetDTO(IQueryable<MfaSetting> query);
    }
}
