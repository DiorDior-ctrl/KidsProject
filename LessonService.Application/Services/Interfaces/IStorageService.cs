namespace LessonService.Application.Services.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(
        string bucketName,
        string objectName,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken = default);

    Task<string> GetPresignedUrlAsync(
        string bucketName,
        string objectName,
        int expirySeconds = 3600,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string bucketName,
        string objectName,
        CancellationToken cancellationToken = default);

    Task EnsureBucketExistsAsync(
        string bucketName,
        CancellationToken cancellationToken = default);
}