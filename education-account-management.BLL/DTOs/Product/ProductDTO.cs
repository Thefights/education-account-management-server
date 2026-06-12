using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Product
{
    public class CreateProductDTO : IUploadImageDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        [MessageRequiredFile, AllowFileType(FileType.Image)]
        public IFormFile? ImageUrl { get; set; }
    }

    public class GetProductDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? Status { get; set; }
    }

    public class GetUserProductDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? Status { get; set; }

        public bool IsFavorited { get; set; }
    }

    public class UpdateProductDTO : IUploadImageDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? ImageUrl { get; set; }
    }

    public class UpdateProductsStatusDTO
    {
        [MinLength(1)]
        public List<int> ProductIds { get; set; } = [];

        [EnumDefined]
        public ProductStatus Status { get; set; }
    }
}
