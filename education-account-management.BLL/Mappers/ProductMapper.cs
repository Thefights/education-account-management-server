using DTOs.Product;
using Riok.Mapperly.Abstractions;

namespace Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
    public partial class ProductMapper : ICrudMapper<Product, CreateProductDTO, GetProductDTO, UpdateProductDTO>
    {
        public partial GetProductDTO MapToGetDTO(Product model);

        public partial List<GetProductDTO> MapToGetDTOList(List<Product> models);

        [MapperIgnoreSource(nameof(CreateProductDTO.ImageUrl))]
        public partial Product MapFromCreateDTO(CreateProductDTO createDTO);

        [MapperIgnoreSource(nameof(UpdateProductDTO.ImageUrl))]
        public partial void MapFromUpdateDTO(UpdateProductDTO updateDTO, Product model);

        public partial IQueryable<GetProductDTO> ProjectToGetDTO(IQueryable<Product> query);
    }
}
