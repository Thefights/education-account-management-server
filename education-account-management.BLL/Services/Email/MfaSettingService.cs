using AvepointMosPlatform.BLL;
using DTOs.Email;
using Infrastructure;
using Interfaces.Audit;
using Interfaces.Email;

namespace Services.Email
{
    public class MfaSettingService(
        IUnitOfWork unitOfWork,
        MfaSettingMapper mfaSettingMapper,
        IAuditLogWriter auditLogWriter,
        ICacheDataService cacheDataService,
        AppConfiguration configuration)
        : IMfaSettingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly MfaSettingMapper _mfaSettingMapper = mfaSettingMapper;
        private readonly IAuditLogWriter _auditLogWriter = auditLogWriter;
        private readonly ICacheDataService _cacheDataService = cacheDataService;
        private readonly AppConfiguration _configuration = configuration;
        private readonly IGenericRepository<MfaSetting> _mfaSettingRepository = unitOfWork.Repository<MfaSetting>();

        public async Task<GetMfaSettingDTO> GetAsync(CancellationToken cancellationToken = default)
        {
            return await _cacheDataService.GetOrSetAsync(
                CacheKeys.MfaSetting,
                SettingsCacheTtlHelper.GetSettingsTtl(_configuration),
                async token =>
                {
                    var setting = await _mfaSettingRepository
                        .Query()
                        .FirstOrDefaultAsync(token)
                        ?? throw new DataNotFoundException(typeof(MfaSetting), 1);

                    return _mfaSettingMapper.MapToGetDTO(setting);
                },
                cancellationToken);
        }

        public async Task<GetMfaSettingDTO> UpdateAsync(
            UpdateMfaSettingDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            var setting = await _mfaSettingRepository
                .Query(tracking: true)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new DataNotFoundException(typeof(MfaSetting), 1);

            _mfaSettingMapper.MapFromUpdateDTO(updateDTO, setting);
            setting.TryValidate();

            await _auditLogWriter.LogAsync(
                AuditLogCategory.SecuritySetting,
                AuditLogAction.UpdateMfaSetting,
                $"MfaSetting:{setting.Id}",
                cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            await _cacheDataService.RemoveAsync(CacheKeys.MfaSetting, cancellationToken);

            return _mfaSettingMapper.MapToGetDTO(setting);
        }
    }
}
