using MediatR;
using Steam_TripleBrain.CQRS.Command.Genre;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Genre
{
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, Result<GenreViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateGenreHandler> _logger;
        public CreateGenreHandler(AppDbContext context, ILogger<CreateGenreHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<GenreViewProfile>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start working of command CreateGenre");
            var exists = await _context.Genres.AnyAsync(g => g.Id == request.Id, cancellationToken);
            _logger.LogInformation("Checking if genre with ID {GenreId} exists: {Exists}", request.Id, exists);
            if (exists)
            {
                _logger.LogInformation("CreateGenre: object with this allready exists");
                return Result<GenreViewProfile>.Failure($"Genre with {request.Id}, not exists");
            }
            var genre = GenreMappingProfile.ToGenre(request);
            await _context.AddAsync(genre);
            _logger.LogInformation("Genre with ID {GenreId} added to context", genre.Id);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Genre with ID {GenreId} saved to database", genre.Id);
            var genreProfile = GenreMappingProfile.ToProfile(genre);
            return Result<GenreViewProfile>.Success(genreProfile, "Genre created successfully.");
        }
    }
}
