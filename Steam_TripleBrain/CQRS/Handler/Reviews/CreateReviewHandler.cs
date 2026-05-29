using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Reviews;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Reviews
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<Review>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateReviewHandler> _logger;

        public CreateReviewHandler(AppDbContext context, ILogger<CreateReviewHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Review>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = MappingProfile.ToReview(request);
            review.CreatedAt = DateTime.UtcNow;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(cancellationToken);

            // Recalculate game rating
            var reviews = await _context.Reviews.Where(r => r.GameId == review.GameId).ToListAsync(cancellationToken);
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == review.GameId, cancellationToken);
            if (game != null && reviews.Count > 0)
            {
                game.Rating = reviews.Average(r => r.Rating);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<Review>.Success(review);
        }
    }
}
