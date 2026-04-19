using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Reviews;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Reviews
{
    public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, Result<Review>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateReviewHandler> _logger;

        public UpdateReviewHandler(AppDbContext context, ILogger<UpdateReviewHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Review>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<Review>.Failure("Review not found");

            var updated = MappingProfile.ToReview(request);
            updated.Id = existing.Id;
            updated.CreatedAt = existing.CreatedAt;

            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Review>.Success(existing);
        }
    }
}
