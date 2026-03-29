using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Game
{
    public class GetAllGamesHandler : IRequestHandler<GetAllGamesQueryRequest, Result<List<GameViewProfile>>>
    {

        private readonly AppDbContext _context;
        private readonly ILogger<GetAllGamesHandler> _logger;

        public GetAllGamesHandler(AppDbContext context, ILogger<GetAllGamesHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<GameViewProfile>>> Handle(GetAllGamesQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start working of command GetAllGames");
            var games = await _context.Games.ToListAsync(cancellationToken);

            var result = games.Select(GameMappingProfile.ToProfile).ToList();

            return Result<List<GameViewProfile>>.Success(result, "Products successfully found");
        }
    }
}
