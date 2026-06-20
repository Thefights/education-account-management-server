using DTOs.EducationAccounts;
using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.EducationAccounts
{
    public interface IEducationAccountSweepService
    {
        Task<EducationAccountSweepResultDTO> SweepAccountsAsync(
            CancellationToken cancellationToken = default);
    }
}
