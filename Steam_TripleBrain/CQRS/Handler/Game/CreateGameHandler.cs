using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Game
{
    public class CreateGameHandler : IRequestHandler<GetGameByIdCommand, Result<GameViewProfile>>
    {
        //Adding DB and logger
        private readonly AppDbContext _context;
        private readonly ILogger<CreateGameHandler> _logger;

        public CreateGameHandler(AppDbContext context, ILogger<CreateGameHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<GameViewProfile>> Handle(GetGameByIdCommand request, CancellationToken cancellationToken)
        {
            //Check if game with the same ID already exists
            _logger.LogInformation("Handling CreateGameCommand for game with ID {GameId}", request.Id);
            var exists = await _context.Games.AnyAsync(g => g.Id == request.Id, cancellationToken);
            _logger.LogInformation("Checking if game with ID {GameId} exists: {Exists}", request.Id, exists);

            if (exists)
            {   //Return failure result if game already exists
                _logger.LogWarning("Game with ID {GameId} already exists. Cannot create.", request.Id);
                return Result<GameViewProfile>.Failure($"Game with ID {request.Id} already exists.");
            }

            //I hate Mapper, so I'll do it manually
            var game = new Models.Game()
            {
                Id = request.Id,
                Name = request.Name,
                Poster = request.Poster,
                Images = request.Images,
                Rating = request.Rating,
                Description = request.Description,
                Genres = request.Genres,
                Tags = request.Tags,
                Price = request.Price,
                Discount = request.Discount,
                Author = request.Author,
                DLCs = request.DLCs
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync(cancellationToken);
            
            var gameViewProfile = new GameViewProfile()
            {
                Id = game.Id,
                Name = game.Name,
                Poster = game.Poster,
                Images = game.Images,
                Rating = game.Rating,
                Description = game.Description,
                Genres = game.Genres,
                Tags = game.Tags,
                Price = game.Price,
                Discount = game.Discount,
                Author = game.Author,
                DLCs = game.DLCs
            };
            return Result<GameViewProfile>.Success(gameViewProfile, "Game created successfully.");
        }
    }
}
