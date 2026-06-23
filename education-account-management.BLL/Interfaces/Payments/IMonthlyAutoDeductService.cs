using System.Threading;
using System.Threading.Tasks;

namespace Interfaces.Payments
{
    public interface IMonthlyAutoDeductService
    {
        Task AutoDeductOutstandingFeesAsync(CancellationToken cancellationToken = default);
    }
}
