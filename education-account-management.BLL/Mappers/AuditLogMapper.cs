using DTOs.Audit;
using Mappers.Base;
using Models;
using Riok.Mapperly.Abstractions;

namespace education_account_management.BLL.Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class AuditLogMapper : IReadMapper<AuditLog, GetAuditLogDTO>
    {
        public partial GetAuditLogDTO MapToGetDTO(AuditLog model);

        public partial List<GetAuditLogDTO> MapToGetDTOList(List<AuditLog> models);

        public partial IQueryable<GetAuditLogDTO> ProjectToGetDTO(IQueryable<AuditLog> query);
    }
}
