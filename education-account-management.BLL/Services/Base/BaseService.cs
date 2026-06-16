using Common;
using DTOs.Base;
using Interfaces.Base;
using Interfaces.Storage;
using Mappers.Base;
using Repositories.Interfaces;
using Utils;
using Validators;

namespace Services.Base
{
    public class BaseService<TModel, TCreateDTO, TGetDTO, TUpdateDTO>(
        IUnitOfWork unitOfWork,
        ICrudMapper<TModel, TCreateDTO, TGetDTO, TUpdateDTO> mapper,
        IUploadService? uploadService = null,
        string[]? includes = null)
            : BaseGetService<TModel, TGetDTO>(unitOfWork, mapper, includes),
                IBaseCrudService<TCreateDTO, TGetDTO, TUpdateDTO>
                where TModel : BaseEntity
    {
        protected readonly ICrudMapper<TModel, TCreateDTO, TGetDTO, TUpdateDTO> _mapper = mapper;
        protected readonly IUploadService? _uploadService = uploadService;

        #region Write
        public virtual async Task<TGetDTO> CreateAsync(TCreateDTO createDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDTO);

            var entityId = await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    var uploadedImageUrl = await ImageUploadHelper.UploadIfPresentAsync(
                        _uploadService,
                        createDTO as IUploadImageDTO,
                        typeof(TModel).Name,
                        token);
                    ImageTransactionHookHelper.RegisterUploadedImageRollback(
                        transaction,
                        _uploadService,
                        uploadedImageUrl);

                    var entity = _mapper.MapFromCreateDTO(createDTO);
                    ImageUploadHelper.SetImageUrlIfPresent(entity, uploadedImageUrl);
                    entity.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(_repository, entity, cancellationToken: token);

                    var resultEntity = await _repository.AddAsync(entity, token);
                    await _unitOfWork.SaveChangeAsync(token);

                    return resultEntity.Id;
                },
                cancellationToken);

            return await this.GetByIdAsync(entityId, cancellationToken);
        }

        public virtual async Task<TGetDTO> UpdateAsync(int id, TUpdateDTO updateDTO, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDTO);

            var entityId = await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    var entity = await _repository.GetTrackedByIdAsync(id, cancellationToken: token)
                         ?? throw new DataNotFoundException(typeof(TModel), id);

                    var oldImageUrl = ImageUploadHelper.GetImageUrl(entity);
                    var uploadedImageUrl = await ImageUploadHelper.UploadIfPresentAsync(
                        _uploadService,
                        updateDTO as IUploadImageDTO,
                        typeof(TModel).Name,
                        token);
                    ImageTransactionHookHelper.RegisterUploadedImageRollback(
                        transaction,
                        _uploadService,
                        uploadedImageUrl);
                    ImageTransactionHookHelper.RegisterOldImageDeleteAfterCommit(
                        transaction,
                        _uploadService,
                        oldImageUrl,
                        uploadedImageUrl);

                    _mapper.MapFromUpdateDTO(updateDTO, entity);
                    ImageUploadHelper.SetImageUrlIfPresent(entity, uploadedImageUrl);
                    entity.TryValidate();
                    await UniqueConstraintValidator.ValidateAsync(_repository, entity, entity.Id, token);

                    var resultEntity = _repository.Update(entity);
                    return resultEntity.Id;
                },
                cancellationToken);

            return await this.GetByIdAsync(entityId, cancellationToken);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    var entity = await _repository.GetTrackedByIdAsync(id, cancellationToken: token)
                        ?? throw new DataNotFoundException(typeof(TModel), id);

                    var imageUrl = ImageUploadHelper.GetImageUrl(entity);
                    ImageTransactionHookHelper.RegisterImageDeleteAfterCommit(
                        transaction,
                        _uploadService,
                        imageUrl);

                    _repository.Remove(entity);
                },
                cancellationToken);
        }

        public virtual async Task DeleteSelectedIdsAsync(List<int> ids, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.ExecuteInTransactionAsync(
                async (transaction, token) =>
                {
                    var entities = await _repository.GetTrackedByIdsAsync(ids, cancellationToken: token);
                    if (entities.Count != ids.Count)
                    {
                        var foundIds = entities.Select(e => e.Id).ToHashSet();
                        var firstNotFoundId = ids.FirstOrDefault(id => !foundIds.Contains(id));
                        throw new DataNotFoundException(typeof(TModel), firstNotFoundId);
                    }

                    foreach (var imageUrl in entities.Select(ImageUploadHelper.GetImageUrl))
                    {
                        ImageTransactionHookHelper.RegisterImageDeleteAfterCommit(
                            transaction,
                            _uploadService,
                            imageUrl);
                    }

                    _repository.RemoveRange(entities);
                },
                cancellationToken);
        }
        #endregion

    }
}
