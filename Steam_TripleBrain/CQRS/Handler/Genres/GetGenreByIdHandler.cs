using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Genres;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Genres
{
    public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdCommand, Result<Genre>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetGenreByIdHandler> _logger;

        public GetGenreByIdHandler(AppDbContext context, ILogger<GetGenreByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Genre>> Handle(GetGenreByIdCommand request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (genre == null)
                return Result<Genre>.Failure("Genre not found");

            return Result<Genre>.Success(genre);
        }
    }
}
