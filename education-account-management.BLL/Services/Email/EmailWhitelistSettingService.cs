using AvepointMosPlatform.BLL;
using DTOs.Email;
using Infrastructure;
using Interfaces.Audit;
using Interfaces.Email;

namespace Services.Email
{
    public class EmailWhitelistSettingService(
        IUnitOfWork unitOfWork,
        EmailWhitelistSettingMapper emailWhitelistSettingMapper,
        IAuditLogWriter auditLogWriter,
        ICacheDataService cacheDataService,
        AppConfiguration configuration)
        : IEmailWhitelistSettingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly EmailWhitelistSettingMapper _emailWhitelistSettingMapper = emailWhitelistSettingMapper;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly ICacheDataService _cacheDataService = cacheDataService;
        private readonly AppConfiguration _configuration = configuration;
        private readonly IGenericRepository<EmailWhitelistSetting> _emailWhitelistSettingRepository = unitOfWork.Repository<EmailWhitelistSetting>();

        public async Task<GetEmailWhitelistSettingDTO> GetAsync(CancellationToken cancellationToken = default)
        {
            return await _cacheDataService.GetOrSetAsync(
                CacheKeys.EmailWhitelistSetting,
                SettingsCacheTtlHelper.GetSettingsTtl(_configuration),
                async token =>
                {
                    var setting = await _emailWhitelistSettingRepository
                        .Query()
                        .FirstOrDefaultAsync(token)
                        ?? throw new DataNotFoundException(typeof(EmailWhitelistSetting), 1);

                    return _emailWhitelistSettingMapper.MapToGetDTO(setting);
                },
                cancellationToken);
        }

        public async Task<GetEmailWhitelistSettingDTO> UpdateAsync(
            UpdateEmailWhitelistSettingDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            var setting = await _emailWhitelistSettingRepository
                .Query(tracking: true)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new DataNotFoundException(typeof(EmailWhitelistSetting), 1);

            _emailWhitelistSettingMapper.MapFromUpdateDTO(updateDTO, setting);
            setting.TryValidate();

            await _auditLogWriter.LogAsync(
                AuditLogCategory.EmailWhitelist,
                AuditLogAction.UpdateEmailWhitelistSetting,
                $"EmailWhitelistSetting:{setting.Id}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            await _cacheDataService.RemoveAsync(CacheKeys.EmailWhitelistSetting, cancellationToken);

            return _emailWhitelistSettingMapper.MapToGetDTO(setting);
        }
    }
}
