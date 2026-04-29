using MediatR;
using Steam_TripleBrain.CQRS.Command.Genre;
using Steam_TripleBrain.CQRS.Handler.Game;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Genre
{
    public class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, Result<GenreViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateGameHandler> _logger;

        public UpdateGenreHandler(AppDbContext appDbContext, ILogger<UpdateGameHandler> logger)
        {
            _context = appDbContext;
            _logger = logger;
        }

        public async Task<Result<GenreViewProfile>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateGenreCommand for genre: {GenreName}", request.Name);
            var exists = await _context.Genres.AnyAsync(g => g.Id == request.Id, cancellationToken);
            _logger.LogInformation("Checking if genre with ID {GenreId} exists: {Exists}", request.Id, exists);
            if (!exists)
            {
                _logger.LogInformation("No such object in DB");
                return Result<GenreViewProfile>.Failure("No object to update");
            }
            var genre = await _context.Genres.FindAsync(request.Id);
            if (genre == null)
            {
                _logger.LogWarning("Genre with ID {GenreId} not found when attempting to load for update.", request.Id);
                return Result<GenreViewProfile>.Failure("Genre not found.");
            }
            genre.Name = request.Name;
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Genre with ID {GenreId} updated in database", genre.Id);
            var genreProfile = GenreMappingProfile.ToProfile(genre);
            return Result<GenreViewProfile>.Success(genreProfile, "Genre updated successfully.");
        }
    }
}
