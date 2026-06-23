using DTOs.TopUp;
using Riok.Mapperly.Abstractions;

namespace Mappers.TopUp;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class ScheduleTopUpMapper : ICrudMapper<ScheduleTopUp, CreateScheduleTopUpDTO, GetScheduleTopUpDTO, UpdateScheduleTopUpDTO>
{
    public partial GetScheduleTopUpDTO MapToGetDTO(ScheduleTopUp model);
    public partial List<GetScheduleTopUpDTO> MapToGetDTOList(List<ScheduleTopUp> models);
    public partial IQueryable<GetScheduleTopUpDTO> ProjectToGetDTO(IQueryable<ScheduleTopUp> query);
    public partial ScheduleTopUp MapFromCreateDTO(CreateScheduleTopUpDTO createDTO);
    public partial void MapFromUpdateDTO(UpdateScheduleTopUpDTO updateDTO, ScheduleTopUp model);
}