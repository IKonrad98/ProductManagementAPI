public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string bucketName, CancellationToken cancellationToken);
}