using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Genres;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Genres
{
    public class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteGenreHandler> _logger;

        public DeleteGenreHandler(AppDbContext context, ILogger<DeleteGenreHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Genres.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("Genre not found");

            _context.Genres.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
