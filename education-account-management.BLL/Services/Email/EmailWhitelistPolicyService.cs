using AvepointMosPlatform.BLL;
using DTOs.Email;
using Infrastructure;
using Interfaces.Email;

namespace Services.Email
{
    public class EmailWhitelistPolicyService(
        IUnitOfWork unitOfWork,
        ICacheDataService cacheDataService,
        EmailWhitelistSettingMapper emailWhitelistSettingMapper,
        EmailWhitelistMapper emailWhitelistMapper,
        AppConfiguration configuration)
        : IEmailWhitelistPolicyService
    {
        private readonly ICacheDataService _cacheDataService = cacheDataService;
        private readonly EmailWhitelistSettingMapper _emailWhitelistSettingMapper = emailWhitelistSettingMapper;
        private readonly EmailWhitelistMapper _emailWhitelistMapper = emailWhitelistMapper;
        private readonly AppConfiguration _configuration = configuration;
        private readonly IGenericRepository<EmailWhitelist> _emailWhitelistRepository = unitOfWork.Repository<EmailWhitelist>();
        private readonly IGenericRepository<EmailWhitelistSetting> _emailWhitelistSettingRepository = unitOfWork.Repository<EmailWhitelistSetting>();

        public async Task<bool> CanSendAsync(string email, CancellationToken cancellationToken = default)
        {
            var setting = await GetEmailWhitelistSettingAsync(cancellationToken);

            if (setting?.IsEnabled != true)
            {
                return true;
            }

            var normalizedEmail = EmailWhitelistValueUtil.Normalize(email);
            if (string.IsNullOrWhiteSpace(normalizedEmail))
            {
                return false;
            }

            var atIndex = normalizedEmail.LastIndexOf('@');
            var domain = atIndex < 0 || atIndex == normalizedEmail.Length - 1
                ? string.Empty
                : normalizedEmail[(atIndex + 1)..];
            var whitelistValues = await GetEmailWhitelistValuesAsync(cancellationToken);
            return whitelistValues.Contains(normalizedEmail, StringComparer.OrdinalIgnoreCase)
                || whitelistValues.Contains(domain, StringComparer.OrdinalIgnoreCase);
        }

        private async Task<GetEmailWhitelistSettingDTO?> GetEmailWhitelistSettingAsync(CancellationToken cancellationToken)
        {
            return await _cacheDataService.GetOrSetAsync(
                CacheKeys.EmailWhitelistSetting,
                SettingsCacheTtlHelper.GetSettingsTtl(_configuration),
                async token =>
                {
                    var setting = await _emailWhitelistSettingRepository
                        .Query()
                        .FirstOrDefaultAsync(token);

                    return setting == null
                        ? null
                        : _emailWhitelistSettingMapper.MapToGetDTO(setting);
                },
                cancellationToken);
        }

        private async Task<List<string>> GetEmailWhitelistValuesAsync(CancellationToken cancellationToken)
        {
            var whitelistItems = await _cacheDataService.GetOrSetAsync(
                CacheKeys.EmailWhitelist,
                SettingsCacheTtlHelper.GetSettingsTtl(_configuration),
                async token => await _emailWhitelistRepository
                    .GetProjectedAsync(
                        _emailWhitelistMapper.ProjectToGetDTO,
                        cancellationToken: token),
                cancellationToken);

            return whitelistItems
                .Select(item => item.Value)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!)
                .ToList();
        }
    }
}
