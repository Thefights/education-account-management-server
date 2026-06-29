using DTOs.Audit;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class ManagementActionLogMapper : IReadMapper<ManagementActionLog, GetManagementActionLogDTO>
    {
        [MapProperty(nameof(ManagementActionLog.ActorUser.Role), nameof(GetManagementActionLogDTO.ActorUserRole))]
        [MapProperty(
            $"{nameof(ManagementActionLog.ActorUser)}.{nameof(User.AdminProfile)}.{nameof(AdminProfile.FullName)}",
            nameof(GetManagementActionLogDTO.ActorFullName))]
        [MapProperty(
            $"{nameof(ManagementActionLog.ActorUser)}.{nameof(User.AdminProfile)}.{nameof(AdminProfile.Email)}",
            nameof(GetManagementActionLogDTO.ActorEmail))]
        public partial GetManagementActionLogDTO MapToGetDTO(ManagementActionLog model);

        public partial List<GetManagementActionLogDTO> MapToGetDTOList(List<ManagementActionLog> models);

        public partial IQueryable<GetManagementActionLogDTO> ProjectToGetDTO(IQueryable<ManagementActionLog> query);
    }
}
