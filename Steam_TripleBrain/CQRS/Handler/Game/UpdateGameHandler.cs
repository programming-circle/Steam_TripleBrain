using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Services;
using System.Net.Http;

namespace Steam_TripleBrain.CQRS.Handler.Game
{
    public class UpdateGameHandler : IRequestHandler<UpdateGameCommand, Result<GameViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateGameHandler> _logger;
        private readonly IFileStorageService _fileStorage;

        public UpdateGameHandler(
            AppDbContext context,
            ILogger<UpdateGameHandler> logger,
            IFileStorageService fileStorage)
        {
            _context = context;
            _logger = logger;
            _fileStorage = fileStorage;
        }

        private static bool IsManagedProductPath(string? path) =>
            !string.IsNullOrEmpty(path) &&
            path.StartsWith("/uploads/products/", StringComparison.OrdinalIgnoreCase);

        public async Task<Result<GameViewProfile>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateGameCommand for game id {GameId}", request.Id);

            var exists = await _context.Games.AnyAsync(g => g.Id == request.Id, cancellationToken);
            if (!exists)
                return Result<GameViewProfile>.Failure("No object to update");

            var game = await _context.Games
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (game == null)
                return Result<GameViewProfile>.Failure("Game not found.");

            var oldPoster = game.Poster;
            var oldImages = game.Images?.ToList() ?? new List<string>();

            if (!string.IsNullOrWhiteSpace(request.Poster))
            {
                string newPoster;
                try
                {
                    newPoster = await _fileStorage.SaveProductImageFromUriOrPathAsync(request.Poster.Trim(), cancellationToken);
                }
                catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or FileNotFoundException or HttpRequestException or IOException)
                {
                    _logger.LogWarning(ex, "Failed to persist poster on update");
                    return Result<GameViewProfile>.Failure($"Poster: {ex.Message}");
                }

                if (!string.Equals(oldPoster, newPoster, StringComparison.Ordinal) &&
                    IsManagedProductPath(oldPoster))
                {
                    await _fileStorage.DeleteAsync(oldPoster, cancellationToken);
                }

                game.Poster = newPoster;
            }

            if (request.Images != null)
            {
                var resolved = new List<string>();
                foreach (var src in request.Images)
                {
                    if (string.IsNullOrWhiteSpace(src))
                        continue;

                    try
                    {
                        resolved.Add(await _fileStorage.SaveProductImageFromUriOrPathAsync(src.Trim(), cancellationToken));
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or FileNotFoundException or HttpRequestException or IOException)
                    {
                        _logger.LogWarning(ex, "Failed to persist gallery image on update");
                        return Result<GameViewProfile>.Failure($"Image gallery: {ex.Message}");
                    }
                }

                foreach (var prev in oldImages)
                {
                    if (!IsManagedProductPath(prev))
                        continue;

                    var stillUsed = resolved.Exists(p => string.Equals(p, prev, StringComparison.OrdinalIgnoreCase));
                    if (!stillUsed)
                        await _fileStorage.DeleteAsync(prev, cancellationToken);
                }

                game.Images = resolved;
            }

            game.Name = request.Name;
            game.Rating = request.Rating;
            game.Description = request.Description;
            game.Price = request.Price;
            game.Discount = request.Discount;
            game.Developer = request.Developer;

            if (request.Genres == null || request.Genres.Count == 0)
                return Result<GameViewProfile>.Failure("At least one genre is required.");

            var oldGenres = game.Genres?.ToList() ?? new List<Steam_TripleBrain.Models.Genre>();
            if (oldGenres.Count > 0)
                _context.Genres.RemoveRange(oldGenres);

            game.Genres = request.Genres.Select(gv => new Steam_TripleBrain.Models.Genre
            {
                Id = gv.Id == Guid.Empty ? Guid.NewGuid() : gv.Id,
                Name = gv.Name
            }).ToList();
            await _context.SaveChangesAsync(cancellationToken);

            var profile = GameMappingProfile.ToProfile(game);
            return Result<GameViewProfile>.Success(profile, "Game updated successfully.");
        }
    }
}
