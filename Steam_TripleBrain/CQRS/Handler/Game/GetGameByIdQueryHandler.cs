using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Query.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Game
{
    public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQueryRequest, Result<GameViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetGameByIdQueryHandler> _logger;

        public GetGameByIdQueryHandler(AppDbContext context, ILogger<GetGameByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<GameViewProfile>> Handle(GetGameByIdQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start working of command GetGameById");

            Guid gameId = Guid.Parse(request.gameId);
            Models.Game game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                _logger.LogInformation("No game with {game.Id} exists",game.Id);
                return Result<GameViewProfile>.Failure($"game with id: {game.Id} not exist");
            }
            var gameViewProfile = GameMappingProfile.ToProfile(game);
            return Result<GameViewProfile>.Success(gameViewProfile, "Game created successfully.");
            //var exists = await _context.Games.AnyAsync(x => x.Id == request.Id, cancellationToken);

            //if (!exists)
            //{
            //    _logger.LogInformation("item {exists} don't exists in DB ", exists);
            //    return Result<GameViewProfile>.Failure("Product with this Id don't exists");
            //}

            //var game = await _context.Games.FindAsync(exists, cancellationToken);

            //var result = GameMappingProfile.ToProfile(game); // Again using my own mapper alternative system

            //return Result<GameViewProfile>.Success(result, "Product successfully found");
        }
    }
}
