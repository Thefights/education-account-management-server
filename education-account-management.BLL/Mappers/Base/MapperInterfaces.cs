namespace Mappers.Base
{
    public interface IReadMapper<TModel, TGetDTO>
    {
        TGetDTO MapToGetDTO(TModel model);
        List<TGetDTO> MapToGetDTOList(List<TModel> models);
        IQueryable<TGetDTO> ProjectToGetDTO(IQueryable<TModel> query);
    }

    public interface ICreateMapper<TCreateDTO, TModel>
    {
        TModel MapFromCreateDTO(TCreateDTO createDTO);
    }

    public interface IUpdateMapper<TUpdateDTO, TModel>
    {
        void MapFromUpdateDTO(TUpdateDTO updateDTO, TModel model);
    }

    public interface ICrudMapper<TModel, TCreateDTO, TGetDTO, TUpdateDTO> :
        IReadMapper<TModel, TGetDTO>,
        ICreateMapper<TCreateDTO, TModel>,
        IUpdateMapper<TUpdateDTO, TModel>;
}
