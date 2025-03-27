using Minio;
using Minio.DataModel.Args;

namespace ProductManagementAPI.Services;

public class MinioStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;

    public MinioStorageService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string bucketName, CancellationToken cancellationToken)
    {
        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName), cancellationToken);

        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName), cancellationToken);
        }

        var objectName = $"{Guid.NewGuid()}_{file.FileName}";

        using var stream = file.OpenReadStream();

        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType), cancellationToken);

        var imageUrl = $"http://localhost:9000/{bucketName}/{objectName}";
        return imageUrl;
    }

    public async Task DeleteFileAsync(string objectName, string bucketName, CancellationToken cancellationToken)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName), cancellationToken);
    }
}