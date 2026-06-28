using Enums;
using Interfaces.FasApplications;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;

namespace Services.FasApplications
{
    public class FasBackgroundService(IUnitOfWork unitOfWork) : IFasBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<int> SweepExpiredApplicationsAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;

            var expiredApplications = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Where(a => a.Status == FasApplicationStatus.Approved
                            && a.ValidityEndDate.HasValue
                            && a.ValidityEndDate.Value.Date < today)
                .ToListAsync(cancellationToken);

            if (expiredApplications.Count == 0)
            {
                return 0;
            }

            foreach (var application in expiredApplications)
            {
                application.Status = FasApplicationStatus.Expired;
                _unitOfWork.Repository<FasApplication>().Update(application);
            }

            // UoW.SaveChangeAsync sẽ được gọi ở tầng Worker để đảm bảo tính đồng bộ với Audit Log.

            return expiredApplications.Count;
        }
    }
}
