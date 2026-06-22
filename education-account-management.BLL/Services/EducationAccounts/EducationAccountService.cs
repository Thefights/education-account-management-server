using DTOs.EducationAccounts;
using Interfaces.Audit;
using Interfaces.EducationAccounts;
using Services.Base;
using Services.EducationAccounts.Utils;
using Validators;

namespace Services.EducationAccounts;

public class EducationAccountService(
    IUnitOfWork unitOfWork,
    EducationAccountMapper mapper,
    ICurrentUserService currentUserService,
    IAuditLogWriter auditLogWriter)
    : BaseService<EducationAccount, CreateEducationAccountDTO, GetEducationAccountDTO, UpdateEducationAccountDTO>(
        unitOfWork,
        mapper,
        includes: [nameof(EducationAccount.Citizen)]),
      IEducationAccountService
{
    private readonly EducationAccountMapper _educationAccountMapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
    private readonly IGenericRepository<Citizen> _citizenRepository = unitOfWork.Repository<Citizen>();
    private readonly IGenericRepository<EducationCreditTransaction> _transactionRepository =
        unitOfWork.Repository<EducationCreditTransaction>();
    private readonly IGenericRepository<Charge> _chargeRepository = unitOfWork.Repository<Charge>();
    private readonly IGenericRepository<EducationAccountStatusHistory> _statusHistoryRepository =
        unitOfWork.Repository<EducationAccountStatusHistory>();

    public override async Task<GetEducationAccountDTO> CreateAsync(
        CreateEducationAccountDTO createDTO,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(createDTO);
        createDTO.Nric = createDTO.Nric.Trim().ToUpperInvariant();

        var citizen = await GetEligibleCitizenAsync(createDTO.Nric, cancellationToken);

        var accountId = await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            createDTO.ResolvedCitizenId = citizen.Id;
            createDTO.GeneratedAccountNumber = EducationAccountHelper.GenerateNextAccountNumber();

            var account = _mapper.MapFromCreateDTO(createDTO);
            account.TryValidate();
            await UniqueConstraintValidator.ValidateAsync(_repository, account, cancellationToken: token);
            await _repository.AddAsync(account, token);
            await _unitOfWork.SaveChangeAsync(token);

            await _auditLogWriter.LogAsync(
                AuditLogCategory.AccountCreation,
                "CreateEducationAccount",
                citizen.Nric,
                token);

            return account.Id;
        }, cancellationToken);

        return await GetByIdAsync(accountId, cancellationToken);
    }

    public override async Task<GetEducationAccountDTO> UpdateAsync(
        int id,
        UpdateEducationAccountDTO updateDTO,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(updateDTO);

        await _unitOfWork.ExecuteInTransactionAsync(async (_, token) =>
        {
            var account = await _repository.Query(tracking: true)
                .Include(item => item.Citizen)
                .FirstOrDefaultAsync(item => item.Id == id, token)
                ?? throw new DataNotFoundException(typeof(EducationAccount), id);

            var oldStatus = account.Status;
            _mapper.MapFromUpdateDTO(updateDTO, account);

            if (account.Status == EducationAccountStatus.Closed)
            {
                await EducationAccountClosureHelper.CloseOrExtendAsync(
                    account,
                    _chargeRepository,
                    _transactionRepository,
                    "Education account closed by administrator.",
                    token);
            }
            else
            {
                account.ClosedAt = null;
            }

            account.TryValidate();
            _repository.Update(account);
            if (oldStatus != account.Status)
            {
                await AddStatusHistoryAsync(
                    account.Id,
                    oldStatus,
                    account.Status,
                    "Education account status updated by administrator.",
                    token);
            }

            await _auditLogWriter.LogAsync(
                AuditLogCategory.AccountCreation,
                "UpdateEducationAccount",
                account.Citizen.Nric,
                token);
        }, cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<GetEducationAccountDTO> GetAccountHolderProfileAsync(
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        return await _repository.FirstOrDefaultProjectedAsync(
                _educationAccountMapper.ProjectToGetDTO,
                account => account.Citizen.User != null && account.Citizen.User.Id == userId,
                _includes,
                cancellationToken)
            ?? throw new DataNotFoundException("Education account for the current account holder was not found.");
    }

    public async Task UpdateEducationAccountsStatusAsync(
        BatchUpdateEducationAccountStatusDTO dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (dto.Ids.Count == 0) return;

        await _unitOfWork.ExecuteInTransactionAsync(async (transaction, token) =>
        {
            var accounts = await _repository.Query()
                .Include(a => a.Citizen)
                .Where(a => dto.Ids.Contains(a.Id))
                .ToListAsync(token);

            foreach (var account in accounts)
            {
                var oldStatus = account.Status;
                account.Status = dto.Status;
                if (dto.Status == EducationAccountStatus.Closed)
                {
                    await EducationAccountClosureHelper.CloseOrExtendAsync(
                        account,
                        _chargeRepository,
                        _transactionRepository,
                        "Education account closed by administrator.",
                        token);
                }
                else
                {
                    account.ClosedAt = null;
                }

                account.TryValidate();
                _repository.Update(account);

                if (oldStatus != account.Status)
                {
                    await AddStatusHistoryAsync(
                        account.Id,
                        oldStatus,
                        account.Status,
                        "Education account status updated by administrator.",
                        token);
                }

                await _auditLogWriter.LogAsync(
                    AuditLogCategory.AccountCreation,
                    $"UpdateEducationAccountStatusTo{account.Status}",
                    account.Citizen.Nric,
                    token);
            }
        }, cancellationToken);
    }

    private async Task<Citizen> GetEligibleCitizenAsync(string nric, CancellationToken cancellationToken)
    {
        var citizen = await _citizenRepository.Query()
            .Include(item => item.EducationAccount)
            .FirstOrDefaultAsync(item => item.Nric == nric, cancellationToken)
            ?? throw new ValidationFailureException(nameof(CreateEducationAccountDTO.Nric), "Citizen not found in registry.");

        var todaySgt = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(8));
        var errorMessage = EducationAccountValidationHelper.ValidateCitizenEligibility(citizen, todaySgt);

        if (errorMessage != null)
        {
            throw new ValidationFailureException(nameof(CreateEducationAccountDTO.Nric), errorMessage);
        }

        return citizen;
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
