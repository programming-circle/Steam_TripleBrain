using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Game
{
    public class UpdateGameHandler : IRequestHandler<UpdateGameCommand, Result<GameViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateGameHandler> _logger;

        public UpdateGameHandler(AppDbContext appDbContext, ILogger<UpdateGameHandler> logger)
        {
            _context = appDbContext;
            _logger = logger;
        }
        public async Task<Result<GameViewProfile>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateGameCommand for game: {GameName}", request.Name);
            var exists = await _context.Games.AnyAsync(g => g.Id == request.Id, cancellationToken);
            _logger.LogInformation("Checking if game with ID {GameId} exists: {Exists}", request.Id, exists);
            if (!exists)
            {
                _logger.LogInformation("No such object in DB");
                return Result<GameViewProfile>.Failure("No object to update");
            }
            // Load the existing game with related data
            var game = await _context.Games
                .Include(g => g.Poster)
                .Include(g => g.Images)
                .Include(g => g.Genres)
                //.Include(g => g.Tags)
                //.Include(g => g.DLCs)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (game == null)
            {
                _logger.LogWarning("Game with ID {GameId} not found when attempting to load for update.", request.Id);
                return Result<GameViewProfile>.Failure("Game not found.");
            }

            // Update scalar properties
            game.Name = request.Name;
            game.Rating = request.Rating;
            game.Description = request.Description;
            game.Price = request.Price;
            game.Discount = request.Discount;
            game.Developer = request.Developer;

            // Update poster
            if (request.Poster != null)
            {
                game.Poster = request.Poster;
            }

            // Update collections: replace with incoming lists (defensive copy)
            game.Images = request.Images ?? null;

            game.Genres = request.Genres?.Select(g => new GenreViewProfile
            {
                Id = g.Id == Guid.Empty ? Guid.NewGuid() : g.Id,
                Name = g.Name
            }).ToList() ?? new List<GenreViewProfile>();
            /*
            game.Tags = request.Tags?.Select(t => new Tag
            {
                Id = t.Id == Guid.Empty ? Guid.NewGuid() : t.Id,
                Name = t.Name
            }).ToList();*/
            /*
            game.DLCs = request.DLCs?.Select(d => new DLC
            {
                Id = d.Id == Guid.Empty ? Guid.NewGuid() : d.Id,
                Name = d.Name,
                Price = d.Price,
                Discount = d.Discount,
                Description = d.Description,
                Game = game
            }).ToList();*/

            // Persist changes
            _context.Games.Update(game);
            await _context.SaveChangesAsync(cancellationToken);

            var profile = GameMappingProfile.ToProfile(game);
            return Result<GameViewProfile>.Success(profile, "Game updated successfully.");
        }
    }
}
