using DTOs.TopUp;
using Interfaces.Base;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.TopUp
{
    public interface ITopupRuleService : IBaseCrudService<CreateTopupRuleDTO, GetTopupRuleDTO, UpdateTopupRuleDTO>
    {
        Task UpdateRulesStatusAsync(BatchUpdateTopupRuleStatusDTO dto, CancellationToken cancellationToken = default);
    }
}
