namespace Steam_TripleBrain.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveProductImageAsync(IFormFile file, CancellationToken ct = default);
        Task DeleteAsync(string? relativePath, CancellationToken ct = default);
    }
}
