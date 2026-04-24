namespace Steam_TripleBrain.Services
{
    public class FileStorages : IFileStorageService
    {
        private static readonly HashSet<string> AllowedExt = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        private const long MaxBytes = 3 * 1024 * 1024;

        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileStorages> _logger;

        public FileStorages(IWebHostEnvironment env, ILogger<FileStorages> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> SaveProductImageAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }

            _logger.LogInformation("Saving product image: {FileName}, Size: {Size} bytes", file.FileName, file.Length);
            if (file.Length > MaxBytes)
            {
                _logger.LogInformation("Image is too large: {FileName}, Size: {Size} bytes", file.FileName, file.Length);
                throw new InvalidOperationException("Image is too large, must be less then 3 MB");
            }

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExt.Contains(ext))
            {
                throw new InvalidOperationException("Invalid image format");
            }

            var webRoot = _env.WebRootPath;
            var dir = Path.Combine(webRoot, "uploads", "products");
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var name = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(dir, name);

            await using var fs = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fs, ct);

            return $"/uploads/products/{name}";

        }

        public Task DeleteAsync(string? relativePath, CancellationToken ct = default)
        {
            _logger.LogInformation("Deleting file at relative path: {RelativePath}", relativePath);
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                _logger.LogInformation("No relative path provided for deletion.");
                return Task.CompletedTask;
            }
            var webRootPath = _env.WebRootPath;
            var trimmed = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(webRootPath, trimmed);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}
