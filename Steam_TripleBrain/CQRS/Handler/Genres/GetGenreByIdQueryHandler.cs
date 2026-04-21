using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Genres;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Genres
{
    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, Result<Profiles.GenreViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetGenreByIdQueryHandler> _logger;

        public GetGenreByIdQueryHandler(AppDbContext context, ILogger<GetGenreByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Profiles.GenreViewProfile>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (genre == null)
                return Result<Profiles.GenreViewProfile>.Failure("Genre not found");

            var profile = GenreMappingProfile.ToProfile(genre);
            return Result<Profiles.GenreViewProfile>.Success(profile);
        }
    }
}
