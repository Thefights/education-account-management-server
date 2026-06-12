using DTOs.Base;
using Interfaces.Storage;

namespace Utils
{
    public static class ImageUploadHelper
    {
        public static async Task<string?> UploadIfPresentAsync(
            IUploadService? uploadService,
            IUploadImageDTO? imageDTO,
            string folder,
            CancellationToken cancellationToken = default)
        {
            if (uploadService == null || imageDTO?.ImageUrl is not { Length: > 0 })
            {
                return null;
            }

            var uploadResult = await uploadService.UploadAsync(imageDTO.ImageUrl, folder, cancellationToken);
            return uploadResult.FileName;
        }

        public static async Task DeleteIfPresentAsync(
            IUploadService uploadService,
            string? imageUrl,
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                await uploadService.DeleteAsync(imageUrl, cancellationToken);
            }
        }

        public static async Task DeleteOldIfReplacedAsync(
            IUploadService uploadService,
            string? oldImageUrl,
            string? newImageUrl,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(oldImageUrl)
                || string.IsNullOrWhiteSpace(newImageUrl)
                || string.Equals(oldImageUrl, newImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            await uploadService.DeleteAsync(oldImageUrl, cancellationToken);
        }

        public static string? GetImageUrl(object entity)
        {
            return entity is EntityWithImage entityWithImage
                ? entityWithImage.ImageUrl
                : null;
        }

        public static void SetImageUrlIfPresent(object entity, string? imageUrl)
        {
            if (entity is EntityWithImage entityWithImage && !string.IsNullOrWhiteSpace(imageUrl))
            {
                entityWithImage.ImageUrl = imageUrl;
            }
        }
    }
}
