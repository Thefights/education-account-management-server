using AvepointMosPlatform.BLL;
using DTOs.Email;
using Infrastructure;
using Interfaces.Audit;
using Interfaces.Email;

namespace Services.Email
{
    public class EmailWhitelistService(
        IUnitOfWork unitOfWork,
        EmailWhitelistMapper emailWhitelistMapper,
        IAuditLogWriter auditLogWriter,
        ICacheDataService cacheDataService,
        AppConfiguration configuration)
        : IEmailWhitelistService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly EmailWhitelistMapper _emailWhitelistMapper = emailWhitelistMapper;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly ICacheDataService _cacheDataService = cacheDataService;
        private readonly AppConfiguration _configuration = configuration;
        private readonly IGenericRepository<EmailWhitelist> _emailWhitelistRepository = unitOfWork.Repository<EmailWhitelist>();

        public async Task<List<GetEmailWhitelistDTO>> GetAllAsync()
        {
            return await _cacheDataService.GetOrSetAsync(
                CacheKeys.EmailWhitelist,
                SettingsCacheTtlHelper.GetSettingsTtl(_configuration),
                async token => await _emailWhitelistRepository.GetProjectedAsync(
                    _emailWhitelistMapper.ProjectToGetDTO,
                    cancellationToken: token));
        }

        public async Task<List<GetEmailWhitelistDTO>> SaveAsync(SaveEmailWhitelistDTO saveDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(saveDTO);

            var values = EmailWhitelistValueUtil.ParseValues(saveDTO.Values);
            var invalidValues = values
                .Where(value => !EmailWhitelistValueUtil.IsValid(value))
                .ToList();
            if (invalidValues.Count != 0)
            {
                throw new InvalidDataException($"Invalid email/domain whitelist value(s): {string.Join(", ", invalidValues)}");
            }

            var existingEntities = await _emailWhitelistRepository
                .Query(tracking: true)
                .ToListAsync(cancellationToken);

            var valueSet = values.ToHashSet(StringComparer.OrdinalIgnoreCase);
            var existingValueSet = existingEntities
                .Select(entity => entity.Value)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var entitiesToRemove = existingEntities
                .Where(entity => !valueSet.Contains(entity.Value))
                .ToList();
            if (entitiesToRemove.Count != 0)
            {
                _emailWhitelistRepository.RemoveRange(entitiesToRemove);
            }

            var entitiesToAdd = values
                .Where(value => !existingValueSet.Contains(value))
                .Select(value => new EmailWhitelist { Value = value })
                .ToList();
            if (entitiesToAdd.Count != 0)
            {
                await _emailWhitelistRepository.AddRangeAsync(entitiesToAdd, cancellationToken);
            }

            await _auditLogWriter.LogAsync(
                AuditLogCategory.EmailWhitelist,
                AuditLogAction.SaveEmailWhitelist,
                $"EmailWhitelist:{values.Count}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            await _cacheDataService.RemoveAsync(CacheKeys.EmailWhitelist, cancellationToken);

            return await _emailWhitelistRepository.GetProjectedAsync(
                _emailWhitelistMapper.ProjectToGetDTO,
                entity => values.Contains(entity.Value),
                cancellationToken: cancellationToken);
        }
    }
}
