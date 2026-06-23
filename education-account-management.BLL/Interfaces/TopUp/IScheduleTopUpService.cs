using DTOs.TopUp;
using Interfaces.Base;

namespace Interfaces.TopUp;

public interface IScheduleTopUpService : IBaseCrudService<CreateScheduleTopUpDTO, GetScheduleTopUpDTO, UpdateScheduleTopUpDTO>
{
    Task UpdateStatusesAsync(BatchUpdateScheduleTopUpStatusDTO dto, CancellationToken cancellationToken = default);
}