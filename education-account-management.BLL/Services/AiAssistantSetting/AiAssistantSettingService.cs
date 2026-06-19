using DTOs.AiAssistantSetting;
using Interfaces.AiAssistantSetting;

namespace Services.AiAssistantSetting
{
    public class AiAssistantSettingService(
        IUnitOfWork unitOfWork,
        AiAssistantSettingMapper mapper,
        ICurrentUserService currentUserService)
        : IAiAssistantSettingService
    {
        private const int SingletonId = 1;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AiAssistantSettingMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<AiAssistantSetting> _repository = unitOfWork.Repository<AiAssistantSetting>();

        public async Task<GetAiAssistantSettingDTO> GetAsync(
            CancellationToken cancellationToken = default)
        {
            return await _repository.FirstOrDefaultProjectedAsync(
                    _mapper.ProjectToGetDTO,
                    cancellationToken: cancellationToken)
                ?? throw new DataNotFoundException(typeof(AiAssistantSetting), SingletonId);
        }

        public async Task<GetAiAssistantSettingDTO> UpdateAsync(
            UpdateAiAssistantSettingDTO updateDTO,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);

            var setting = await _repository.GetTrackedByIdAsync(
                    SingletonId,
                    cancellationToken: cancellationToken)
                ?? throw new DataNotFoundException(typeof(AiAssistantSetting), SingletonId);

            _mapper.MapFromUpdateDTO(updateDTO, setting);
            setting.UpdatedBy = _currentUserService.UserId;
            setting.TryValidate();

            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return _mapper.MapToGetDTO(setting);
        }
    }
}
