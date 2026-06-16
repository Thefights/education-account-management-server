using Interfaces.Storage;
using Persistence.SqlServer.Transactions;

namespace Utils
{
    public static class ImageTransactionHookHelper
    {
        public static void RegisterUploadedImageRollback(
            IUnitOfWorkTransaction transaction,
            IUploadService? uploadService,
            string? uploadedImageUrl)
        {
            if (uploadService == null || string.IsNullOrWhiteSpace(uploadedImageUrl))
            {
                return;
            }

            transaction.OnRollback(() => DeleteUploadedImageIfPresentAsync(uploadService, uploadedImageUrl));
        }

        public static void RegisterOldImageDeleteAfterCommit(
            IUnitOfWorkTransaction transaction,
            IUploadService? uploadService,
            string? oldImageUrl,
            string? newImageUrl)
        {
            if (string.IsNullOrWhiteSpace(newImageUrl)
                || string.Equals(oldImageUrl, newImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            RegisterImageDeleteAfterCommit(transaction, uploadService, oldImageUrl);
        }

        public static void RegisterImageDeleteAfterCommit(
            IUnitOfWorkTransaction transaction,
            IUploadService? uploadService,
            string? imageUrl)
        {
            if (uploadService == null || string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            transaction.AfterCommit(() => DeleteUploadedImageIfPresentAsync(uploadService, imageUrl));
        }

        private static Task DeleteUploadedImageIfPresentAsync(
            IUploadService uploadService,
            string imageUrl)
        {
            return ImageUploadHelper.DeleteIfPresentAsync(uploadService, imageUrl, CancellationToken.None);
        }
    }
}
