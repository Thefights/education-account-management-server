using DTOs.TopUp;
using Interfaces.Base;

namespace Interfaces.TopUp;

public interface ISystemTopupService : IBaseCrudService<CreateSystemTopupDTO, GetSystemTopupDTO, UpdateSystemTopupDTO>
{
    Task UpdateStatusesAsync(BatchUpdateSystemTopupStatusDTO dto, CancellationToken cancellationToken = default);
}
