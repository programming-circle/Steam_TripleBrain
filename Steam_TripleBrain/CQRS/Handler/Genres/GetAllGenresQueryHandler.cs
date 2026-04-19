using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Genres;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Genres
{
    public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, Result<List<GenreViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllGenresQueryHandler> _logger;

        public GetAllGenresQueryHandler(AppDbContext context, ILogger<GetAllGenresQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<GenreViewProfile>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _context.Genres.ToListAsync(cancellationToken);
            var profiles = genres.Select(g => MappingProfile.ToProfile(g)).ToList();
            return Result<List<GenreViewProfile>>.Success(profiles);
        }
    }
}
