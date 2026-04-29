using MediatR;
using Steam_TripleBrain.CQRS.Handler.OrderItem;
using Steam_TripleBrain.CQRS.Query.Genre;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Genre
{
    public class DeleteGenreByIdQueryHandler : IRequestHandler<GenreDeleteByIdQueryRequest, Result<GenreViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteOrderItemByIdHandler> _logger;

        public DeleteGenreByIdQueryHandler(AppDbContext context, ILogger<DeleteOrderItemByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<GenreViewProfile>> Handle(GenreDeleteByIdQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GenreDeleteByIdQuery for genre ID: {GenreId}", request.Id);
            var genre = await _context.Genres.FindAsync(request.Id);
            if (genre == null)
            {
                _logger.LogInformation("No genre with ID {GenreId} exists", request.Id);
                return Result<GenreViewProfile>.Failure($"Genre with id: {request.Id} not exist");
            }
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync(cancellationToken);
            var genreProfile = GenreMappingProfile.ToProfile(genre);
            _logger.LogInformation("Genre with ID {GenreId} deleted successfully", request.Id);
            return Result<GenreViewProfile>.Success(genreProfile, "Genre deleted successfully.");
        }
    }
}
