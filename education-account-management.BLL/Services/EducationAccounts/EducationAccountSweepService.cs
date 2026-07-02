using DTOs.EducationAccounts;
using Interfaces.EducationAccounts;
using Mappers.EducationAccounts;
using Services.EducationAccounts.Utils;
using Validators;
using Interfaces.Email;
using Interfaces.Notifications;

namespace Services.EducationAccounts
{
    public class EducationAccountSweepService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        INotificationWriter notificationWriter,
        IOutboxWriter outboxWriter,
        EmailTemplateBuilder emailTemplateBuilder,
        AppConfiguration configuration) : IEducationAccountSweepService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly INotificationWriter _notificationWriter = notificationWriter;
        private readonly IOutboxWriter _outboxWriter = outboxWriter;
        private readonly EmailTemplateBuilder _emailTemplateBuilder = emailTemplateBuilder;
        private readonly AppConfiguration _configuration = configuration;

        private readonly IGenericRepository<Citizen> _citizenRepository = unitOfWork.Repository<Citizen>();
        private readonly IGenericRepository<EducationAccount> _repository = unitOfWork.Repository<EducationAccount>();
        private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository = unitOfWork.Repository<EducationCreditTransaction>();
        private readonly IGenericRepository<Charge> _chargeRepository = unitOfWork.Repository<Charge>();
        private readonly IGenericRepository<EducationAccountStatusHistory> _statusHistoryRepository = unitOfWork.Repository<EducationAccountStatusHistory>();
        private readonly IGenericRepository<EducationAccountSweepReport> _sweepReportRepository = unitOfWork.Repository<EducationAccountSweepReport>();

        public async Task<EducationAccountSweepResultDTO> SweepAccountsAsync(CancellationToken cancellationToken = default)
        {
            var nowSgt = DateTime.UtcNow.AddHours(8);
            var batchDate = DateOnly.FromDateTime(nowSgt);
            var existingReport = await _sweepReportRepository.Query()
                .Include(report => report.Targets)
                .FirstOrDefaultAsync(report => report.BatchDate == batchDate, cancellationToken);

            if (existingReport != null)
                return EducationAccountSweepReportMapper.MapToResultDTO(existingReport);

            var result = new EducationAccountSweepResultDTO
            {
                BatchDate = batchDate,
                StartedAt = DateTime.UtcNow
            };

            var firstEligibleBirthDate = batchDate.AddYears(-16);
            var closingBirthDate = batchDate.AddYears(-31);
            var citizensToCreate = await _citizenRepository.Query()
                .Where(citizen => citizen.DateOfBirth <= firstEligibleBirthDate
                    && citizen.DateOfBirth > closingBirthDate
                    && !citizen.IsAutoSweepExcluded)
                .Where(citizen => citizen.EducationAccount == null)
                .OrderBy(citizen => citizen.Id)
                .ToListAsync(cancellationToken);

            var accountsToClose = await _repository.Query()
                .Where(account => account.Status != EducationAccountStatus.Closed)
                .Where(account => account.Citizen.DateOfBirth <= closingBirthDate)
                .OrderBy(account => account.Id)
                .Select(account => new
                {
                    account.Id,
                    account.CitizenId,
                    account.Citizen.Nric,
                    account.Status
                })
                .ToListAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                var reservedAccountNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var citizen in citizensToCreate)
                {
                    try
                    {
                        var accountNumber = await BusinessCodeGenerator.GenerateUniqueAsync(
                            BusinessCodeGenerator.EducationAccountPrefix,
                            (candidate, innerToken) => _repository.AnyAsync(
                                account => account.AccountNumber == candidate,
                                innerToken),
                            reservedCodes: reservedAccountNumbers,
                            conflictMessage: "Unable to generate a unique education account number.",
                            cancellationToken: token);

                        var account = new EducationAccount
                        {
                            AccountNumber = accountNumber,
                            CitizenId = citizen.Id
                        };
                        account.TryValidate();
                        await UniqueConstraintValidator.ValidateAsync(_repository, account, cancellationToken: token);
                        await _repository.AddAsync(account, token);

                        result.AccountsCreatedCount++;
                        result.Targets.Add(new EducationAccountSweepTargetDTO
                        {
                            CitizenId = citizen.Id,
                            Nric = citizen.Nric,
                            Action = SweepAction.Create,
                            Status = SweepTargetStatus.Success
                        });
                    }
                    catch (Exception exception) when (exception is not OperationCanceledException)
                    {
                        result.Targets.Add(new EducationAccountSweepTargetDTO
                        {
                            CitizenId = citizen.Id,
                            Nric = citizen.Nric,
                            Action = SweepAction.Create,
                            Status = SweepTargetStatus.Failed,
                            Reason = exception.GetBaseException().Message
                        });
                    }
                }

                foreach (var candidate in accountsToClose)
                {
                    try
                    {
                        var account = await _repository.Query(tracking: true)
                            .Include(item => item.Citizen)
                            .FirstOrDefaultAsync(item => item.Id == candidate.Id, token)
                            ?? throw new DataNotFoundException(typeof(EducationAccount), candidate.Id);

                        var finalStatus = await EducationAccountClosureHelper.CloseOrExtendAsync(
                            account,
                            _chargeRepository,
                            _transactionRepository,
                            "Education account balance expired at age 31.",
                            token);

                        _repository.Update(account);
                        if (candidate.Status != account.Status)
                        {
                            await AddStatusHistoryAsync(
                                account.Id,
                                candidate.Status,
                                account.Status,
                                account.Status == EducationAccountStatus.Extended
                                    ? "Education account extended because unpaid charges remain."
                                    : "Education account closed at age 31.",
                                token);
                        }

                        if (finalStatus == EducationAccountStatus.Extended
                            && candidate.Status != EducationAccountStatus.Extended)
                        {
                            if (!string.IsNullOrWhiteSpace(account.Citizen.Email))
                            {
                                var outstandingAmount = await _chargeRepository.Query(tracking: false)
                                    .Where(charge => charge.Enrollment.SchoolStudent.EducationAccountId == account.Id
                                        && charge.RemainingAmount > 0)
                                    .SumAsync(charge => charge.RemainingAmount, token);
                                var template = _emailTemplateBuilder.BuildEducationAccountExtendedEmail(
                                    account.Citizen.FullName,
                                    account.AccountNumber,
                                    outstandingAmount,
                                    BuildAccountHolderPortalLink("/account-holder/tuition-payment"));

                                await _outboxWriter.EnqueueEmailAsync(
                                    account.Citizen.Email,
                                    template,
                                    token);
                            }

                            result.AccountsExtendedCount++;
                            result.Targets.Add(new EducationAccountSweepTargetDTO
                            {
                                CitizenId = candidate.CitizenId,
                                Nric = candidate.Nric,
                                Action = SweepAction.Extend,
                                Status = SweepTargetStatus.Success
                            });
                        }
                        else if (finalStatus == EducationAccountStatus.Closed)
                        {
                            result.AccountsClosedCount++;
                            result.Targets.Add(new EducationAccountSweepTargetDTO
                            {
                                CitizenId = candidate.CitizenId,
                                Nric = candidate.Nric,
                                Action = SweepAction.Close,
                                Status = SweepTargetStatus.Success
                            });
                        }
                    }
                    catch (Exception exception) when (exception is not OperationCanceledException)
                    {
                        result.Targets.Add(new EducationAccountSweepTargetDTO
                        {
                            CitizenId = candidate.CitizenId,
                            Nric = candidate.Nric,
                            Action = candidate.Status == EducationAccountStatus.Active ? SweepAction.Close : SweepAction.Extend,
                            Status = SweepTargetStatus.Failed,
                            Reason = exception.GetBaseException().Message
                        });
                    }
                }

                result.CompletedAt = DateTime.UtcNow;
                var report = new EducationAccountSweepReport
                {
                    BatchDate = result.BatchDate,
                    StartedAt = result.StartedAt,
                    CompletedAt = result.CompletedAt,
                    AccountsCreatedCount = result.AccountsCreatedCount,
                    AccountsClosedCount = result.AccountsClosedCount,
                    AccountsExtendedCount = result.AccountsExtendedCount,
                    Targets = result.Targets.Select(item => new EducationAccountSweepTarget
                    {
                        CitizenId = item.CitizenId,
                        Nric = item.Nric,
                        Action = item.Action,
                        Status = item.Status,
                        Reason = item.Reason
                    }).ToList()
                };
                await UniqueConstraintValidator.ValidateAsync(_sweepReportRepository, report, cancellationToken: token);
                await _sweepReportRepository.AddAsync(report, token);
            }, cancellationToken);

            var savedReport = await _sweepReportRepository.Query()
                .FirstOrDefaultAsync(r => r.BatchDate == result.BatchDate, cancellationToken);

            var failedCount = result.Targets.Count(t => t.Status == SweepTargetStatus.Failed);

            var systemAdminUserIds = await _unitOfWork.Repository<User>()
                .Query()
                .Where(user => user.Role == UserRole.SystemAdmin &&
                    user.Status == UserStatus.Active)
                .Select(user => user.Id)
                .ToListAsync(cancellationToken);

            await _notificationWriter.CreateForUsersAsync(
                systemAdminUserIds,
                failedCount > 0
                    ? NotificationType.AccountSweepFailedRecords
                    : NotificationType.AccountSweepCompleted,
                failedCount > 0
                    ? NotificationSeverity.Warning
                    : NotificationSeverity.Success,
                failedCount > 0
                    ? "Account sweep completed with failed records"
                    : "Account sweep completed",
                $"Batch {result.BatchDate.ToString("yyyy-MM-dd")} completed: {result.AccountsCreatedCount} created, {failedCount} failed, {result.AccountsClosedCount} closed, {result.AccountsExtendedCount} extended.",
                nameof(EducationAccountSweepReport),
                savedReport?.Id,
                new
                {
                    result.BatchDate,
                    result.AccountsCreatedCount,
                    result.AccountsClosedCount,
                    result.AccountsExtendedCount,
                    failedCount
                },
                cancellationToken);

            return result;
        }

        private async Task AddStatusHistoryAsync(
            int educationAccountId,
            EducationAccountStatus previousStatus,
            EducationAccountStatus newStatus,
            string reason,
            CancellationToken cancellationToken)
        {
            var history = new EducationAccountStatusHistory
            {
                EducationAccountId = educationAccountId,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Reason = reason,
                ChangedAt = DateTime.UtcNow,
                ChangedByUserId = _currentUserService.CurrentUserId
            };
            history.TryValidate();
            await _statusHistoryRepository.AddAsync(history, cancellationToken);
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
