using Enums;
using Interfaces.Email;
using Interfaces.FasApplications;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;

namespace Services.FasApplications
{
    public class FasBackgroundService(
        IUnitOfWork unitOfWork,
        IOutboxWriter outboxWriter,
        EmailTemplateBuilder emailTemplateBuilder,
        AppConfiguration configuration) : IFasBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOutboxWriter _outboxWriter = outboxWriter;
        private readonly EmailTemplateBuilder _emailTemplateBuilder = emailTemplateBuilder;
        private readonly AppConfiguration _configuration = configuration;

        public async Task<int> SweepExpiredApplicationsAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;
            var reminderDate = today.AddDays(14);
            var expiringApplications = await _unitOfWork.Repository<FasApplication>()
                .Query(tracking: false)
                .Include(application => application.FasScheme)
                .Include(application => application.SchoolStudent)
                    .ThenInclude(student => student.EducationAccount)
                        .ThenInclude(account => account.Citizen)
                .Where(application => application.Status == FasApplicationStatus.Approved
                    && application.ValidityEndDate.HasValue
                    && application.ValidityEndDate.Value.Date == reminderDate)
                .ToListAsync(cancellationToken);

            foreach (var application in expiringApplications)
            {
                var citizen = application.SchoolStudent.EducationAccount.Citizen;
                if (string.IsNullOrWhiteSpace(citizen.Email) || !application.ValidityEndDate.HasValue)
                {
                    continue;
                }

                var template = _emailTemplateBuilder.BuildFasValidityExpiringSoonEmail(
                    citizen.FullName,
                    application.FasScheme.SchemeName,
                    application.ApplicationNumber,
                    application.ValidityEndDate.Value,
                    BuildAccountHolderPortalLink("/account-holder/fas/management"));

                await _outboxWriter.EnqueueEmailOnceAsync(citizen.Email, template, cancellationToken);
            }

            if (expiringApplications.Count > 0)
            {
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }

            var expiredApplications = await _unitOfWork.Repository<FasApplication>()
                .Query()
                .Include(application => application.FasScheme)
                .Include(application => application.SchoolStudent)
                    .ThenInclude(student => student.EducationAccount)
                        .ThenInclude(account => account.Citizen)
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

                var citizen = application.SchoolStudent.EducationAccount.Citizen;
                if (!string.IsNullOrWhiteSpace(citizen.Email) && application.ValidityEndDate.HasValue)
                {
                    var template = _emailTemplateBuilder.BuildFasExpiredReapplyEmail(
                        citizen.FullName,
                        application.FasScheme.SchemeName,
                        application.ApplicationNumber,
                        application.ValidityEndDate.Value,
                        BuildAccountHolderPortalLink("/account-holder/fas/management"));

                    await _outboxWriter.EnqueueEmailAsync(citizen.Email, template, cancellationToken);
                }
            }
            _unitOfWork.Repository<FasApplication>().UpdateRange(expiredApplications);

            // UoW.SaveChangeAsync sẽ được gọi ở tầng Worker để đảm bảo tính đồng bộ với Audit Log.

            return expiredApplications.Count;
        }

        private string BuildAccountHolderPortalLink(string path)
        {
            var frontendUrl = _configuration.UrlsConfig?.FrontendUrl?.Trim();
            if (string.IsNullOrWhiteSpace(frontendUrl))
            {
                return "#";
            }

            return $"{frontendUrl.TrimEnd('/')}{path}";
        }
    }
}
