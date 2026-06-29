using DTOs.ApplicationSettings;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class ApplicationSettingMapper :
        IReadMapper<ApplicationSetting, GetApplicationSettingDTO>,
        IUpdateMapper<UpdateApplicationSettingDTO, ApplicationSetting>
    {
        public partial GetApplicationSettingDTO MapToGetDTO(ApplicationSetting model);

        public partial List<GetApplicationSettingDTO> MapToGetDTOList(List<ApplicationSetting> models);

        public partial IQueryable<GetApplicationSettingDTO> ProjectToGetDTO(IQueryable<ApplicationSetting> query);

        public partial void MapFromUpdateDTO(UpdateApplicationSettingDTO updateDTO, ApplicationSetting model);
    }
}