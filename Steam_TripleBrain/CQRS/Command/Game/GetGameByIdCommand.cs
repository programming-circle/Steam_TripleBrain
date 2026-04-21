using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Game
{

    public class GetGameByIdCommand : IRequest<Result<GameViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetGameByIdCommand> _logger;

        public GetGameByIdCommand(AppDbContext context,ILogger<GetGameByIdCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public ImageUrlViewProfile Poster { get; set; }

        public List<ImageUrlViewProfile>? Images { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }
        public List<GenreViewProfile> Genres { get; set; }

        //public List<TagViewProfile>? Tags { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public Guid Author { get; set; }

        public List<DLCViewProfile>? DLCs { get; set; }

        //public async Task<Result<GameViewProfile>> HandleById(Guid requestId, CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("Start working of command GetGameById");
        //    var exists = await _context.Games.AnyAsync(x => x.Id == requestId, cancellationToken);

        //    if (!exists)
        //    {
        //        _logger.LogInformation("item {exists} don't exists in DB ", exists);
        //        return Result<GameViewProfile>.Failure("Product with this Id don't exists");
        //    }

        //    var game = await _context.Games.FindAsync(exists, cancellationToken);

        //    var result = GameMappingProfile.ToProfile(game); // Again using my own mapper alternative system

        //    return Result<GameViewProfile>.Success(result, "Product successfully found");
        //}

    }


}
