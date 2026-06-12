using Amazon.S3;
using Amazon.S3.Model;
using AvepointMosPlatform.BLL;
using Infrastructure;
using Interfaces.Storage;
using Polly;
using Polly.Registry;

namespace Services.Storage
{
    public class R2StorageService(
        IAmazonS3 s3Client,
        AppConfiguration configuration,
        ILogger<R2StorageService> logger,
        ResiliencePipelineProvider<string> resiliencePipelineProvider)
        : IStorageService
    {
        private readonly IAmazonS3 _s3Client = s3Client;
        private readonly AppConfiguration _configuration = configuration;
        private readonly ILogger<R2StorageService> _logger = logger;
        private readonly ResiliencePipeline _uploadPipeline = resiliencePipelineProvider
            .GetPipeline(ResiliencePipelineNames.R2Upload);
        private readonly ResiliencePipeline _deletePipeline = resiliencePipelineProvider
            .GetPipeline(ResiliencePipelineNames.R2Delete);

        public async Task UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var request = new PutObjectRequest
            {
                BucketName = _configuration.R2Config.Bucket,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead,
                UseChunkEncoding = false,
                AutoResetStreamPosition = true,
                AutoCloseStream = false
            };

            try
            {
                await _uploadPipeline.ExecuteAsync(
                    async token => await _s3Client.PutObjectAsync(request, token),
                    cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Failed to upload file {FileName} to R2.", fileName);
                throw new InternalAppException("File upload failed.", ex);
            }
        }

        public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _configuration.R2Config.Bucket,
                Key = fileName
            };

            try
            {
                await _deletePipeline.ExecuteAsync(
                    async token => await _s3Client.DeleteObjectAsync(request, token),
                    cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Failed to delete file {FileName} from R2.", fileName);
            }
        }
    }
}
