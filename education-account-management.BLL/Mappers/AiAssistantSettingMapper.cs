using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class AiAssistantSettingMapper :
        IReadMapper<AiAssistantSetting, GetAiAssistantSettingDTO>,
        IUpdateMapper<UpdateAiAssistantSettingDTO, AiAssistantSetting>
    {
        public partial GetAiAssistantSettingDTO MapToGetDTO(AiAssistantSetting model);

        public partial List<GetAiAssistantSettingDTO> MapToGetDTOList(List<AiAssistantSetting> models);

        public partial IQueryable<GetAiAssistantSettingDTO> ProjectToGetDTO(IQueryable<AiAssistantSetting> query);

        public partial void MapFromUpdateDTO(UpdateAiAssistantSettingDTO updateDTO, AiAssistantSetting model);
    }
}