namespace Nucleos.Infrastructure.Services.FileStorage;

// TODO: implementar com AWS SDK
public class S3FileStorageService : IFileStorageService, Application.Common.Interfaces.IFileStorageService
{
    public Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
        => throw new NotImplementedException("S3 não configurado.");
    public Task DeleteAsync(string fileUrl, CancellationToken ct = default)
        => throw new NotImplementedException("S3 não configurado.");
}
