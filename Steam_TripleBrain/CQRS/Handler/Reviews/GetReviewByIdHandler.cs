using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Reviews;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Reviews
{
    public class GetReviewByIdHandler : IRequestHandler<GetReviewByIdQuery, Result<Profiles.Review>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetReviewByIdHandler> _logger;

        public GetReviewByIdHandler(AppDbContext context, ILogger<GetReviewByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Profiles.Review>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
            if (review == null)
                return Result<Profiles.Review>.Failure("Review not found");

            var profile = MappingProfile.ToProfile(review);
            return Result<Profiles.Review>.Success(profile);
        }
    }
}