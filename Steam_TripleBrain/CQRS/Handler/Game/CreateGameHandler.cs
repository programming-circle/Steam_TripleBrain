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
    public class CreateGameHandler : IRequestHandler<CreateGameCommand, Result<GameViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateGameHandler> _logger;
        private readonly IFileStorageService _fileStorage;

        public CreateGameHandler(
            AppDbContext context,
            ILogger<CreateGameHandler> logger,
            IFileStorageService fileStorage)
        {
            _context = context;
            _logger = logger;
            _fileStorage = fileStorage;
        }

        public async Task<Result<GameViewProfile>> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateGameCommand for game with ID {GameId}", request.Id);

            var exists = await _context.Games.AnyAsync(g => g.Id == request.Id, cancellationToken);
            if (exists)
            {
                _logger.LogWarning("Game with ID {GameId} already exists. Cannot create.", request.Id);
                return Result<GameViewProfile>.Failure($"Game with ID {request.Id} already exists.");
            }

            string posterPath;
            try
            {
                posterPath = await _fileStorage.SaveProductImageFromUriOrPathAsync(request.Poster, cancellationToken);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or FileNotFoundException or HttpRequestException or IOException)
            {
                _logger.LogWarning(ex, "Failed to persist poster");
                return Result<GameViewProfile>.Failure($"Poster: {ex.Message}");
            }

            List<string>? galleryPaths = null;
            if (request.Images is { Count: > 0 })
            {
                galleryPaths = new List<string>();
                foreach (var src in request.Images)
                {
                    if (string.IsNullOrWhiteSpace(src))
                        continue;

                    try
                    {
                        galleryPaths.Add(await _fileStorage.SaveProductImageFromUriOrPathAsync(src.Trim(), cancellationToken));
                    }
                    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or FileNotFoundException or HttpRequestException or IOException)
                    {
                        _logger.LogWarning(ex, "Failed to persist gallery image");
                        return Result<GameViewProfile>.Failure($"Image gallery: {ex.Message}");
                    }
                }
            }

            var game = GameMappingProfile.ToGame(request);
            game.Poster = posterPath;
            game.Images = galleryPaths;

            await _context.Games.AddAsync(game, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var gameViewProfile = GameMappingProfile.ToProfile(game);
            return Result<GameViewProfile>.Success(gameViewProfile, "Game created successfully.");
        }
    }
}
