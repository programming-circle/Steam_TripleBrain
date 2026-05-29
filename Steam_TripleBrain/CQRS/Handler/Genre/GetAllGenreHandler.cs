using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Genre;
using Steam_TripleBrain.CQRS.Query.Genre;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Genre
{
    public class GetAllGenreHandler : IRequestHandler<GetAllGenreQuery, Result<List<GenreViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllGenreHandler> _logger;
        public GetAllGenreHandler(AppDbContext context, ILogger<GetAllGenreHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<GenreViewProfile>>> Handle(GetAllGenreQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start working of query GetAllGenre");
            //Guid genreId = request.genreId;
            IQueryable<Models.Genre> query = _context.Genres.AsNoTracking();
            var profiles = query.Select(GenreMappingProfile.ToProfile).ToList();
            return Result<List<GenreViewProfile>>.Success(profiles, "Ok");
        }
    }
}
