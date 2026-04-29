using MediatR;
using Steam_TripleBrain.CQRS.Query.Genre;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Genre
{
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQueryRequest, Result<GenreViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetGenreByIdQueryHandler> _logger;
        public GetGenreByIdQueryHandler(AppDbContext context, ILogger<GetGenreByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<GenreViewProfile>> Handle(GetGenreByIdQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start working of command GetGenreById");
            //Guid genreId = request.genreId;
            Models.Genre genre = await _context.Genres.FindAsync(request.Id);
            if (genre == null)
            {
                _logger.LogInformation("No genre with {genre.Id} exists", request.Id);
                return Result<GenreViewProfile>.Failure($"genre with id: {request.Id} not exist");
            }
            _logger.LogInformation("Genre with {genre.Id} exists", request.Id);
            var genreViewProfile = GenreMappingProfile.ToProfile(genre);
            return Result<GenreViewProfile>.Success(genreViewProfile, "Genre created successfully.");
        }
    }
}
