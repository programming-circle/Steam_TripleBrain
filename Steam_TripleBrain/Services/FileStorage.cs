namespace Steam_TripleBrain.Services
{
    public class FileStorage : IFileStorageService
    {
        public const string HttpClientName = nameof(FileStorage);

        private static readonly HashSet<string> AllowedExt = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        private const long MaxBytes = 3 * 1024 * 1024;

        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileStorage> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public FileStorage(
            IWebHostEnvironment env,
            ILogger<FileStorage> logger,
            IHttpClientFactory httpClientFactory)
        {
            _env = env;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SaveProductImageAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException(nameof(file));

            _logger.LogInformation("IMG### Saving product image: {FileName}, Size: {Size} bytes", file.FileName, file.Length);
            if (file.Length > MaxBytes)
                throw new InvalidOperationException("Image is too large, must be less then 3 MB");

            var ext = NormalizeExtension(Path.GetExtension(file.FileName));
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExt.Contains(ext))
                throw new InvalidOperationException("Invalid image format");

            await using var stream = file.OpenReadStream();
            return await SaveStreamToProductsFolderAsync(stream, ext, file.Length, ct);
        }

        public async Task<string> SaveProductImageFromUriOrPathAsync(string urlOrPath, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urlOrPath))
                throw new ArgumentException("Image source is required.", nameof(urlOrPath));

            var trimmed = urlOrPath.Trim();

            if (trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return await SaveFromHttpAsync(trimmed, ct);
            }

            if (trimmed.StartsWith("/uploads/products/", StringComparison.OrdinalIgnoreCase))
            {
                var local = MapWebRelativeToPhysical(trimmed);
                if (File.Exists(local))
                    return trimmed.Replace('\\', '/');
            }

            if (File.Exists(trimmed))
                return await SaveFromLocalFileAsync(trimmed, ct);

            throw new FileNotFoundException("Image file not found or unsupported source.", trimmed);
        }

        public Task DeleteAsync(string? relativePath, CancellationToken ct = default)
        {
            _logger.LogInformation("IMG### Deleting file at relative path: {RelativePath}", relativePath);
            if (string.IsNullOrWhiteSpace(relativePath))
                return Task.CompletedTask;

            var fullPath = MapWebRelativeToPhysical(relativePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        private string MapWebRelativeToPhysical(string relativePath)
        {
            var trimmed = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(_env.WebRootPath, trimmed);
        }

        private async Task<string> SaveFromLocalFileAsync(string absolutePath, CancellationToken ct)
        {
            var ext = NormalizeExtension(Path.GetExtension(absolutePath));
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExt.Contains(ext))
                throw new InvalidOperationException("Invalid image format");

            var info = new FileInfo(absolutePath);
            if (info.Length > MaxBytes)
                throw new InvalidOperationException("Image is too large, must be less then 3 MB");

            await using var stream = File.OpenRead(absolutePath);
            return await SaveStreamToProductsFolderAsync(stream, ext, info.Length, ct);
        }

        private async Task<string> SaveFromHttpAsync(string url, CancellationToken ct)
        {
            using var client = _httpClientFactory.CreateClient(HttpClientName);
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var len = response.Content.Headers.ContentLength;
            if (len.HasValue && len.Value > MaxBytes)
                throw new InvalidOperationException("Image is too large, must be less then 3 MB");

            var mediaType = response.Content.Headers.ContentType?.MediaType;
            var ext = ResolveExtensionFromUrl(url, mediaType);

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            return await SaveStreamToProductsFolderAsync(stream, ext, len, ct);
        }

        private static string NormalizeExtension(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return ext;
            ext = ext.ToLowerInvariant();
            return ext == ".jpeg" ? ".jpg" : ext;
        }

        private static string ResolveExtensionFromUrl(string url, string? mediaType)
        {
            try
            {
                var pathExt = NormalizeExtension(Path.GetExtension(new Uri(url).AbsolutePath));
                if (!string.IsNullOrEmpty(pathExt) && AllowedExt.Contains(pathExt))
                    return pathExt;
            }
            catch (UriFormatException)
            {
                /* ignore */
            }

            var fromMime = mediaType?.ToLowerInvariant() switch
            {
                "image/jpeg" or "image/jpg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                _ => null
            };

            if (fromMime != null && AllowedExt.Contains(fromMime))
                return fromMime;

            throw new InvalidOperationException("Could not determine image type from URL or Content-Type.");
        }

        private async Task<string> SaveStreamToProductsFolderAsync(Stream stream, string ext, long? knownLength, CancellationToken ct)
        {
            ext = NormalizeExtension(ext);
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExt.Contains(ext))
                throw new InvalidOperationException("Invalid image format");

            if (knownLength.HasValue && knownLength.Value > MaxBytes)
                throw new InvalidOperationException("Image is too large, must be less then 3 MB");

            var webRoot = _env.WebRootPath;
            var dir = Path.Combine(webRoot, "uploads", "products");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var name = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(dir, name);

            await using var fs = new FileStream(fullPath, FileMode.Create);
            var buffer = new byte[8192];
            long total = 0;
            int read;
            while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
            {
                total += read;
                if (total > MaxBytes)
                {
                    fs.Close();
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                    throw new InvalidOperationException("Image is too large, must be less then 3 MB");
                }

                await fs.WriteAsync(buffer.AsMemory(0, read), ct);
            }

            _logger.LogInformation("IMG### Stored product image at {RelativePath}", $"/uploads/products/{name}");
            return $"/uploads/products/{name}";
        }
    }
}
