using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Reviews;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Reviews
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, Result<List<Profiles.Review>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllReviewsHandler> _logger;

        public GetAllReviewsHandler(AppDbContext context, ILogger<GetAllReviewsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<Profiles.Review>>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _context.Reviews.ToListAsync(cancellationToken);
            var profiles = reviews.Select(r => MappingProfile.ToProfile(r)).ToList();
            return Result<List<Profiles.Review>>.Success(profiles);
        }
    }
}