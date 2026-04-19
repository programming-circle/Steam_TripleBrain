using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Genres;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Genres
{
    public class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, Result<Genre>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateGenreHandler> _logger;

        public UpdateGenreHandler(AppDbContext context, ILogger<UpdateGenreHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Genre>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<Genre>.Failure("Genre not found");

            existing.Name = request.Name;

            _context.Genres.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Genre>.Success(existing);
        }
    }
}
