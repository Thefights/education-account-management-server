using Amazon.Runtime;
using Amazon.S3;
using Interfaces.Base;
using Interfaces.Storage;
using Services.Base;
using Services.Storage;

namespace Extensions.DependencyInjection;

public static class StorageServiceExtensions
{
    public static IServiceCollection AddStorageServices(
        this IServiceCollection services,
        AppConfiguration configuration)
    {
        services.AddScoped<IUploadService, UploadService>();
        services.AddScoped<IStorageService, R2StorageService>();
        services.AddScoped<IFileValidator, FileValidator>();

        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = $"https://{configuration.R2Config.AccountId}.r2.cloudflarestorage.com",
                ForcePathStyle = true,
                SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA256,
                MaxErrorRetry = 3,
                Timeout = TimeSpan.FromMinutes(5),
                AuthenticationRegion = "auto",
                BufferSize = 65536,
                DisableLogging = false,
                UseHttp = false
            };

            var credentials = new BasicAWSCredentials(
                configuration.R2Config.AccessKeyId,
                configuration.R2Config.SecretAccessKey);

            return new AmazonS3Client(credentials, config);
        });

        return services;
    }
}