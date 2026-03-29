using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Game
{
    public class UpdateGameHandler : IRequestHandler<UpdateGameCommand, Result<GameViewProfile>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<UpdateGameHandler> _logger;

        public UpdateGameHandler(AppDbContext appDbContext, ILogger<UpdateGameHandler> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }
        public Task<Result<GameViewProfile>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateGameCommand for game: {GameName}", request.Name);
            //var game = new GetGameByIdCommand. ( request.Id , cancellationToken);
            return null;
        }
    }
}
