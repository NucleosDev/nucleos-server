using Microsoft.Extensions.Logging;

namespace Nucleos.Infrastructure.Services.FileStorage;

public class LocalFileStorageService : IFileStorageService, Application.Common.Interfaces.IFileStorageService
{
    private readonly ILogger<LocalFileStorageService> _logger;
    private readonly string _basePath;
    public LocalFileStorageService(ILogger<LocalFileStorageService> logger) { _logger = logger; _basePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads"); }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
    {
        Directory.CreateDirectory(_basePath);
        var path = Path.Combine(_basePath, $"{Guid.NewGuid()}_{fileName}");
        await using var file = File.Create(path);
        await stream.CopyToAsync(file, ct);
        return $"/uploads/{Path.GetFileName(path)}";
    }

    public Task DeleteAsync(string fileUrl, CancellationToken ct = default)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileUrl.TrimStart('/'));
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }
}
