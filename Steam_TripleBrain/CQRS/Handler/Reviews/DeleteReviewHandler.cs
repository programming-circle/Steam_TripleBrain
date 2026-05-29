using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Reviews;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Reviews
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteReviewHandler> _logger;

        public DeleteReviewHandler(AppDbContext context, ILogger<DeleteReviewHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("Review not found");

            _context.Reviews.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            // Recalculate rating for the associated game
            var reviews = await _context.Reviews.Where(r => r.GameId == existing.GameId).ToListAsync(cancellationToken);
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == existing.GameId, cancellationToken);
            if (game != null)
            {
                game.Rating = reviews.Count > 0 ? reviews.Average(r => r.Rating) : 0;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<bool>.Success(true);
        }
    }
}
