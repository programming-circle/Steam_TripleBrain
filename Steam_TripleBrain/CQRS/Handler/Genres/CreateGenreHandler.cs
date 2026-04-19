using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Genres;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Genres
{
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, Result<Genre>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateGenreHandler> _logger;

        public CreateGenreHandler(AppDbContext context, ILogger<CreateGenreHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Genre>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = new Genre
            {
                Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
                Name = request.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Genre>.Success(genre);
        }
    }
}
