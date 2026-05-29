using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Services;
using System.Linq;
using System.Collections.Generic;
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
            var existsName = await _context.Games.AnyAsync(g => g.Name == request.Name, cancellationToken);
            if (exists || existsName)
            {
                _logger.LogWarning("Game with ID {Id} {Name} already exists. Cannot create.", request.Id ,request.Name);
                return Result<GameViewProfile>.Failure($"Game with ID {request.Id} or Name {request.Name} already exists.");
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

            // Attach existing genres by Id or Name instead of creating duplicates
            if (request.Genres != null && request.Genres.Count > 0)
            {
                var genreEntities = new List<Models.Genre>();
                foreach (var g in request.Genres)
                {
                    Models.Genre? existingGenre = null;
                    if (g.Id != Guid.Empty)
                    {
                        existingGenre = await _context.Genres.FindAsync(new object[] { g.Id }, cancellationToken);
                    }

                    if (existingGenre == null && !string.IsNullOrWhiteSpace(g.Name))
                    {
                        existingGenre = await _context.Genres.FirstOrDefaultAsync(x => x.Name == g.Name, cancellationToken);
                    }

                    if (existingGenre != null)
                    {
                        genreEntities.Add(existingGenre);
                    }
                    else
                    {
                        var newGenre = new Models.Genre { Id = g.Id == Guid.Empty ? Guid.NewGuid() : g.Id, Name = g.Name };
                        genreEntities.Add(newGenre);
                        // Add new genre to context so it will be persisted together with the game
                        await _context.Genres.AddAsync(newGenre, cancellationToken);
                    }
                }

                // Remove duplicates by Id
                game.Genres = genreEntities.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            }

            await _context.Games.AddAsync(game, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var gameViewProfile = GameMappingProfile.ToProfile(game);
            return Result<GameViewProfile>.Success(gameViewProfile, "Game created successfully.");
        }
    }
}
