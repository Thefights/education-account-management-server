using DTOs.EducationAccounts;
using Interfaces.EducationAccounts;
using Mappers.EducationAccounts;
using Services.EducationAccounts.Utils;
using Validators;

namespace Services.EducationAccounts
{
    public class EducationAccountSweepService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService) : IEducationAccountSweepService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

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
                    && citizen.DateOfBirth > closingBirthDate)
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
                    account.Citizen.Nric,
                    account.Status
                })
                .ToListAsync(cancellationToken);

            await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
            {
                foreach (var citizen in citizensToCreate)
                {
                    try
                    {
                        var account = new EducationAccount
                        {
                            AccountNumber = EducationAccountHelper.GenerateNextAccountNumber(),
                            CitizenId = citizen.Id
                        };
                        account.TryValidate();
                        await UniqueConstraintValidator.ValidateAsync(_repository, account, cancellationToken: token);
                        await _repository.AddAsync(account, token);

                        result.AccountsCreatedCount++;
                        result.Targets.Add(new EducationAccountSweepTargetDTO
                        {
                            Nric = citizen.Nric,
                            Action = SweepAction.Create,
                            Status = SweepTargetStatus.Success
                        });
                    }
                    catch (Exception exception) when (exception is not OperationCanceledException)
                    {
                        result.Targets.Add(new EducationAccountSweepTargetDTO
                        {
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
                            result.AccountsExtendedCount++;
                            result.Targets.Add(new EducationAccountSweepTargetDTO
                            {
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
                        Nric = item.Nric,
                        Action = item.Action,
                        Status = item.Status,
                        Reason = item.Reason
                    }).ToList()
                };
                await UniqueConstraintValidator.ValidateAsync(_sweepReportRepository, report, cancellationToken: token);
                await _sweepReportRepository.AddAsync(report, token);
            }, cancellationToken);

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
    }
}
