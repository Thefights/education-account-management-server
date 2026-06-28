using DTOs.ApplicationSettings;
using Interfaces.ApplicationSettings;


namespace Services.ApplicationSettings
{
    public class ApplicationSettingService(
        IUnitOfWork unitOfWork,
        ApplicationSettingMapper mapper,
        ICurrentUserService currentUserService)
        : IApplicationSettingService
    {
        private const int SingletonId = 1;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ApplicationSettingMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<ApplicationSetting> _repository = unitOfWork.Repository<ApplicationSetting>();

        public async Task<GetApplicationSettingDTO> GetAsync(
            CancellationToken cancellationToken = default)
        {
            return await _repository.FirstOrDefaultProjectedAsync(
                    _mapper.ProjectToGetDTO,
                    cancellationToken: cancellationToken)
                ?? throw new DataNotFoundException(typeof(ApplicationSetting), SingletonId);
        }

        public async Task<GetApplicationSettingDTO> UpdateAsync(
            UpdateApplicationSettingDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);

            var setting = await _repository.GetTrackedByIdAsync(
                    SingletonId,
                    cancellationToken: cancellationToken)
                ?? throw new DataNotFoundException(typeof(ApplicationSetting), SingletonId);

            _mapper.MapFromUpdateDTO(updateDTO, setting);
            setting.UpdatedBy = _currentUserService.UserId;
            setting.TryValidate();

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return _mapper.MapToGetDTO(setting);
        }
    }
}
