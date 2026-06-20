using DTOs.TopUp;
using Interfaces.Base;

namespace Interfaces.TopUp
{
    public interface ITopupScheduleService : IBaseCrudService<CreateTopupScheduleDTO, GetTopupScheduleDTO, UpdateTopupScheduleDTO>
    {
    }
}
