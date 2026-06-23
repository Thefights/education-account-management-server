using DTOs.TopUp;
using Riok.Mapperly.Abstractions;

namespace Mappers.TopUp
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class SystemTopupMapper : ICrudMapper<SystemTopup, CreateSystemTopupDTO, GetSystemTopupDTO, UpdateSystemTopupDTO>
    {
        public partial GetSystemTopupDTO MapToGetDTO(SystemTopup model);
        public partial List<GetSystemTopupDTO> MapToGetDTOList(List<SystemTopup> models);
        public partial IQueryable<GetSystemTopupDTO> ProjectToGetDTO(IQueryable<SystemTopup> query);
        public partial SystemTopup MapFromCreateDTO(CreateSystemTopupDTO createDTO);
        public partial void MapFromUpdateDTO(UpdateSystemTopupDTO updateDTO, SystemTopup model);
    }
}