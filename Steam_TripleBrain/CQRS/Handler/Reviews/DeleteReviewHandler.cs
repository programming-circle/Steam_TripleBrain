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

            return Result<bool>.Success(true);
        }
    }
}
