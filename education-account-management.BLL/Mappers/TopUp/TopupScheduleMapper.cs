using DTOs.TopUp;
using Mappers.Base;
using Models;
using Riok.Mapperly.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class TopupScheduleMapper : ICrudMapper<TopupSchedule, CreateTopupScheduleDTO, GetTopupScheduleDTO, UpdateTopupScheduleDTO>
    {
        [MapProperty(nameof(TopupSchedule.TopupRule), nameof(GetTopupScheduleDTO.TopupRule))]
        public partial GetTopupScheduleDTO MapToGetDTO(TopupSchedule model);

        public partial List<GetTopupScheduleDTO> MapToGetDTOList(List<TopupSchedule> models);
        public partial IQueryable<GetTopupScheduleDTO> ProjectToGetDTO(IQueryable<TopupSchedule> query);

        public TopupSchedule MapFromCreateDTO(CreateTopupScheduleDTO createDTO) => MapToSchedule(createDTO);
        public void MapFromUpdateDTO(UpdateTopupScheduleDTO updateDTO, TopupSchedule model) => MapToSchedule(updateDTO, model);

        public partial TopupSchedule MapToSchedule(CreateTopupScheduleDTO dto);

        public partial void MapToSchedule(UpdateTopupScheduleDTO dto, TopupSchedule model);
    }
}
