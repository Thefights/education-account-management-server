using DTOs.Audit;
using Mappers.Base;
using Models;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class AuditLogMapper : IReadMapper<AuditLog, GetAuditLogDTO>
    {
        [MapProperty(nameof(AuditLog.ActorUser.Role), nameof(GetAuditLogDTO.ActorUserRole))]
        public partial GetAuditLogDTO MapToGetDTO(AuditLog model);

        public partial List<GetAuditLogDTO> MapToGetDTOList(List<AuditLog> models);

        public partial IQueryable<GetAuditLogDTO> ProjectToGetDTO(IQueryable<AuditLog> query);

        private string? MapPayloadJson(string? payloadJson)
            => MaskingHelper.MaskPayload(payloadJson);
    }
}
