public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string bucketName, CancellationToken cancellationToken);

    Task DeleteFileAsync(string objectName, string bucketName, CancellationToken cancellationToken);
}